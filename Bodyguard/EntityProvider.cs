using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Ext;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class EntityProvider
    {
        public List<Ped> TargetEntities => _targetEntities;

        private readonly List<Ped> _targetEntities = new List<Ped>();
        private readonly List<Ped> _allNearestEntities = new List<Ped>();
        
        private EntityProvider() { }

        private static EntityProvider _instance;
        private static readonly object Lock = new object();
        
        public static EntityProvider GetInstance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new EntityProvider();
                    }
                }
            }
            return _instance;
        }

        public void Update()
        {
            _allNearestEntities.Clear();
            _targetEntities.Clear();
            
            var entities = World.GetAllPeds();
            var player = Game.Player.Character;
            const float sqrtRange = 250f;

            foreach (var entity in entities)
            {
                if (entity.IsAlive && entity.Position.SqrtDistanceTo(player.Position) <= sqrtRange)
                {
                    _allNearestEntities.Add(entity);
                    
                    if (entity.IsInCombatAgainst(player))
                    {
                        _targetEntities.Add(entity);
                    }
                }
            }
        }
    }
}