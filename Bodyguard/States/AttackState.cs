using CitizenFX.Core;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class AttackState : IState
    {
        private readonly StateContext _context;
        
        public AttackState(StateContext context)
        {
            Debug.WriteLine("[AttackState] Start");
            
            _context = context;
        }
        
        public void Update()
        {
            var states = _context.BotStates;
            states.Pop();
            states.Push(new FollowState(_context));
        }
    }
}