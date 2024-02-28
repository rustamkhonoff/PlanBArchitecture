using System;
using System.Collections.Generic;

namespace SaveService
{
    public class SavesMapContainer : ISavesMap
    {
        public SavesMapContainer(Dictionary<Type, string> map)
        {
            Map = map;
        }

        public Dictionary<Type, string> Map { get; }
    }
}