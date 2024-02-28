using System;
using System.Collections.Generic;

namespace SaveService
{
    public interface ISavesMap
    {
        public Dictionary<Type, string> Map { get; }
    }
}