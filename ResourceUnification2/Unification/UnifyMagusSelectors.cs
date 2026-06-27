using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static System.Collections.Specialized.BitVector32;

namespace ResourceUnification2.Unification
{
    internal class UnifyMagusSelectors
    {
        public static void CleanUpScion()
        {
            Main.Log.Log("Starting cleanup of Eldritch Scion Arcana Selector");

            var ep = BlueprintTools.GetBlueprint<BlueprintFeature>("95e04a9e86aa9e64dad7122625b79c62");
            ep.HideInUI = true;
            ep.HideInCharacterSheetAndLevelUp = true;
            ep.RemoveComponents<AddAbilityResources>();
            ep.RemoveComponents<AddFacts>();
            //FeatureConfigurator.For("95e04a9e86aa9e64dad7122625b79c62").RemoveComponents(x => x is AddAbilityResources).RemoveComponents(x => x is AddFacts).SetHideInCharacterSheetAndLevelUp(true).SetHideInCharacterSheetAndLevelUp(true).Configure();
         
            ArchetypeConfigurator.For("d078b2ef073f2814c9e338a789d97b73")
                .ModifyRemoveFeatures(x=>x.m_Features.Remove(BlueprintTool.GetRef<BlueprintFeatureBaseReference>("3ce9bb90749c21249adc639031d5eed1")))
                .RemoveRemoveFeaturesEntry(3)
                .RemoveRemoveFeaturesEntry(6)
                .RemoveRemoveFeaturesEntry(9)
                .RemoveRemoveFeaturesEntry(12)
                .RemoveRemoveFeaturesEntry(15)
                .RemoveRemoveFeaturesEntry(18)
                .RemoveAddFeaturesEntry(3)
                .RemoveAddFeaturesEntry(6)
                .RemoveAddFeaturesEntry(9)
                .RemoveAddFeaturesEntry(12)
                .RemoveAddFeaturesEntry(15)
                .RemoveAddFeaturesEntry(18)

               .Configure();
           
        }


    }
}
