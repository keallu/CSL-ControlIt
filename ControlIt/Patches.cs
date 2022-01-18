using HarmonyLib;
using System;
using UnityEngine;
using System.Reflection;
using ColossalFramework.UI;
using ColossalFramework.PlatformServices;
using System.Collections.Generic;

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

    [HarmonyPatch(typeof(DLCPanel), "Refresh")]
    public static class DLCPanelRefreshPatch
    {
        static bool Prefix()
        {
            try
            {
                return !ModConfig.Instance.RestrictAdvertising;
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] DLCPanelRefreshPatch:Prefix -> Exception: " + e.Message);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(DLCPanelNew), "RefreshList")]
    public static class DLCPanelNewRefreshListPatch
    {
        static bool Prefix()
        {
            try
            {
                return !ModConfig.Instance.RestrictAdvertising;
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] DLCPanelNewRefreshListPatch:Prefix -> Exception: " + e.Message);
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
                        PublishedFileId publishedFileId = asset.publishedFileId;
                        if (publishedFileId != PublishedFileId.invalid)
                        {
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

                        ___m_NameLabel.text = (string)typeof(PackageEntry).GetMethod("FormatPackageName", BindingFlags.NonPublic | BindingFlags.Static).Invoke(__instance, new object[] { entryName, authorName, isWorkshopItem });
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] PackageEntrySetNameLabelPatch:Prefix -> Exception: " + e.Message);
            }
        }
    }
}
