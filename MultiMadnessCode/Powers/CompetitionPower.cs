using BaseLib.Utils;
using Godot;
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




public class CompetitionPower : MultiMadnessPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    private Dictionary<Creature, int> Damages = new Dictionary<Creature, int>();

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer == null)
        {
            return;
        }
        if (target.Side == this.Owner.Side)
        {
            return;
        }
        if (dealer.Side != this.Owner.Side)
        {
            return;
        }

        if (!Damages.ContainsKey(dealer))
        {
            Damages[dealer] = 0;
        }

        Damages[dealer] += result.TotalDamage;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != this.Owner.Side)
        {
            return;
        }
        bool blockWin = false;
        foreach (Creature i in participants)
        {
            if (i.Side != this.Owner.Side) continue;
            if (i == this.Owner) continue;
            if (i.Block >= this.Owner.Block)
            {
                GD.Print(i.Block + " Blockmax increased!");
                blockWin = true;
            }
        }
        int myDamage = Damages.ContainsKey(this.Owner) ? Damages[this.Owner] : 0;
        bool damageWin = true;
        foreach ((Creature i, int j) in Damages)
        {
            if (myDamage >= j)
            {
                damageWin = false;
            }
        }

        if (damageWin)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, this.Amount, this.Owner, null);
        }
        if (blockWin)
        {
            await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner, this.Amount, this.Owner, null);
        }

        blockWin = false;
        Damages =  new Dictionary<Creature, int>();
    }
}