namespace Patterns.FSM
{
    public abstract class TransferState<T> : StateBase 
    {
        public abstract void Enter(T transfer);
    }
}