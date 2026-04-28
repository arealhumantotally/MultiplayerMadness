using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MultiMadness.MultiMadnessCode.Relics;

namespace MultiMadness.MultiMadnessCode.RestSiteOptions;

public class Chip : RestSiteOption
{
    
    public Chip(Player owner) : base(owner)
    {
    }

    public override async Task<bool> OnSelect()
    {
        bool chipped = false;
        foreach (Player i in this.Owner.RunState.Players)
        {
            foreach (RelicModel j in i.Relics)
            {
                if (j is not HeftyGeode geode) continue;
                chipped |= await geode.ChipGeode();
            }
        }
        if (chipped)
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