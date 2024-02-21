using System;

namespace Services.SaveService
{
    public interface ISaveService
    {
        public void Save<T>(T data) where T : class;
        public T Load<T>(Func<T> newInstanceFunc = null) where T : class;
        void AddSavesMap(ISavesMap savesMap);
        void AddSaveMap(Type type, string key);
    }
}