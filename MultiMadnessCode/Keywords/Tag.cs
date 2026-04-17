using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace MultiMadness.MultiMadnessCode.Keywords;

public class TagVar(string name, decimal baseValue) : DynamicVar(name, baseValue)
{
    
}
[HarmonyPatch(typeof(CardModel), "CanPlay")]
class RestKeywordPatch