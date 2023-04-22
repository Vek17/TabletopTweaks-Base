using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.TextTools;
using System;
using System.Text;
using TabletopTweaks.Core.UMMTools.Utility;
using static TabletopTweaks.Base.Main;

namespace TabletopTweaks.Base.Bugfixes.UI {
    static class DiceRollDescriptions {
        [HarmonyPatch(typeof(LogHelper), "GetRollDescription")]
        private static class LogHelper_GetRollDescription_Patch {
            static bool Prefix(RuleRollDice ruleRollDice, ref string __result) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("DiceReplacementUI")) { return true; }
                if (ruleRollDice == null) {
                    PFLog.Default.Error("Can't find d20 check in context for text template!", Array.Empty<object>());
                    __result = string.Empty;
                    return false;
                }
                if (ruleRollDice.RollHistory != null && ruleRollDice.RollHistory.Count != 1) {
                    StringBuilder stringBuilder = new StringBuilder();
                    bool firstRoll = true;
                    bool taggedResult = false;
                    string startingHTMLTag = string.Empty;
                    string endingHTMLTag = string.Empty;
                    foreach (int roll in ruleRollDice.RollHistory) {

                        if (!firstRoll) {
                            stringBuilder.Append(", ");
                        }
                        firstRoll = false;
                        if (!taggedResult && (roll == ruleRollDice.m_Result || roll == ruleRollDice.Result)) {
                            startingHTMLTag = "<b><u>";
                            endingHTMLTag = "</u></b>";
                            taggedResult = true;
                        }
                        if (ruleRollDice.ReplaceOneWithMax && roll == 1) {
                            stringBuilder
                                .Append("<s>1</s> ".Grey())
                                .Append(startingHTMLTag)
                                .Append("20")
                                .Append(endingHTMLTag);
                        } else if (ruleRollDice.ResultOverride.HasValue
                            && !ruleRollDice.m_PreRolledResult.HasValue
                            && roll == ruleRollDice.m_Result) {
                            var actualroll = ruleRollDice.m_PreRolledResult.GetValueOrDefault() > 0 ?
                                ruleRollDice.m_PreRolledResult.Value : ruleRollDice.m_Result;
                            stringBuilder
                                .Append($"<s>{actualroll}</s> ".Grey())
                                .Append(startingHTMLTag)
                                .Append($"{ruleRollDice.ResultOverride.Value}")
                                .Append(endingHTMLTag);
                        } else {
                            stringBuilder
                                .Append(startingHTMLTag)
                                .Append(roll)
                                .Append(endingHTMLTag);
                        }
                        if (taggedResult) {
                            startingHTMLTag = string.Empty;
                            endingHTMLTag = string.Empty;
                        }
                    }
                    if (ruleRollDice.Rerolls != null) {
                        string text = "";
                        bool firstReRoll = true;
                        foreach (RuleRollDice.RerollData rerollData in ruleRollDice.Rerolls) {
                            BlueprintUnitFact blueprintUnitFact = rerollData.Source.Blueprint as BlueprintUnitFact;
                            if (blueprintUnitFact != null) {
                                if (!firstReRoll) {
                                    text += ", ";
                                }
                                text += blueprintUnitFact.Name;
                                firstReRoll = false;
                            }
                        }
                        if (text != "") {
                            stringBuilder
                                .Append(" [")
                                .Append(text)
                                .Append("]");
                        }
                    }
                    __result = stringBuilder.ToString();
                    return false;
                }
                if (ruleRollDice.ReplacedOne) {
                    __result = $"{"<s>1</s> 20".Grey()}";
                    return false;
                }
                if (ruleRollDice.ResultOverride.HasValue) {
                    var originalResult = ruleRollDice.m_PreRolledResult ?? ruleRollDice.m_Result;
                    if (originalResult == 0) {
                        __result = ruleRollDice.Result.ToString();
                    } else {
                        __result = $"{$"<s>{originalResult}</s>".Grey()} {ruleRollDice.Result}";
                    }
                    return false;
                }
                __result = ruleRollDice.Result.ToString();
                return false;
            }
        }

        [HarmonyPatch(typeof(RuleDispelMagic), nameof(RuleDispelMagic.OnTrigger))]
        private static class RuleDispelMagic_OnTrigger_Patch {
            static void Postfix(RuleDispelMagic __instance) {
                if (TTTContext.Fixes.BaseFixes.IsDisabled("DiceReplacementUI")) { return; }

                if (__instance.CheckRoll.ResultOverride.HasValue) {
                    __instance.CheckRoll.m_Result = __instance.CheckRoll.ResultOverride.Value;
                    __instance.CheckRoll.ResultOverride = null;
                }
            }
        }
    }
}
