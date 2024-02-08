using Zenject;

namespace Services.DependencyInjectionService.Implementation.Zenject
{
    public sealed class DependencyInjectionContainerUpdater : MonoInstaller, IInitializable
    {
        public void Initialize()
        {
            Container
                .Resolve<IDependencyInjectionService>()
                .Update(Container);
        }

        public override void InstallBindings()
        {
            Container
                .Bind<IInitializable>()
                .FromInstance(this)
                .AsCached();
        }
    }
}