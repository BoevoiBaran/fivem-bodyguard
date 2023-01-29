using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Ext;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class InVehicleState : IState
    {
        private readonly StateContext _context;
        private readonly Vehicle _vehicle;
        
        private readonly bool _isDriver;

        public InVehicleState(StateContext context, Vehicle vehicle, Vector3 destinationPoint, float targetHeading,
            bool isDriver)
        {
            _context = context;
            _vehicle = vehicle;
            _destinationPoint = destinationPoint;
            _updatedTargetHeading = targetHeading;
            _isDriver = isDriver;
        }
        
        private bool _parkVehicleTaskStarted;
        private bool _leaveVehicleTaskStarted;
        private Vector3 _destinationPoint;
        private float _updatedTargetHeading;
        
        public void Update()
        {
            var guard = _context.BodyguardPed;
            var player = _context.OwnerPed;
            var playerPosition = player.Position;
            var playerDistanceToDestinationPoint = playerPosition.DistanceTo(_destinationPoint);

            if (_isDriver && playerDistanceToDestinationPoint > 30)
            {
                API.GetClosestVehicleNodeWithHeading(playerPosition.X, playerPosition.Y, playerPosition.Z, ref _destinationPoint, ref _updatedTargetHeading, 1, 3.0F, 0);
                _context.BodyguardPed.Task.DriveTo(_vehicle, _destinationPoint, 10F, 20F, 262972);
                return;
            }

            var vehicleDistanceToDestinationPoint = _vehicle.Position.DistanceTo(_destinationPoint);
            
            if (_isDriver && vehicleDistanceToDestinationPoint < 10 && !player.IsInCombat && !_parkVehicleTaskStarted)
            {
                guard.Task.ParkVehicle(_vehicle, _destinationPoint, _updatedTargetHeading);
                _parkVehicleTaskStarted = true;
                return;
            }

            if (vehicleDistanceToDestinationPoint > 10)
            {
                return;
            }

            if (_vehicle.IsStopped && guard.IsInVehicle(_vehicle) && !_leaveVehicleTaskStarted)
            {
                // 64 = normal exit and closes door, maybe a bit slower animation than 0.
                // 256 = normal exit but does not close the door.
                var leaveFlag = _context.OwnerPed.IsInCombat ? 256 : 64;
                API.TaskLeaveVehicle(guard.Handle, _vehicle.Handle, leaveFlag);
                _leaveVehicleTaskStarted = true;
                return;
            }

            if (!guard.IsInVehicle(_vehicle))
            {
                var states = _context.BotStates;
                states.Pop();
                states.Push(new FollowState(_context));
            }
        }
    }
}