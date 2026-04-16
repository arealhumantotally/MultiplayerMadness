using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MultiMadness.MultiMadnessCode.Relics;

namespace MultiMadness.MultiMadnessCode.RestSiteOptions;

public class Chip : RestSiteOption
{

    protected HeftyGeode Relic;
    
    public Chip(Player owner, HeftyGeode originalRelic) : base(owner)
    {
        Relic = originalRelic;
    }

    public override async Task<bool> OnSelect()
    {
        if (await Relic.ChipGeode())
        {
            return true;
        }
        this.IsEnabled = false;
        return false;
    }
    
    public override LocString Description
    {
        get
        {
            if (!this.IsEnabled)
                return new LocString("rest_site_ui", $"OPTION_{this.OptionId}.descriptionDisabled");
            LocString description = new LocString("rest_site_ui", $"OPTION_{this.OptionId}.description");
            return description;
        }
    }
    public override string OptionId => "MULTIPLAYERMADNESS-CHIP";
    
}