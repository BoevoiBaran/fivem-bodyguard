using System.Collections.Generic;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.States;


// ReSharper disable once CheckNamespace
namespace Client
{
    public class BodyGuardsTeam
    {
        private const int DriverIndexInTeam = 0;
        private static readonly List<VehicleSeat> VehicleSeatsOrder = new List<VehicleSeat>
        {
            VehicleSeat.Driver,
            VehicleSeat.Passenger,
            VehicleSeat.LeftRear,
            VehicleSeat.RightRear
        };
        
        private readonly Vehicle _vehicle;
        private readonly bool _hasVehicle;
        private readonly List<Bodyguard> _bodyguards;
        private int _teamVehicleBlip;
        private Ped Driver => _bodyguards[DriverIndexInTeam].GuardPed;
        
        public BodyGuardsTeam(List<Bodyguard> members)
        {
            _bodyguards = members;
            _hasVehicle = false;
        }
        
        public BodyGuardsTeam(List<Bodyguard> members, Vehicle vehicle)
        {
            _bodyguards = members;
            _vehicle = vehicle;
            _hasVehicle = true;
        }

        public void Update()
        {
            foreach (var guard in _bodyguards)
            {
                guard.Update();
            }
        }

        public void Setup(Vector3 ownerPosition)
        {
            if (_hasVehicle)
            {
                SetupVehicleTeam(ownerPosition);
            }
            else
            {
                SetupInfantryTeam();   
            }
        }

        private void SetupVehicleTeam(Vector3 ownerPosition)
        {
            _teamVehicleBlip = API.AddBlipForEntity(_vehicle.Handle);
            API.SetBlipColour(_teamVehicleBlip, 40);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString("Bodyguards");
            API.EndTextCommandSetBlipName(_teamVehicleBlip);

            for (var i = 0; i < _bodyguards.Count; i++)
            {
                var guardPed = _bodyguards[i].GuardPed;
                guardPed.SetIntoVehicle(_vehicle, VehicleSeatsOrder[i]);
            }

            var targetLocation = new Vector3();
            var targetHeading = 0.0f;
            API.GetClosestVehicleNodeWithHeading(ownerPosition.X, ownerPosition.Y, ownerPosition.Z, ref targetLocation, ref targetHeading, 1, 3.0F, 0);
            Driver.Task.DriveTo(_vehicle, targetLocation, 10F, 20F, 262972);

            foreach (var guardPed in _bodyguards)
            {
                var initState = new InVehicleState(
                    guardPed.Context, 
                    _vehicle, 
                    targetLocation, 
                    targetHeading,
                    _vehicle.Driver.Handle == guardPed.GuardPed.Handle);
                
                guardPed.PushInitState(initState);
            }
        }

        private void SetupInfantryTeam()
        {
            foreach (var guardPed in _bodyguards)
            {
                var initState = new FollowState(guardPed.Context);
                guardPed.PushInitState(initState);
            }
        }
    }
}