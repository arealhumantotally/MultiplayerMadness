using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MultiMadness.MultiMadnessCode.Relics;

namespace MultiMadness.MultiMadnessCode.Relics;

[Pool(typeof(EventRelicPool))]
public class TheDeal() : MultiMadnessRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;
    
    public override async Task AfterObtained()
    {
        IReadOnlyList<Player> players = this.Owner.RunState.Players;

        List<Player> otherPlayers = players.Where(p => p != this.Owner).ToList();
        
        List<CardModel> strikesanddefends = this.Owner.Deck.Cards.Where(p =>
            (
                (
                    p.Tags.Contains<CardTag>(CardTag.Strike)
                    || p.Tags.Contains<CardTag>(CardTag.Defend)
                ) 
                && p.Rarity == CardRarity.Basic
            )
             || p.Type == CardType.Curse   
                ).ToList();
        foreach (CardModel card in strikesanddefends)
        {
            otherPlayers.UnstableShuffle(this.Owner.RunState.Rng.Niche);
            CardModel clonedcard = Owner.RunState.CloneCard(card);
            clonedcard.Owner = null;
            clonedcard.Owner = otherPlayers[0];
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(clonedcard, otherPlayers[0].Deck));
            await CardPileCmd.RemoveFromDeck(card);
            GD.Print("Looped");
        }
        GD.Print("Past Loop");
    }
}