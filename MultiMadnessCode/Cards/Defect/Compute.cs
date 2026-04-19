using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MultiMadness.MultiMadnessCode.Cards;

namespace MultiMadness.MultiMadnessCode.Cards.Defect;

[Pool(typeof(TokenCardPool))]
public class Compute() : MultiMadnessCard(0,
    CardType.Skill, CardRarity.Token,
    TargetType.AnyAlly)
{
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int paid = this.ResolveEnergyXValue();
        if (this.IsUpgraded)
        {
            ++paid;
        }
        
        PlayerCmd.GainEnergy(paid, play.Target.Player);
        
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain];

    protected override void OnUpgrade()
    {
        
    }
}