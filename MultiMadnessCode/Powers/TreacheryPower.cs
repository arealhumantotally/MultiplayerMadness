using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MultiMadness.MultiMadnessCode.Powers;


public class TreacheryPower : MultiMadnessPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("AllyDamage", 0)];

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        TreacheryPower treacheryPower = this;
        if (side != this.Owner.Side)
        {
            return;
        }
        foreach (Creature i in this.CombatState.Allies)
        {
            if (i == this.Owner)
            {
                continue;
            }
            await CreatureCmd.Damage(choiceContext, i, (Decimal) this.DynamicVars["AllyDamage"].IntValue, ValueProp.Unpowered, this.Owner);
        }

        await PowerCmd.Apply<StrengthPower>(this.Owner, (Decimal)this.Amount, this.Owner, (CardModel)null);

    }

    public void IncrementAllyDamage()
    {
        this.AssertMutable();
        this.DynamicVars["AllyDamage"].BaseValue += 6;
    }
}