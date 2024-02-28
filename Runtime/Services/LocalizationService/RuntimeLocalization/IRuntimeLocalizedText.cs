using System;

namespace LocalizationService.RuntimeLocalization
{
    public interface IRuntimeLocalizedText
    {
        void UpdateText();
        void UpdateKey(string key, bool autoUpdate);
        void UpdateTable(string table, bool autoUpdate);
        void UpdateFormat(string format, bool autoUpdate);
        void UpdateArguments(Func<object[]> func, bool autoUpdate);
    }
}