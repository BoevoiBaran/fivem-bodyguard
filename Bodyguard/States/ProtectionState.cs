﻿using CitizenFX.Core;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class ProtectionState :IState
    {
        private readonly StateContext _context;
        
        public ProtectionState(StateContext context)
        {
            Debug.WriteLine("[ProtectionState] Start");
            
            _context = context;
        }

        public void Update()
        {
            var states = _context.BotStates;
            states.Pop();
            states.Push(new FollowState(_context));
        }
    }
}