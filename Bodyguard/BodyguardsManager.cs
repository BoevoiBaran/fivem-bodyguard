using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Helpers;
using Client.States;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class BodyguardsManager
    {
        private bool _started;
        private readonly BodyguardConfig _config;
        private readonly List<BodyGuardsTeam> _bodyguards = new List<BodyGuardsTeam>();

        private static uint _bodyguardGroupHash; 
        
        public BodyguardsManager(BodyguardConfig config)
        {
            _config = config;
            _started = false;

            CreateBodyguardsTeamRelationshipsGroup();
        }

        private void CreateBodyguardsTeamRelationshipsGroup()
        {
            var groupGeneratedName = Guid.NewGuid().ToString();

            API.AddRelationshipGroup(groupGeneratedName, ref _bodyguardGroupHash);

            const int relationshipsType = (int) Relationship.Companion;
            var player = Game.Player.Character;
            var playerGroupHash = (uint) player.RelationshipGroup.Hash;
            
            API.SetRelationshipBetweenGroups(relationshipsType, _bodyguardGroupHash, playerGroupHash);
            API.SetRelationshipBetweenGroups(relationshipsType, playerGroupHash, _bodyguardGroupHash);
        }

        private void AddTeam(BodyGuardsTeam team)
        {
            if (_bodyguards.Count == 0)
            {
                _started = true;
            }
            
            _bodyguards.Add(team);
        }

        public void RemoveTeam()
        {
            //Remove logic
            
            if (_bodyguards.Count == 0)
            {
                _started = false;
            }
        }
        
        
        public async Task SpawnBodyguard()
        {
            var player = Game.Player.Character;
            var bodyGuardHash = PedHash.ChemSec01SMM;
            var spawnPoint = player.Position + player.ForwardVector * 2;
            var bodyGuard = await CreateBodyguard(player, bodyGuardHash, spawnPoint);
            
            if (bodyGuard != null)
            {
                var team = new BodyGuardsTeam(new List<Bodyguard>{bodyGuard});
                team.Setup(player.Position);
                AddTeam(team); 
            }
        }
        
        public async Task SpawnBodyguardTeam()
        {
            var player = Game.Player.Character;

            const int defaultTeamCount = 4;
            
            var guards = new List<Bodyguard>(defaultTeamCount);
            var teamSpawnPosition = player.Position - (player.ForwardVector * 3f);
            var bodyGuardHash = PedHash.ChemSec01SMM;
            for (var i = 0; i < defaultTeamCount; i++)
            {
                var guard = await CreateBodyguard(player, bodyGuardHash, teamSpawnPosition);
                if (guard != null)
                { 
                    guard.Context.BodyguardCurrentIndex = i; 
                    guards.Add(guard); 
                }
            }

            var team = new BodyGuardsTeam(guards);
            team.Setup(player.Position);
            AddTeam(team);
        }

        public async Task SpawnBodyguardTeamOnVehicle()
        {
            var player = Game.Player.Character;
            
            var vehicle = await SpawnVehicle(player, VehicleHash.Baller6);
            if (vehicle == null)
            {
                Debug.WriteLine("[Bodyguard] Spawn team vehicle failed");    
                
                return;   
            }

            const int defaultTeamCount = 4;
            var passengerCapacity = vehicle.PassengerCapacity;
            var teamCount = defaultTeamCount <= passengerCapacity ? defaultTeamCount : passengerCapacity; 
            
            var guards = new List<Bodyguard>(teamCount);
            var teamSpawnPosition = vehicle.Position;
            var bodyGuardHash = PedHash.ChemSec01SMM;
            for (var i = 0; i < teamCount; i++)
            {
                var guard = await CreateBodyguard(player, bodyGuardHash, teamSpawnPosition);
                if (guard != null)
                { 
                    guard.Context.BodyguardCurrentIndex = i; 
                    guards.Add(guard); 
                }
            }

            var team = new BodyGuardsTeam(guards, vehicle);
            team.Setup(player.Position);
            AddTeam(team);
        }

        private static async Task<Vehicle> SpawnVehicle(Ped player, VehicleHash vehicleHash)
        {
            var spawnLocation = new Vector3();
            var spawnHeading = 0F;
            var unusedVar = 0;
            
            API.GetNthClosestVehicleNodeWithHeading(
                player.Position.X, 
                player.Position.Y, 
                player.Position.Z, 
                100, 
                ref spawnLocation, 
                ref spawnHeading, 
                ref unusedVar, 
                9, 
                3.0F, 
                2.5F);
            
            var success = await ModelLoader.LoadModel((uint)vehicleHash);
            if (success)
            {
                var teamVehicle = await World.CreateVehicle(vehicleHash, spawnLocation, spawnHeading);
                teamVehicle.Mods.PrimaryColor = VehicleColor.MetallicBlack;
                teamVehicle.Mods.LicensePlate = "SUC-36484";
                teamVehicle.Mods.LicensePlateStyle = LicensePlateStyle.BlueOnWhite3;
                return teamVehicle;    
            }

            return null;
        }
        
        private static async Task<Bodyguard> CreateBodyguard(Ped owner, PedHash modelHash, Vector3 spawnPoint)
        {
            var bodyGuardHashUint = (uint) modelHash;
            var success = await ModelLoader.LoadModel(bodyGuardHashUint);

            if (success)
            {
                var bodyguardPed = await World.CreatePed(modelHash, spawnPoint);
                var context = new StateContext(bodyguardPed, owner);
                var bodyGuard = new Bodyguard(context, _bodyguardGroupHash);
                return bodyGuard;
            }

            return null;
        }

        public void Update()
        {
            if (_started)
            {
                foreach (var team in _bodyguards)
                {
                    team.Update();
                }
            }
        }
    }
}