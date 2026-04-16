using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MultiMadness.MultiMadnessCode.Extensions;

namespace MultiMadness.MultiMadnessCode.Enchantments;

public class Prismatic : CustomEnchantmentModel
{
    protected override string? CustomIconPath => "res://MultiMadness/images/enchantments/MultiMadness-prismatic.png";


    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay play)
    {
        GD.Print(IconPath);
        IReadOnlyList<Player> players = play.Card.Owner.RunState.Players;
        if (this.Status != EnchantmentStatus.Normal)
        {
            return;
        }
        List<Player> otherPlayers = players.Where(p => p != play.Card.Owner).ToList();
        otherPlayers.UnstableShuffle(play.Card.Owner.RunState.Rng.Niche);
        Player victim = otherPlayers[0];
        this.Status = EnchantmentStatus.Disabled;
        CardModel cardClone =  play.Card.CreateClone();
        cardClone.Owner = null;
        cardClone.Owner = victim;
        await CardPileCmd.AddGeneratedCardToCombat(cardClone,PileType.Hand,true);
    }
}