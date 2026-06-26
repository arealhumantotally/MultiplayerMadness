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




public class TaxationPowerCard : MultiMadnessPower
{
    public const int ratio = 2;
    public override PowerType Type => PowerType.Buff;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public int cardsDrawn = 0;
    
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => cardsDrawn % 2;
    //Causes sate divergences for reasons unknown.
    public async override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner == this.Owner.Player)
        {
            return;
        }
        cardsDrawn += 1;
        int cardModulus = cardsDrawn % ratio;
        int i = cardsDrawn - cardModulus;
        GD.Print(cardsDrawn);
        await CardPileCmd.Draw(choiceContext,i / ratio, this.Owner.Player);
        cardsDrawn = cardModulus;
        GD.Print(cardsDrawn);
        this.InvokeDisplayAmountChanged();
        
    }
    

    
    
}