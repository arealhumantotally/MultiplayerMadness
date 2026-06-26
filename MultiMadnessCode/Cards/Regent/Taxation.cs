using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MultiMadness.MultiMadnessCode.Cards;
using MultiMadness.MultiMadnessCode.Powers;

namespace MultiMadness.MultiMadnessCode.Cards.Necrobinder;


[Pool(typeof(RegentCardPool))]
public class Taxation() : MultiMadnessCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (this.IsUpgraded)
        {
            await PowerCmd.Apply<TaxationPowerCard>(choiceContext, this.Owner.Creature,1,this.Owner.Creature,this);
        }
        await PowerCmd.Apply<TaxationPower>(choiceContext, this.Owner.Creature,1,this.Owner.Creature,this);
        
    }

    protected override void OnUpgrade()
    {

    }
}