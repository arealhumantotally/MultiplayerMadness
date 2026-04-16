using BaseLib.Abstracts;
using BaseLib.Extensions;
using MultiMadness.MultiMadnessCode.Extensions;
using Godot;

namespace MultiMadness.MultiMadnessCode.Relics;

public abstract class MultiMadnessRelic : CustomRelicModel
{
    //MultiMadness/images/relics
    public override string PackedIconPath
    {
        get
        {
            
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".RelicImagePath();
        }
    }

    protected override string PackedIconOutlinePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic_outline.png".RelicImagePath();
        }
    }

    protected override string BigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".BigRelicImagePath();
        }
    }
}