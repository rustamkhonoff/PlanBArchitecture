namespace DependencyInjectionService
{
    public interface IBootable :
#if ZENJECT
        Zenject.IInitializable
#endif
    {
#if ZENJECT
        void Zenject.IInitializable.Initialize()
        {
            Boot();
        }
#endif
        void Boot();
    }
}