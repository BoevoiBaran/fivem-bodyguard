using System.Collections.Generic;
using CitizenFX.Core;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class StateContext
    {
        public readonly Ped BodyguardPed;
        public readonly Ped OwnerPed;
        public readonly Stack<IState> BotStates;
        
        public int BodyguardCurrentIndex;
        public int BodyguardsCount;

        public StateContext(Stack<IState> botStates, Ped bodyguardPed, Ped ownerPed)
        {
            BotStates = botStates;
            BodyguardPed = bodyguardPed;
            OwnerPed = ownerPed;
        }
    }
}