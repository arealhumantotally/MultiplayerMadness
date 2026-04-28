using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MultiMadness.MultiMadnessCode.Enchantments;
using MultiMadness.MultiMadnessCode.Relics;
using MultiMadness.MultiMadnessCode.RestSiteOptions;

namespace MultiMadness.MultiMadnessCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class HeftyGeode() : MultiMadnessRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    public override int DisplayAmount => this.ChipsRemaining;
    private int _chipsRemaining;
    public override async Task AfterObtained()
    {
        _chipsRemaining = this.Owner.RunState.Players.Count * 3;
        GD.Print(_chipsRemaining);
        this.InvokeDisplayAmountChanged();
    }

    public override bool ShowCounter => _chipsRemaining > 0;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Prismatic>();

    [SavedProperty]
    public int ChipsRemaining
    {
        get => this._chipsRemaining;
        set
        {
            this.AssertMutable();
            this._chipsRemaining = value;
            GD.Print(_chipsRemaining);
            this.InvokeDisplayAmountChanged();
        }
    }

    public async Task<bool> ChipGeode()
    {
        switch (this.ChipsRemaining)
        {
            case 1:
                --ChipsRemaining;
                await BreakGeode();
                return true;
            case 0:
                return false;
            default:
                --ChipsRemaining;
                return true;
        }
    }

    protected async Task BreakGeode()
    {
        HeftyGeode heftyGeode = this;
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, 1)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };
        Prismatic canonicalEnchantment = ModelDb.Enchantment<Prismatic>();
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(heftyGeode.Owner, (EnchantmentModel) canonicalEnchantment, 1, prefs))
        {
            CardCmd.Enchant(canonicalEnchantment.ToMutable(), card, 1);
            CardCmd.Preview(card);
        }
    }

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (_chipsRemaining == 0)
        {
            return true;
        }
        options.Add((RestSiteOption) new Chip(player));
        return true;
    }
}