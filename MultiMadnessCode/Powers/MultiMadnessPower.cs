using BaseLib.Abstracts;
using BaseLib.Extensions;
using MultiMadness.MultiMadnessCode.Extensions;
using Godot;

namespace MultiMadness.MultiMadnessCode.Powers;

public abstract class MultiMadnessPower : CustomPowerModel
{
    //Loads from MultiMadness/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            GD.Print($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath());
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}