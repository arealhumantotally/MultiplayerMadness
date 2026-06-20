using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MultiMadness.MultiMadnessCode.Cards;

namespace MultiMadness.MultiMadnessCode.Cards.Necrobinder;

[Pool(typeof(NecrobinderCardPool))]
public class NeverAgain() : MultiMadnessCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllAllies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("vulnamount",5)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (this.CombatState?.Allies == null)
        {
            return;
        }
        foreach (Creature i in this.CombatState.Allies)
        {
            if (i.Player == this.Owner)
            {
                continue;
            }
            await PowerCmd.Apply<IntangiblePower>(choiceContext, i, 1.0M, this.Owner.Creature, this);
        }
        await PowerCmd.Apply<VulnerablePower>(choiceContext, this.Owner.Creature, this.DynamicVars["vulnamount"].IntValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["vulnamount"].UpgradeValueBy(-2);
    }
}