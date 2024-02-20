using System;

namespace Services.LocalizationService.Runtime
{
    public interface IRuntimeLocalizedTMP
    {
        void UpdateText();
        void UpdateKey(string key, bool autoUpdate);
        void UpdateTable(string table, bool autoUpdate);
        void UpdateFormat(string format, bool autoUpdate);
        void UpdateArguments(Func<object[]> func, bool autoUpdate);
    }
}