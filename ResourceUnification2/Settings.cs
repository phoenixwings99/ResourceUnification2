using BlueprintCore.Utils;
using Kingmaker.Localization;
using Kingmaker.UI.SettingsUI;
using ModMenu.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Menu = ModMenu.ModMenu;

namespace ResourceUnification2
{
    internal class Settings
    {
        private static readonly string RootKey = "resunify.settings";
        private static readonly string RootStringKey = "ResUnify.Settings";


        internal static void Init()
        {
            Main.Log.Log("Initializing settings.");
            SettingsBuilder settings =
              SettingsBuilder.New(RootKey, GetString("Title"))
                .AddDefaultButton(OnDefaultsApplied);
            settings.AddToggle(MakeToggle("AzataPerformanceUnification", true, true));
            settings.AddToggle(MakeToggle("MagusSelectionUnification", true, true));

            Menu.AddSettings(settings);
        }
        

        internal static bool IsEnabled(string key)
        {
            return Menu.GetSettingValue<bool>(GetKey(key.ToLower()));
        }

        internal static T GetDD<T>(string key) where T : Enum
        {
            return Menu.GetSettingValue<T>(GetKey(key));
        }

        internal static bool IsDisabled(string key)
        {
            return !Menu.GetSettingValue<bool>(GetKey(key.ToLower()));
        }

        private static LocalizedString GetString(string key, bool usePrefix = true)
        {
            var fullKey = usePrefix ? $"{RootStringKey}.{key}" : key;
            return LocalizationTool.GetString(fullKey);
        }

        private static void OnDefaultsApplied()
        {
            Main.Log.Log($"Default settings restored.");
        }

        private static string GetKey(string partialKey)
        {
            return $"{RootKey}.{partialKey}".ToLower();
        }



        private static Toggle MakeToggle(string keyStub, bool defaultVal, bool hasDesc)
        {

            var toggle = Toggle.New($"{RootKey}.{keyStub.ToLower()}", defaultVal, LocalizationTool.GetString($"{RootStringKey}.{keyStub}"));
            if (hasDesc)
                toggle.WithLongDescription(LocalizationTool.GetString($"{RootStringKey}.{keyStub}.Desc"));

            return toggle;
        }

        private static Dropdown<T> MakeDropdown<T>(string keyStub, T defaultVal, UISettingsEntityDropdownEnum<T> dd, bool desc = false) where T : Enum
        {

            var dropper = Dropdown<T>.New(GetKey(keyStub), defaultVal, LocalizationTool.GetString($"{RootStringKey}.{keyStub}"), dd);
            if (desc)
                dropper.WithLongDescription(LocalizationTool.GetString($"{RootStringKey}.{keyStub}.Desc"));
            return dropper;

        }
    }
}
