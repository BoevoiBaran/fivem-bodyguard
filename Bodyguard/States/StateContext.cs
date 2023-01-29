using System.Collections.Generic;
using CitizenFX.Core;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class StateContext
    {
        public readonly Ped BodyguardPed;
        public readonly Ped OwnerPed;
        public Stack<IState> BotStates;
        
        public int BodyguardCurrentIndex;
        public int BodyguardsCount;

        public StateContext(Ped bodyguardPed, Ped ownerPed)
        {
            BodyguardPed = bodyguardPed;
            OwnerPed = ownerPed;
        }

        public void SetupStates(Stack<IState> botStates)
        {
            BotStates = botStates;
        }
    }
}