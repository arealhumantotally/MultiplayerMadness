using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MultiMadness.MultiMadnessCode.Cards;

namespace MultiMadness.MultiMadnessCode.Cards.Defect;

[Pool(typeof(DefectCardPool))]
public class DistributedComputing() : MultiMadnessCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Compute>()];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (Player i in this.CombatState.Players)
        {
            if (!i.Creature.IsAlive) continue;
            if (i == this.Owner) continue;
            await CardPileCmd.AddGeneratedCardToCombat((CardModel) this.CombatState.CreateCard<Compute>(i), PileType.Hand, true);
        }
        
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}