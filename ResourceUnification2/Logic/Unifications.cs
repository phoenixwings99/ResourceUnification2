using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using ResourceUnification2.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace ResourceUnification2.Logic
{
    internal class Unifications
    {
        
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.Last)]
            static void Postfix()
            {
                   
                try
                {

                    BlueprintAbilityResource cavalierResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("672e8c9c98db1df4aa66676a66036e71");
                    if (cavalierResource.m_MaxAmount.StartingLevel == 4 && cavalierResource.m_MaxAmount.StartingIncrease == 1)
                    {
                        cavalierResource.m_MaxAmount.StartingLevel = 1;
                        cavalierResource.m_MaxAmount.StartingIncrease = 0;
                    }

                    Main.Log.Log($"Doing Built In Unifications");

                    ModifyTools.RegisterForProcessingAsBaseGameAnchor("Ki", "e9590244effb4be4f830b1e3fffced13");
                    ModifyTools.RegisterForProcessingAsBaseGameAnchor("ArcanePool", "3ce9bb90749c21249adc639031d5eed1");
                    ModifyTools.RegisterForProcessingAsBaseGameAnchor("ArcanistArcaneReservoir", "55db1859bd72fd04f9bd3fe1f10e4cbb");
                    ModifyTools.RegisterForProcessingAsBaseGameAnchor("AlchemistBomb", new GameResourceEntry { ResourceAdderFeatureGuid = "28384b1d7e25c8743b8bbfc56211ac8c", WrapperGuid = "c59b2f256f5a70a4d896568658315b7d" });

                    ModifyTools.RegisterForProcessingAsBaseGameAnchor("BardPerformance", "b92bfc201c6a79e49afd0b5cfbfc269f");

                    ModifyTools.RegisterForProcessing("Ki", "ae98ab7bda409ef4bb39149a212d6732", true);
                    ModifyTools.RegisterForProcessing("ArcanePool", "95e04a9e86aa9e64dad7122625b79c62", true);//EScion
                    ModifyTools.RegisterForProcessing("ArcanePool", "466c40aba50096341bf6532b1e53e8bd", true);//Armored Battlemage



                    ModifyTools.RegisterForProcessing("ArcanistArcaneReservoir", "9d1e2212594cf47438fff2fa3477b954", true);

                    //ModifyTools.RegisterForProcessing("BardPerformance", "02c96331ed2d87d43a4a3509142678b8", true);//Azata
                    ModifyTools.RegisterForProcessing("BardPerformance", "2b338db602044e91b1591e7f82332512", true);//Penta DLC5

                    foreach (CombinedScalingResourceEntry v in Main.Context.ResourceDefines.ClassScalingResourceEntries)
                    {
                        Main.Log.Log($"Processing {v.Key} from config");
                        foreach (GameResourceEntry classFeature in v.ClassResourceFeatureGuids)
                        {
                            ModifyTools.RegisterForProcessing(v.Key, classFeature, true);
                        }
                        foreach (GameResourceEntry nonClassFeature in v.NonClassResourceFeatureGuids)
                        {
                            ModifyTools.RegisterForProcessing(v.Key, nonClassFeature, false);
                        }


                    }
                    foreach (ResourceDefines mod in Main.Context.OtherModDefines)
                    {
                        foreach (CombinedScalingResourceEntry v in mod.ClassScalingResourceEntries)
                        {
                            foreach (GameResourceEntry classFeature in v.ClassResourceFeatureGuids)
                            {
                                ModifyTools.RegisterForProcessing(v.Key, classFeature, true);
                            }
                            foreach (GameResourceEntry nonClassFeature in v.NonClassResourceFeatureGuids)
                            {
                                ModifyTools.RegisterForProcessing(v.Key, nonClassFeature, false);
                            }
                        }
                    }

                    ModifyTools.Finish();


                }
                catch (Exception e)
                {

                    Main.Context.Logger.LogError(e, $"Error caught while applying unifications");
                }
            }
        }
        
    }
}

