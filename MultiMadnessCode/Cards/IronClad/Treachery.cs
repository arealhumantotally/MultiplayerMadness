using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MultiMadness.MultiMadnessCode.Powers;

namespace MultiMadness.MultiMadnessCode.Cards.IronClad;

[Pool(typeof(IroncladCardPool))]
public class Treachery() : MultiMadnessCard(2,
    CardType.Power, CardRarity.Rare,
    MegaCrit.Sts2.Core.Entities.Cards.TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("AllyDamage", 0)];
    public override CardMultiplayerConstraint MultiplayerConstraint => (CardMultiplayerConstraint)1;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        (await PowerCmd.Apply<TreacheryPower>(this.Owner.Creature, 3, this.Owner.Creature, this))?.IncrementAllyDamage();
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}