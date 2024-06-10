namespace Patterns.FSM
{
    public abstract class StateBase
    {
        public virtual void Exit()
        {
            //ignore
        }

        public virtual void Tick()
        {
            //none
        }
    }
}