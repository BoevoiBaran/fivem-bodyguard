using CitizenFX.Core;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class InVehicleState : IState
    {
        private readonly StateContext _context;

        public InVehicleState(StateContext context)
        {
            Debug.WriteLine("[InVehicleState] Start");
            
            _context = context;
        }
        
        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}