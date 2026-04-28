using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MultiMadness.MultiMadnessCode.Cards;

namespace MultiMadness.MultiMadnessCode.Cards.Silent;
[Pool(typeof(SilentCardPool))]
public class Pass() : MultiMadnessCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyAlly)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
        CardModel selectedcard = (await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1,1), (Func<CardModel, bool>) null, (AbstractModel) this)).FirstOrDefault();
        if (selectedcard == null) return;
        
        CardModel cardClone =  selectedcard.CreateClone();
        cardClone.Owner = null;
        cardClone.Owner = play.Target.Player;
        await CardPileCmd.AddGeneratedCardToCombat(cardClone,PileType.Hand,true);
        await CardCmd.Exhaust(choiceContext, selectedcard);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}