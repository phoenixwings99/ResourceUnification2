using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceUnification2
{
    internal class Settings
    {
        private static readonly string RootKey = "lotp.settings";
        private static readonly string RootStringKey = "LOTP.Settings";
    

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
    }
}
