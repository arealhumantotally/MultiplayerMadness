using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MultiMadness.MultiMadnessCode.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class YouveGotMail() : MultiMadnessCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new EnergyVar("Energy",1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        //This massive statement just generates an enumerable of two common cards from the defect pool.
        IEnumerable<CardModel> cards = CardFactory.GetDistinctForCombat(play.Target.Player,
            ModelDb.CardPool<DefectCardPool>()
                .GetUnlockedCards(play.Target.Player.UnlockState,
                    play.Target.Player.RunState.CardMultiplayerConstraint)
                .Where(c => c.Rarity == CardRarity.Common),
            this.DynamicVars.Cards.IntValue,
            this.Owner.RunState.Rng.CombatCardGeneration).Take(2);

        foreach (CardModel card in cards)
        {
            GD.Print("Added a card");
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, true);
        }
        await PlayerCmd.GainEnergy(this.DynamicVars.Energy.IntValue, play.Target.Player);

    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override void OnUpgrade()
    {
        this.DynamicVars["Cards"].UpgradeValueBy(1);
    }
}