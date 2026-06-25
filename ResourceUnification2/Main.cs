using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Owlcat.Runtime.Core.Logging;
using ResourceUnification2.Logic;
using System.Reflection;
using System.Text;
using UnityModManagerNet;

namespace ResourceUnification2;

public static class Main
{
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger Log;
    internal static UnificationModContext Context;

    public static bool Load(UnityModManager.ModEntry modEntry)
    {
        Log = modEntry.Logger;
        modEntry.OnGUI = OnGUI;
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        Context = new(modEntry);
        Context.ModEntry.OnSaveGUI = OnSaveGUI;
       
        try
        {
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch
        {
            HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
            throw;
        }
        return true;
    }

    public static void OnGUI(UnityModManager.ModEntry modEntry)
    {

    }

    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
        Context.SaveAllSettings();



    }

    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class BlueprintsCaches_Patch
    {
        private static bool Initialized = false;

        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static void Init_Postfix()
        {
            try
            {
                if (Initialized)
                {
                    Log.Log("Already initialized blueprints cache.");
                    return;
                }
                Initialized = true;

                Log.Log("Patching blueprints.");
                //MagusArcanePool.TestApply();
                // Insert your mod's patching methods here
                // Example
                // SuperAwesomeFeat.Configure()
            }
            catch (Exception e)
            {
                Log.Log(string.Concat("Failed to initialize.", e));
            }
        }
    }
    [HarmonyPriority(Priority.Last)]
    [HarmonyPatch(typeof(StartGameLoader), "LoadAllJson")]

    static class StartGameLoader_LoadAllJson
    {
        private static bool Run = false;

        static void Postfix()
        {
            //This is for running compat with KineticistExpandedElements
            if (Run) return; Run = true;
            try
            {
                BlueprintTool.GetRef<BlueprintArchetypeReference>("be63e4da1a53d6044a403cab0e0c5288");
                //MagusArcanePool.Thiefling();
            }
            catch
            {

            }
        }
    }
}
