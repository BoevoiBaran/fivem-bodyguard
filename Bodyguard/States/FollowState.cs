using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Ext;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class FollowState : IState
    {
        private readonly StateContext _context;
        private Vector3 _currentPosition;
        
        public FollowState(StateContext context)
        {
            _context = context;
        }

        public void Update()
        {
            var player = _context.OwnerPed;
            var playerPos = player.Position;
            var guardPos = _context.BodyguardPed.Position;
            var distanceToPlayer = guardPos.DistanceTo(playerPos);
            var speed = _context.OwnerPed.IsSprinting || _context.OwnerPed.IsRunning || distanceToPlayer > 10 ? 10 : 1; 

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

        private Vector3 GetGuardPositionInOrder(int numGuards, Vector3 ownerPosition)
        {
            if (numGuards == 1)
            {
                // Position the guard in front of the player, 3 meters away
                var forward = API.GetEntityForwardVector(_context.OwnerPed.Handle);
                return ownerPosition + (forward * 3);
            }
            else if (numGuards == 2)
            {
                // Position one guard in front of the player, 3 meters away
                // Position the other guard behind the player, 3 meters away
                var forward = API.GetEntityForwardVector(_context.OwnerPed.Handle);
                var guardPosition = ownerPosition + (forward * 3);
                var back = -forward;
                return ownerPosition + (back * 3);
            }
            
            // Position the guards evenly around the player
            var angle = 360 / numGuards;
            var radians = angle * _context.BodyguardCurrentIndex * Math.PI / 180;
            var x = (float)(ownerPosition.X + 3 * Math.Cos(radians));
            var y = (float)(ownerPosition.Y + 3 * Math.Sin(radians));
            return new Vector3(x, y, ownerPosition.Z);
        }
    }
}