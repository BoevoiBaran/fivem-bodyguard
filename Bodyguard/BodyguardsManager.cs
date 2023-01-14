using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class BodyguardsManager
    {
        private bool _started;
        private readonly List<Bodyguard> _bodyguards = new List<Bodyguard>();
        
        public BodyguardsManager()
        {
            _started = false;
        }

        public void AddBodyguard(Bodyguard guard)
        {
            if (_bodyguards.Count == 0)
            {
                _started = true;
            }
            
            _bodyguards.Add(guard);
        }

        public void RemoveBodyguard()
        {
            //Remove logic
            
            if (_bodyguards.Count == 0)
            {
                _started = false;
            }
        }

        public void Update()
        {
            if (_started)
            {
                foreach (var guard in _bodyguards)
                {
                    guard.Update();
                }
            }
        }
    }
}