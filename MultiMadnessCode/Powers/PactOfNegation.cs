using System.Reflection;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MultiMadness.MultiMadnessCode.Cards.Necrobinder;

namespace MultiMadness.MultiMadnessCode.Powers;





public class PactOfNegationPower : MultiMadnessPower
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Upgrade", 0)];
    public override PowerType Type => PowerType.Buff;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override PowerStackType StackType => PowerStackType.Counter;

    

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (this.DynamicVars["Upgrade"].IntValue == 0)
        {
            if (card.Type == CardType.Attack)
            {
                return playCount;
            }
        }
        //This causes a loop and is generally fucky, so we wont allow it
        if (card is PactOfNegation)
        {
            return playCount;
        }

        if (playCount == 0)
        {
            return playCount;
        }

        if (this.Owner == card.Owner.Creature)
        {
            return playCount;
        }
        CardModel duped = card.CreateDupe();
        duped.Owner = null;
        duped.Owner = this.Owner.Player;
        duped.BaseReplayCount += 1;
        Creature? neotarget = target;
        
        if (target == this.Owner)
        {
            neotarget = card.Owner.Creature;
        }
        
        HookPlayerChoiceContext choice = new HookPlayerChoiceContext(this.Owner.Player, this.Owner.Player.NetId, GameActionType.Combat);
        CardCmd.AutoPlay(choice,duped,neotarget);

        //Using reflection is very hacky here but i genuinely have no idea how else i can fix this without patching 
        //onplaywrapper() which would be worse i think.
        if (card.Type == CardType.Power)
        {
            MethodInfo vfxMethod = AccessTools.Method(typeof(CardModel), "PlayPowerCardFlyVfx");
            //Specifically, the animation for powers is only played in the replay step
            //Which means if we skip it the card just gets stuck in the play pile indefinitely.
            vfxMethod.Invoke(card, null);
        }
        PowerCmd.Decrement(this);
        return 0;

        
    }
}