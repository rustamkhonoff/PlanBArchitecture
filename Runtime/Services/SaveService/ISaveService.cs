using System;

namespace SaveService
{
    public interface ISaveService
    {
        public void Save<T>(T data, Func<T, string> customDataFormatter = null) where T : class;
        public T Load<T>(Func<T> newInstanceFunc = null, Func<string, T> customDataFormatter = null) where T : class;
        void AddSavesMap(ISavesMap savesMap);
        void AddSaveMap(Type type, string key);
    }
}