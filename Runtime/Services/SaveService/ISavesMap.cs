using System;
using System.Collections.Generic;

namespace Services.SaveService
{
    public interface ISavesMap
    {
        public Dictionary<Type, string> Map { get; }
    }
}