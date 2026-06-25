using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using ResourceUnification2.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceUnification2.Unification
{
    internal class ManualUnifyAzata
    {
        internal static void Do()
        {
            FeatureConfigurator.For("02c96331ed2d87d43a4a3509142678b8")
                .RemoveComponents(x => x is AddAbilityResources)//Remove Add Azata Performance
                .EditComponent<IncreaseResourcesByClass>(x => x.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("e190ba276831b5c4fa28737e5e49e6a6"))//Point Increase Resources By Class at Bard Song
                .Configure();

            AbilityResourceConfigurator.For("83f8a1c45ed205a4a989b7826f5c0687")
                .AddComponent<ResourceRedirectComponent>(x => x.m_RedirectTo = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("e190ba276831b5c4fa28737e5e49e6a6"))//Engage redirect to Bardsong
                .Configure();

            AbilityResourceConfigurator.For("e190ba276831b5c4fa28737e5e49e6a6").AddComponent<ResourceSourceInfoComponent>(x =>
                {
                    x.m_Unlock = BlueprintTool.GetRef<BlueprintFeatureReference>("02c96331ed2d87d43a4a3509142678b8");
                    x.AltStat = Kingmaker.EntitySystem.Stats.StatType.Charisma;

                }
            ).Configure();

            //Note For Future Versions - keep eye out for mods with azata performance extenders and nuke and replace with bard performance extenders.
            
        }

        internal static void DoLate()
        {
            if (Main.IsDarkCodexInstalled() && Settings.IsEnabled("AzataPerformanceUnification"))
            {
                
                FeatureConfigurator.For("3ce468766ce044a4af920a7eab2f41e6").RemoveFromGroups(Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility).Configure();
                
            }
        }
    }
}
