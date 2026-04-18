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



public class LucidityPower : MultiMadnessPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (this.CombatState.CurrentSide != this.Owner.Side)
        {
            return;
        }

        if (result.WasFullyBlocked)
        {
            return;
        }

        if (target != this.Owner)
        {
            return;
        }
        foreach (Creature i in this.CombatState.Allies)
        {
            if (i == this.Owner)
            {
                continue;
            }
            await CreatureCmd.GainBlock(i, new BlockVar(this.Amount * result.UnblockedDamage,ValueProp.Move),null, false);
        }
    }
}