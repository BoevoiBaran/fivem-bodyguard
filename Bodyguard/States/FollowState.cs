using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Ext;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class FollowState : IState
    {
        private readonly StateContext _context;

        public FollowState(StateContext context)
        {
            _context = context;
        }
        
        private Vector3 _previousPosition;
        
        public void Update()
        {
            var player = _context.OwnerPed;
            var playerPos = player.Position;
            if (_previousPosition.SqrtDistanceTo(playerPos) < 4)
            {
                return;
            }
            
            var guardPos = _context.BodyguardPed.Position;
            var positionInOrder = GetGuardPositionInOrder(player, _context.BodyguardCurrentIndex);
            var sqrtDistanceToPosition = guardPos.SqrtDistanceTo(positionInOrder);
            var speed = GetSpeed(sqrtDistanceToPosition);
            
            if (sqrtDistanceToPosition > 4)
            {
                API.TaskGoToCoordAnyMeans(
                    _context.BodyguardPed.Handle,
                    positionInOrder.X,
                    positionInOrder.Y,
                    positionInOrder.Z,
                    speed,
                    0, 
                    false,
                    786603, 
                    0xbf800000);
            }
            
            _previousPosition = playerPos;

            if (_context.OwnerPed.IsInCombat)
            {
                var states = _context.BotStates;
                states.Pop();
                states.Push(new DefenceState(_context));
            }
        }

        private int GetSpeed(float sqrtDistanceToPosition)
        {
            return _context.OwnerPed.IsSprinting || _context.OwnerPed.IsRunning || sqrtDistanceToPosition > 9 ? 10 : 1;
        }

        private Vector3 GetGuardPositionInOrder(Ped player, int guardIndex)
        {
            var guardPosition = player.Position;
            var playerPosition = player.Position;
            var forward = player.ForwardVector;
            var right = player.RightVector;
            switch (guardIndex)
            {
                case 0:
                    guardPosition = playerPosition + (forward * 3f);
                    break;
                case 1:
                    guardPosition = playerPosition - (forward * 3f);
                    break;
                case 2:
                    guardPosition = playerPosition + (right * 3f);
                    break;
                case 3:
                    guardPosition = playerPosition - (right * 3f);
                    break;
            }
            return guardPosition;
        }
    }
}