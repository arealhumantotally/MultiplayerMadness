using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MultiMadness.MultiMadnessCode.Cards;
using MultiMadness.MultiMadnessCode.Powers;

namespace MultiMadness.MultiMadnessCode.Cards.IronClad;

[Pool(typeof(IroncladCardPool))]
public class Lucidity() : MultiMadnessCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Multiplier", 1)];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<LucidityPower>(this.Owner.Creature,this.DynamicVars["Multiplier"].IntValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["Multiplier"].UpgradeValueBy(1);
    }
}