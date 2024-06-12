#if ZENJECT
using Zenject;

namespace DependencyInjectionService.Zenject
{
    public class ZenjectBinder : IBinder
    {
        private readonly DiContainer m_diContainer;

        public ZenjectBinder(DiContainer diContainer)
        {
            m_diContainer = diContainer;
        }

        public void BindAsTransient<TType>(params object[] arguments)
        {
            m_diContainer.Bind<TType>().AsTransient().WithArguments(arguments);
        }

        public void BindAsTransientFromResource<TType>(string path, params object[] arguments)
        {
            m_diContainer.Bind<TType>().FromResource(path).AsTransient().WithArguments(arguments);
        }

        public void BindAsTransient<TInterface, TType>(params object[] arguments) where TType : TInterface
        {
            m_diContainer.Bind<TInterface>().To<TType>().AsTransient().WithArguments(arguments);
        }

        public void BindAsTransientFromResource<TInterface, TType>(string path, params object[] arguments) where TType : TInterface
        {
            m_diContainer.Bind<TInterface>().To<TType>().FromResource(path).AsTransient().WithArguments(arguments);
        }

        public void BindAsSingle<TType>(params object[] arguments)
        {
            m_diContainer.Bind<TType>().AsSingle().WithArguments(arguments);
        }

        public void BindAsSingle<TInterface, TType>(params object[] arguments) where TType : TInterface
        {
            m_diContainer.Bind<TInterface>().To<TType>().AsSingle().WithArguments(arguments);
        }

        public void BindAsSingleFromResources<TType>(string path, params object[] arguments)
        {
            m_diContainer.Bind<TType>().FromResource(path).AsSingle().WithArguments(arguments);
        }

        public void BindAsSingleFromResources<TInterface, TType>(string path, params object[] arguments) where TType : TInterface
        {
            m_diContainer.Bind<TInterface>().To<TType>().FromResource(path).AsSingle().WithArguments(arguments);
        }

        public void BindInterfacesAsSingle<TType>(params object[] arguments)
        {
            m_diContainer.BindInterfacesAndSelfTo<TType>().WithArguments(arguments);
        }

        public void BindInterfacesFromResource<TType>(string path, params object[] arguments)
        {
            m_diContainer.BindInterfacesAndSelfTo<TType>().FromResource(path).WithArguments(arguments);
        }
    }
}
#endif