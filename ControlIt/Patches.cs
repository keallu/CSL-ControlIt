using ColossalFramework.PlatformServices;
using ColossalFramework.UI;
using ControlIt.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ControlIt
{
    [HarmonyPatch(typeof(MainMenu), "Awake")]
    public static class MainMenuAwakePatch
    {
        static void Postfix()
        {
            try
            {
                GameObject modManagerGameObject = GameObject.Find("ControlItModManager");

                if (modManagerGameObject == null)
                {
                    modManagerGameObject = new GameObject("ControlItModManager");
                    modManagerGameObject.AddComponent<ModManager>();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] MainMenuAwakePatch:Postfix -> Exception: " + e.Message);
            }
        }
    }

    [HarmonyPatch(typeof(MainMenu), "OnDestroy")]
    public static class MainMenuOnDestroyPatch
    {
        static void Prefix()
        {
            try
            {
                GameObject modManagerGameObject = GameObject.Find("ControlItModManager");

                if (modManagerGameObject != null)
                {
                    UnityEngine.Object.Destroy(modManagerGameObject);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] MainMenuOnDestroyPatch:Prefix -> Exception: " + e.Message);
            }
        }
    }

    [HarmonyPatch(typeof(NewsFeedPanel), "RefreshFeed")]
    public static class NewsFeedPanelRefreshFeedPatch
    {
        static bool Prefix()
        {
            try
            {
                return !ModConfig.Instance.RestrictNews;
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] NewsFeedPanelRefreshFeedPatch:Prefix -> Exception: " + e.Message);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(DLCPanel), "RefreshDLCOwnerships")]
    public static class DLCPanelRefreshDLCOwnershipsPatch
    {
        static bool Prefix()
        {
            try
            {
                return !ModConfig.Instance.RestrictAdvertising;
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] DLCPanelRefreshDLCOwnershipsPatch:Prefix -> Exception: " + e.Message);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(DLCPanel), "RefreshList")]
    public static class DLCPanelRefreshListPatch
    {
        static bool Prefix()
        {
            try
            {
                return !ModConfig.Instance.RestrictAdvertising;
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] DLCPanelRefreshListPatch:Prefix -> Exception: " + e.Message);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(WorkshopAdPanel), "Awake")]
    public static class WorkshopAdPanelAwakePatch
    {
        static void Prefix()
        {
            try
            {
                if (ModConfig.Instance.RestrictAdvertising)
                {
                    typeof(WorkshopAdPanel).GetField("dontInitialize", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, true);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] WorkshopAdPanelAwakePatch:Prefix -> Exception: " + e.Message);
            }
        }
    }

    [HarmonyPatch(typeof(CategoryContentPanel), "RequestDetails")]
    public static class CategoryContentPanelRequestDetailsPatch
    {
        static bool Prefix(ref Dictionary<PublishedFileId, List<EntryData>> ___m_DetailsPending, ref List<EntryData> ___m_Assets)
        {
            try
            {
                if (ModConfig.Instance.RestrictUserGeneratedContentDetails)
                {
                    ___m_DetailsPending = new Dictionary<PublishedFileId, List<EntryData>>();
                    foreach (EntryData asset in ___m_Assets)
                    {
                        if (asset.lastDataRequest != 0f && asset.lastDataRequest + 30f > Time.time)
                        {
                            continue;
                        }
                        PublishedFileId publishedFileId = asset.publishedFileId;
                        if (publishedFileId != PublishedFileId.invalid && asset.needUpdateData)
                        {
                            asset.lastDataRequest = Time.time;
                            if (___m_DetailsPending.TryGetValue(publishedFileId, out var value))
                            {
                                value.Add(asset);
                                continue;
                            }
                            List<EntryData> list = new List<EntryData>();
                            ___m_DetailsPending[publishedFileId] = list;
                            list.Add(asset);
                        }
                    }

                    Statistics.Instance.UserGeneratedContentDetailsRequestRestricted += ___m_DetailsPending.Count;

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] CategoryContentPanelRequestDetailsCoroutinePatch:Prefix -> Exception: " + e.Message);

                return true;
            }
        }
    }

    [HarmonyPatch(typeof(PackageEntry), "SetNameLabel")]
    public static class PackageEntrySetNameLabelPatch
    {
        static void Prefix(ref UILabel ___m_NameLabel, PackageEntry __instance, string entryName, string authorName)
        {
            try
            {
                if (ModConfig.Instance.RestrictUserGeneratedContentDetails && ModConfig.Instance.UpdateUserGeneratedContentDetailsWhenBrowsing)
                {
                    if (__instance.publishedFileId != PublishedFileId.invalid && string.IsNullOrEmpty(authorName))
                    {
                        PlatformService.workshop.RequestItemDetails(__instance.publishedFileId);

                        bool isWorkshopItem = (bool)typeof(PackageEntry).GetProperty("isWorkshopItem", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance, null);

                        ___m_NameLabel.text = ___m_NameLabel.text + (string)typeof(PackageEntry).GetMethod("FormatPackageName", BindingFlags.NonPublic | BindingFlags.Static).Invoke(__instance, new object[] { entryName, authorName, isWorkshopItem });
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] PackageEntrySetNameLabelPatch:Prefix -> Exception: " + e.Message);
            }
        }
    }

    public struct TelemetryEntry
    {
        public string eventName;

        public ICollection<KeyValuePair<string, string>> keyValuePairs;
    }

    [HarmonyPatch(typeof(PopsManager), "Send")]
    public static class PopsManagerSendPatch
    {
        static bool Prefix(TelemetryEntry telemetryEntry)
        {
            try
            {
                if (ModConfig.Instance.LogTelemetryEntriesToFile)
                {
                    LogHelper.WriteTelemetryEntryToFile(telemetryEntry.eventName, telemetryEntry.keyValuePairs.ToList());
                }

                if (ModConfig.Instance.RestrictTelemetry)
                {
                    Statistics.Instance.TelemetryEntriesSendRestricted++;

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] PopsManagerSendPatch:Prefix -> Exception: " + e.Message);

                return true;
            }
        }
    }

    [HarmonyPatch(typeof(PopsManager), "Buffer")]
    public static class PopsManagerBufferPatch
    {
        static bool Prefix(TelemetryEntry telemetryEntry)
        {
            try
            {
                if (ModConfig.Instance.LogTelemetryEntriesToFile)
                {
                    LogHelper.WriteTelemetryEntryToFile(telemetryEntry.eventName, telemetryEntry.keyValuePairs.ToList());
                }

                if (ModConfig.Instance.RestrictTelemetry)
                {
                    Statistics.Instance.TelemetryEntriesSendRestricted++;

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] PopsManagerBufferPatch:Prefix -> Exception: " + e.Message);

                return true;
            }
        }
    }
}
