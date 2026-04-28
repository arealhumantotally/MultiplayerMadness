using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MultiMadness.MultiMadnessCode.Cards;

namespace MultiMadness.MultiMadnessCode.Cards.Regent;

[Pool(typeof(RegentCardPool))]
public class ProtectMeSquire() : MultiMadnessCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardModel card = (await CardSelectCmd.FromHand(choiceContext,play.Target.Player, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 0, 1), (Func<CardModel, bool>) null, (AbstractModel) this)).FirstOrDefault();
        if (card == null) return;
        await CardCmd.TransformTo<MinionSacrifice>(card);
        await PowerCmd.Apply<CoveredPower>(this.Owner.Creature, 1, play.Target, this);
    }

    protected override void OnUpgrade()
    {

    }
}