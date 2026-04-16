using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MultiMadness.MultiMadnessCode.Relics;

namespace MultiMadness.MultiMadnessCode.Relics;

[Pool(typeof(EventRelicPool))]
public class TheDeal() : MultiMadnessRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;
    public override bool IsAllowed(IRunState runState) => runState.Players.Count > 1;
    
    public override async Task AfterObtained()
    {
        IReadOnlyList<Player> players = this.Owner.RunState.Players;

        List<Player> otherPlayers = players.Where(p => p != this.Owner).ToList();
        otherPlayers.UnstableShuffle(this.Owner.RunState.Rng.Niche);
        Player victim = otherPlayers[0];
        await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), victim.Creature, 10, false);
        await CreatureCmd.SetCurrentHp(victim.Creature, 1);

        await PlayerCmd.GainGold(666, this.Owner);

        await PlayerCmd.GainMaxPotionCount(1, this.Owner);

        CardCreationOptions cardCreationOptions = new CardCreationOptions(
            (IEnumerable<CardPoolModel>)[this.Owner.Character.CardPool], CardCreationSource.Other,
            CardRarityOddsType.BossEncounter);
        List<Reward> rewards = new List<Reward>();
        rewards.Add(new CardReward(cardCreationOptions,3,this.Owner));
        await RewardsCmd.OfferCustom(this.Owner, rewards);
    }
}