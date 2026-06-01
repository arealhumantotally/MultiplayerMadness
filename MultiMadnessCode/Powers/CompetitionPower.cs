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
    public Dictionary<Creature, int> Damages = new Dictionary<Creature, int>();
    public bool BlockWin = true;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
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
        foreach (Creature i in participants)
        {
            if (i.Side != this.Owner.Side) continue;
            if (i == this.Owner)
            if (i.Block >= this.Owner.Block)
            {
                GD.Print(i.Block + " Blockmax increased!");
                BlockWin = false;
            }
        }
        int myDamage = Damages.ContainsKey(this.Owner) ? Damages[this.Owner] : 0;
        bool DamageWin = true;
        foreach ((Creature i, int j) in Damages)
        {
            if (myDamage >= j)
            {
                DamageWin = false;
            }
        }

        if (DamageWin)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, this.Amount, this.Owner, null);
        }
        if (BlockWin)
        {
            await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner, this.Amount, this.Owner, null);
        }

        BlockWin = false;
        Damages =  new Dictionary<Creature, int>();
    }
}