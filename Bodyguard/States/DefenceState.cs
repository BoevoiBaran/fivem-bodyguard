using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class DefenceState :IState
    {
        private readonly StateContext _context;
        
        public DefenceState(StateContext context)
        {
            _context = context;
        }

        public void Update()
        {
            
            //TaskGuardAssignedDefensiveArea ???
            var player = _context.OwnerPed;
            var nearbyEnemies = new List<Ped>(); //<- GetTargets
            if (nearbyEnemies.Count > 0)
            {
                foreach (var enemy in nearbyEnemies)
                {
                    if (API.IsPedAPlayer(enemy.Handle) && enemy.Handle != player.Handle)
                    {
                        API.TaskCombatPed(_context.BodyguardPed.Handle, enemy.Handle, 0, 16);
                    }
                }
            }

            if (!player.IsInCombat)
            {
                var states = _context.BotStates;
                states.Pop();
                states.Push(new FollowState(_context));
            }
        }
    }
}