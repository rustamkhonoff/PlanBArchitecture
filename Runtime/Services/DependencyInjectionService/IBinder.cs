namespace DependencyInjectionService
{
    public interface IBinder
    {
        void BindAsTransient<TType>(params object[] arguments);
        void BindAsTransientFromResource<TType>(string path, params object[] arguments);
        void BindAsTransient<TInterface, TType>(params object[] arguments) where TType : TInterface;
        void BindAsTransientFromResource<TInterface, TType>(string path, params object[] arguments) where TType : TInterface;
        void BindAsSingle<TType>(params object[] arguments);
        void BindAsSingle<TInterface, TType>(params object[] arguments) where TType : TInterface;
        void BindAsSingleFromResources<TType>(string path, params object[] arguments);
        void BindAsSingleFromResources<TInterface, TType>(string path, params object[] arguments) where TType : TInterface;
        void BindInterfacesAsSingle<TType>(params object[] arguments);
        void BindInterfacesFromResource<TType>(string path, params object[] arguments);
    }
}