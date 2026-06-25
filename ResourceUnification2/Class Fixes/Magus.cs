using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using ResourceUnification2.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.EnterNameDialogue;

namespace ResourceUnification2.Class_Fixes
{
    public static class Magus
    {
        public static void vanishEldritchPool()
        {

            return;
            ArchetypeConfigurator.For("d078b2ef073f2814c9e338a789d97b73")
                .RemoveFromAddFeatures(1, "95e04a9e86aa9e64dad7122625b79c62")
                .RemoveFromAddFeatures(3, "d4b54d9db4932454ab2899f931c2042c")
                .RemoveFromAddFeatures(6, "d4b54d9db4932454ab2899f931c2042c")
                .RemoveFromAddFeatures(9, "d4b54d9db4932454ab2899f931c2042c")
                .RemoveFromAddFeatures(12, "d4b54d9db4932454ab2899f931c2042c")
                .RemoveFromAddFeatures(15, "d4b54d9db4932454ab2899f931c2042c")
                .RemoveFromAddFeatures(18, "d4b54d9db4932454ab2899f931c2042c")
                .RemoveFromRemoveFeatures(1, "3ce9bb90749c21249adc639031d5eed1")
                .RemoveFromRemoveFeatures(3, "e9dc4dfc73eaaf94aae27e0ed6cc9ada")
                .RemoveFromRemoveFeatures(6, "e9dc4dfc73eaaf94aae27e0ed6cc9ada")
                .RemoveFromRemoveFeatures(9, "e9dc4dfc73eaaf94aae27e0ed6cc9ada")
                .RemoveFromRemoveFeatures(12, "e9dc4dfc73eaaf94aae27e0ed6cc9ada")
                .RemoveFromRemoveFeatures(15, "e9dc4dfc73eaaf94aae27e0ed6cc9ada")
                .RemoveFromRemoveFeatures(18, "e9dc4dfc73eaaf94aae27e0ed6cc9ada")
                .Configure();
            
        }

        public static void EScionSanityCheck()
        {
            //if (Settings.IsDisabled("CleanupEldritchScion"))
            //    return;

            BlueprintArchetype escion = BlueprintTool.Get<BlueprintArchetype>("d078b2ef073f2814c9e338a789d97b73");
           
            List<Tuple<int, string>> levelArcaneWeaponPairs = new();
            levelArcaneWeaponPairs.Add(new(5, "36b609a6946733c42930c55ac540416b"));
            levelArcaneWeaponPairs.Add(new(9, "70be888059f99a245a79d6d61b90edc5"));
            levelArcaneWeaponPairs.Add(new(13, "1804187264121cd439d70a96234d4ddb"));
            levelArcaneWeaponPairs.Add(new(17, "3cbe3e308342b3247ba2f4fbaf5e6307"));

            CleanArche(escion, levelArcaneWeaponPairs);
           
        }
        private static void CleanArche(BlueprintArchetype arche, List<Tuple<int, string>> levelArcaneWeaponPairs)
        {
            foreach (var pair in levelArcaneWeaponPairs)
            {
                var weapon = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(pair.Item2);
                var addlist = arche.AddFeatures.FirstOrDefault(x => x.Level == pair.Item1 && x.m_Features.Any(x => x.deserializedGuid.Equals(weapon.deserializedGuid)));
                var removeList = arche.RemoveFeatures.FirstOrDefault(x => x.Level == pair.Item1 && x.m_Features.Any(x => x.deserializedGuid.Equals(weapon.deserializedGuid)));
                if (addlist?.m_Features == null || removeList?.m_Features == null)
                    continue;

                bool killedAdd = addlist.m_Features.Remove(weapon);
                bool killedRemove = removeList.m_Features.Remove(weapon);
                if (killedAdd)
                    Main.Log.Log($"killed add element of add/remove cancelling pair on escion level {pair.Item1}");
                if (killedRemove)
                    Main.Log.Log($"killed remove element of add/remove cancelling pair on escion level {pair.Item1}");
                if (killedAdd != killedRemove)
                    Main.Log.Log($"redundancy kill incomplete on escion level {pair.Item1}");

            }
        }
    }
}
