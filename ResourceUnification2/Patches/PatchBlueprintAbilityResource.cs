using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using ResourceUnification2.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static ResourceUnification2.Components.ImprovedAbilityResourceCalc;

namespace ResourceUnification2.Patches
{
    
    internal class PatchBlueprintAbilityResource
    {
        [HarmonyPatch(typeof(BlueprintAbilityResource), "GetMaxAmount")]
        static class ApplyAltStatsToMax
        {

            [HarmonyPriority(Priority.Normal)]
            public static bool Prefix(ref int __result, BlueprintAbilityResource __instance, UnitDescriptor unit)
            {


                try
                {
                    if (TacticalCombatHelper.IsActive && BlueprintRoot.Instance.TacticalCombat.LeaderManaResource == __instance)
                    {
                        return true;
                    }
                    ImprovedAbilityResourceCalc customHandler = __instance.GetComponent<ImprovedAbilityResourceCalc>();
                    if (customHandler != null)
                    {
#if DEBUG
                        Main.Log.Log($"Starting Prefix Custom Logic on {__instance.name} with stat logic");
#endif
                        double runningTotal = 0;
                        float otherClassMultiplier = customHandler.OtherClassesModifier;
                        
                        if (customHandler.UsesStat || __instance.Components.OfType<ResourceSourceInfoComponent>().Any())
                        {
                            int increase = 0;
                            foreach (ResourceSourceInfoComponent t in __instance.GetComponents<ResourceSourceInfoComponent>(x => x.Active(unit)))
                            {

                                int stat = (unit.Stats.GetStat(t.AltStat) as ModifiableValueAttributeStat).ModifiedValue;
                                int statVal = (unit.Stats.GetStat(t.AltStat) as ModifiableValueAttributeStat).Bonus;

                                increase = Math.Max((statVal), increase);
#if DEBUG
                                Main.Log.Log($"Prefix ver ApplyAltStatsToMax executing for {__instance.name} on {unit.CharacterName}, alt stat candidate {t.AltStat} {stat} from {t.m_Unlock.NameSafe()} offering {statVal}");
#endif
                            }
                            runningTotal += increase;
                        }
#if DEBUG
                        Main.Log.Log($"Moving to level logic on {__instance.name} with stat logic");
#endif
                        int bestBaseValue = 0;
                        int bestMinClassValue = 0;
                        double classRunningTotal = 0;
                        foreach (ClassData charClass in unit.Progression.Classes)
                        {
#if DEBUG
                            Main.Log.Log($"Assessing {charClass.CharacterClass.Name} on {unit.CharacterName}");

#endif
                            int found = 0;
                            double best = 0;
                            if (customHandler.classEntries.TryGetValue(charClass.CharacterClass.ToReference<BlueprintCharacterClassReference>(), out ClassEntry classEntry))
                            {

#if DEBUG
                                Main.Log.Log($"ClassEntry found");
#endif
                                List<ClassGainSubEntry> entries = new List<ClassGainSubEntry>();
                                foreach (ArchetypeEntry v in classEntry.archetypeEntries)
                                {
                                    if (v.Applies(charClass))
                                    {
                                        entries.Add(v);
                                    }
                                }
                                if (classEntry.vanilla != null && classEntry.vanilla.Applies(charClass))
                                {
                                    entries.Add(classEntry.vanilla);
                                }



                                found = entries.Count;
                                if (found == 0)
                                {

                                    best = charClass.Level * otherClassMultiplier;
#if DEBUG
                                    Main.Log.Log($"Applicable entries for {charClass.CharacterClass.Name} not found on  on {__instance.name}");
#endif
                                }
                                else
                                {


                                    foreach (ClassGainSubEntry entry in entries)
                                    {
                                        bestBaseValue = Math.Max(bestBaseValue, entry.BaseValue);
                                        if (entry.PerLevel)
                                        {
                                            best = Math.Max(best, entry.IncreasePerTick * charClass.Level);
                                        }
                                        else
                                        {
#if DEBUG
                                            Main.Log.Log($"LevelStep is {entry.LevelStep}");
#endif

                                            bestMinClassValue = Math.Max(entry.MinClassLevelIncrease, bestMinClassValue);
                                            best = Math.Max(best, (double)entry.IncreasePerTick * ((double)charClass.Level - (double)entry.StartLevel) / (double)entry.LevelStep);
                                            if (charClass.Level >= entry.StartLevel)
                                            {
                                                best += entry.StartIncrease;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
#if DEBUG
                                Main.Log.Log($"Entry for {charClass.CharacterClass.Name} not found on  on {__instance.name}");
#endif
                                best += charClass.Level * otherClassMultiplier;
                            }
                            classRunningTotal += best;

#if DEBUG
                            Main.Log.Log($"Custom class calc logic executing for {__instance.name} on {unit.CharacterName}, class {charClass.CharacterClass.Name} provided {best}: total is {runningTotal}");
#endif
                        }
                        runningTotal += (double)bestBaseValue;
                        runningTotal += Math.Max(classRunningTotal, (double)bestMinClassValue);
                        int bonus = 0;


                        EventBus.RaiseEvent<IResourceAmountBonusHandler>(unit.Unit, delegate (IResourceAmountBonusHandler h)
                        {
                            h.CalculateMaxResourceAmount(__instance, ref bonus);
                        });
                        __result = Math.Max(__instance.m_Min, __instance.ApplyMinMax((int)runningTotal) + bonus);
#if DEBUG
                        Main.Log.Log($"Custom class calc logic executing for {__instance.name} on {unit.CharacterName}, total is {__result}");
#endif
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    
                        Main.Log.LogException(e);

                        return true;
                    
                }
            }
        }
    }
    
}