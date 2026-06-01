using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MultiMadness.MultiMadnessCode.Cards;
using MultiMadness.MultiMadnessCode.Powers;

namespace MultiMadness.MultiMadnessCode.Cards.Silent;

[Pool(typeof(SilentCardPool))]
public class Competition() : MultiMadnessCard(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<CompetitionPower>(choiceContext, this.Owner.Creature, 1, this.Owner.Creature, null);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}