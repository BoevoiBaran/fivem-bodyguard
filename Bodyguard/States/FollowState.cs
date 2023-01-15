using CitizenFX.Core;
using CitizenFX.Core.Native;
using Shared;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class FollowState : IState
    {
        private readonly StateContext _context;
        
        public FollowState(StateContext context)
        {
            Debug.WriteLine("[FollowState] Start");
            
            _context = context;
        }

        public void Update()
        {
            var player = _context.OwnerPed;
            var playerPos = player.Position;
            var guardPos = _context.BodyguardPed.Position;
            var distanceToPlayer = guardPos.DistanceTo(playerPos);
            var speed = _context.OwnerPed.IsSprinting || _context.OwnerPed.IsRunning ? 10 : 1; 

            if (distanceToPlayer > 3)
            {
                API.TaskGoToEntity(_context.BodyguardPed.Handle, player.Handle, -1, 2.0f, speed, 1073741824, 0);    
            }

            if (_context.OwnerPed.IsInCombat)
            {
                var states = _context.BotStates;
                states.Pop();
                states.Push(new DefenceState(_context));
            }
        }
    }
}