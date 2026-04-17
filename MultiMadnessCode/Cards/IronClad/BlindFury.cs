using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using MultiMadness.MultiMadnessCode.Cards;

namespace MultiMadness.MultiMadnessCode.Cards.IronClad;

[Pool(typeof(IroncladCardPool))]
public class BlindFury() : MultiMadnessCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("AllyDamage", 4), new DamageVar(13, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        foreach (Creature i in this.CombatState.Allies)
        {
            if (i.Player == this.Owner)
            {
                continue;
            }
            await CreatureCmd.Damage(choiceContext, i, (Decimal) this.DynamicVars["AllyDamage"].IntValue, ValueProp.Unpowered, this.Owner.Creature);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(5);
    }
}