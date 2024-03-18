namespace DependencyInjectionService
{
    public interface ITick
#if ZENJECT
        : Zenject.ITickable
#endif
    {
#if ZENJECT
        void Zenject.ITickable.Tick()
        {
            Tick();
        }
#endif
        new void Tick();
    }
}