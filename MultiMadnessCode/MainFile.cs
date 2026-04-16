using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Events;
using MultiMadness.MultiMadnessCode.Relics;


namespace MultiMadness.MultiMadnessCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "MultiMadness"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(Vakuu), "Pool2", MethodType.Getter)]
class VakuuLootPatch
{
    static IEnumerable<EventOption> Postfix(IEnumerable<EventOption> __result, Vakuu __instance)
    {


        var myMethod = typeof(Vakuu)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(m => m is { Name: "RelicOption", IsGenericMethodDefinition: true } && m.GetParameters()[0].ParameterType == typeof(string));

        MethodInfo relicOption = myMethod.MakeGenericMethod(typeof(TheDeal));
        GD.Print("Got to make generic step");
        List<EventOption> eventList = new List<EventOption>();
        if (__instance.Owner != null)
        {
            if (__instance.Owner.RunState.Players.Count > 1)
            {
                eventList.Add((EventOption)relicOption.Invoke(__instance,new  Object?[] { "INITIAL", null}));
                GD.Print("Invoked");
            }
            GD.Print("OwnerChecked");
        }

        IEnumerable<EventOption> CombinedEnum = (IEnumerable<EventOption>) eventList.Concat(__result);
        GD.Print("Done");
        return CombinedEnum;
    }
}

[HarmonyPatch(typeof(RestSiteOption), "IconPath", MethodType.Getter)]
class RestSiteIconPatch
{
    static string Postfix(string __result, RestSiteOption __instance)
    {
        if (__instance.OptionId == "MULTIPLAYERMADNESS-CHIP")
        {
            if (ResourceLoader.Exists($"MultiMadness/images/ui/rest_site/chipin.png"))
            {
                GD.Print("Resource Found");
                return "MultiMadness/images/ui/rest_site/chipin.png";
            }
            return ImageHelper.GetImagePath($"ui/rest_site/option_cook.png");
            
        }
        return __result;
    }
}
