using CitiesHarmony.API;
using ICities;
using UnityEngine;
using System.Reflection;

namespace ControlIt
{
    public class ModInfo : IUserMod
    {
        public string Name => "Control It!";
        public string Description => "Allows to control network traffic and some visual appearance to the main menu.";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());

            if (GameObject.Find("MenuContainer") != null)
            {
                GameObject modManagerGameObject = GameObject.Find("ControlItModManager");

                if (modManagerGameObject == null)
                {
                    modManagerGameObject = new GameObject("ControlItModManager");
                    modManagerGameObject.AddComponent<ModManager>();
                }
            }
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }

            GameObject modManagerGameObject = GameObject.Find("ControlItModManager");

            if (modManagerGameObject != null)
            {
                UnityEngine.Object.Destroy(modManagerGameObject);
            }
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;
            bool selected;
            float selectedValue;

            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();

            group = helper.AddGroup(Name + " - " + assemblyName.Version.Major + "." + assemblyName.Version.Minor);

            selected = ModConfig.Instance.ShowStatistics;
            group.AddCheckbox("Show Statistics in Main Menu", selected, sel =>
            {
                ModConfig.Instance.ShowStatistics = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Network Traffic (requires a restart of the game to take effect)");

            selected = ModConfig.Instance.RestrictNews;
            group.AddCheckbox("Restrict News", selected, sel =>
            {
                ModConfig.Instance.RestrictNews = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.RestrictAdvertising;
            group.AddCheckbox("Restrict Advertising (DLC and workshop)", selected, sel =>
            {
                ModConfig.Instance.RestrictAdvertising = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.RestrictUserGeneratedContentDetails;
            group.AddCheckbox("Restrict User Generated Content Details", selected, sel =>
            {
                ModConfig.Instance.RestrictUserGeneratedContentDetails = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.RestrictTelemetry;
            group.AddCheckbox("Restrict Telemetry", selected, sel =>
            {
                ModConfig.Instance.RestrictTelemetry = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Visual Appearance");

            selected = ModConfig.Instance.HideMenuBackground;
            group.AddCheckbox("Hide Menu Background", selected, sel =>
            {
                ModConfig.Instance.HideMenuBackground = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.NewsPanelOpacity;
            group.AddSlider("News Panel Opacity", 0.0f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.NewsPanelOpacity = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.AccountPanelOpacity;
            group.AddSlider("Account Panel Opacity", 0.0f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.AccountPanelOpacity = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.DLCPanelOpacity;
            group.AddSlider("DLC Panel Opacity", 0.0f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.DLCPanelOpacity = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.WorkshopPanelOpacity;
            group.AddSlider("Workshop Panel Opacity", 0.0f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.WorkshopPanelOpacity = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.HideChirper;
            group.AddCheckbox("Hide Chirper", selected, sel =>
            {
                ModConfig.Instance.HideChirper = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Advanced");

            selected = ModConfig.Instance.UpdateUserGeneratedContentDetailsWhenBrowsing;
            group.AddCheckbox("Update User Generated Content Details when browsing Content Manager", selected, sel =>
            {
                ModConfig.Instance.UpdateUserGeneratedContentDetailsWhenBrowsing = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.LogTelemetryEntriesToFile;
            group.AddCheckbox("Log Telemetry Entries to File", selected, sel =>
            {
                ModConfig.Instance.LogTelemetryEntriesToFile = sel;
                ModConfig.Instance.Save();
            });
        }
    }
}