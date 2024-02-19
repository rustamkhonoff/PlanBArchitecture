#if ZENJECT
using Services.UI.Extensions.Zenject.Implementation;
using Zenject;

namespace Services.UI.Extensions.Zenject
{
    internal class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<UIService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
#endif