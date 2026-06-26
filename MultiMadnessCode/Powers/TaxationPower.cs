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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MultiMadness.MultiMadnessCode.Powers;



public class TaxationPower : MultiMadnessPower
{
    public const int ratio = 2;
    public override PowerType Type => PowerType.Buff;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public int energyGained = 0;
    
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => energyGained % 2;

    public override decimal ModifyEnergyGain(Player player, decimal amount)
    {
        if (player == this.Owner.Player)
        {
            return amount;
        }
        energyGained += (int)amount;
        int energyModulus = energyGained % ratio;
        int i = energyGained - energyModulus;
        GD.Print(energyGained);
        PlayerCmd.GainEnergy(i / ratio,this.Owner.Player);
        energyGained = energyModulus;
        GD.Print(energyGained);
        this.InvokeDisplayAmountChanged();
        
        return amount;

    }

    
    
}