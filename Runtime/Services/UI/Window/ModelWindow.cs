namespace Services.UI.Window
{
    public abstract class ModelWindow<T> : WindowBase
    {
        public abstract void Setup(T transaction);
    }
}