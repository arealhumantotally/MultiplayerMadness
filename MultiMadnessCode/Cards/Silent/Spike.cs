using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MultiMadness.MultiMadnessCode.Cards;

namespace MultiMadness.MultiMadnessCode.Cards.Silent;

[Pool(typeof(SilentCardPool))]
public class Spike() : MultiMadnessCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Creature target = play.Target;
        await PowerCmd.Apply<PoisonPower>(choiceContext, target, 3, this.Owner.Creature, null);
        int poison = target.GetPower<PoisonPower>().Amount;
        await PowerCmd.Apply<StrengthPower>(choiceContext, target, poison, this.Owner.Creature, null);
        if (this.IsUpgraded)
        {
            await CardPileCmd.Draw(choiceContext, 1, this.Owner );
        }
    }

    protected override void OnUpgrade()
    {

    }
}