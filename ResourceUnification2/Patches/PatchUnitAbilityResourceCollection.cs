using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using ResourceUnification2.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceUnification2.Patches
{
    
    internal class PatchUnitAbilityResourceCollection
    {
        [HarmonyPatch(typeof(UnitAbilityResourceCollection), "GetResource")]
        static class RedirectGetResource
        {
            public static void Prefix(UnitAbilityResource __result, UnitAbilityResourceCollection __instance, ref BlueprintScriptableObject blueprint)
            {
                ResourceRedirectComponent redirect = blueprint.Components?.OfType<ResourceRedirectComponent>().FirstOrDefault();
                if (redirect?.RedirectTo != null && !redirect.RedirectTo.Equals(blueprint.ToReference<BlueprintAbilityResourceReference>()))
                {
#if DEBUG
                    Main.Log.Log($"Redirecting from {blueprint.NameSafe()} to {redirect.RedirectTo.NameSafe()} in GetResource");
#endif

                    blueprint = redirect.RedirectTo;
                }
            }
        }

        [HarmonyPatch(typeof(UnitAbilityResourceCollection), "Add")]
        static class RedirectAdd
        {
            public static void Prefix(UnitAbilityResourceCollection __instance, ref BlueprintScriptableObject blueprint)
            {
                ResourceRedirectComponent redirect = blueprint.Components?.OfType<ResourceRedirectComponent>().FirstOrDefault();
                if (redirect?.RedirectTo != null && ! redirect.RedirectTo.Equals(blueprint.ToReference<BlueprintAbilityResourceReference>()))
                {
                    Main.Log.Log($"Redirecting from {blueprint.NameSafe()} to {redirect.RedirectTo.NameSafe()} in Add");
                    blueprint = redirect.RedirectTo;
                }
            }
        }

        [HarmonyPatch(typeof(UnitAbilityResourceCollection), "Remove")]
        static class RedirectRemove
        {
            public static void Prefix(UnitAbilityResourceCollection __instance, ref BlueprintScriptableObject blueprint)
            {
                ResourceRedirectComponent redirect = blueprint.Components?.OfType<ResourceRedirectComponent>().FirstOrDefault();
                if (redirect?.RedirectTo != null && !redirect.RedirectTo.Equals(blueprint.ToReference<BlueprintAbilityResourceReference>()))
                {
                    Main.Log.Log($"Redirecting from {blueprint.NameSafe()} to {redirect.RedirectTo.NameSafe()} in Remove");
                    blueprint = redirect.RedirectTo;
                }
            }
        }


    }
    
}
