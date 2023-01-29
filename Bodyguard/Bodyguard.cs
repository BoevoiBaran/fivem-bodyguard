using System.Collections.Generic;
using CitizenFX.Core;
using Client.States;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class Bodyguard
    {
        public Ped GuardPed => _context.BodyguardPed;
        
        private readonly StateContext _context;
        private readonly Stack<IState> _botStates = new Stack<IState>();
        
        public Bodyguard(StateContext context)
        {
            _context = context;
            _context.SetupStates(_botStates);
        }

        public void PushInitState(IState state)
        {
            _botStates.Push(state);
        }
        
        public void Update()
        {
            if (_botStates.Count != 0)
            {
                _botStates.Peek()?.Update();    
            }
        }
    }
}