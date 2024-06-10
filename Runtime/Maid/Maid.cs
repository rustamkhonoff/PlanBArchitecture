using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Maid
{
    public static class Maid
    {
        private const string InfoColor = nameof(Color.white);
        private const string WarningColor = nameof(Color.yellow);
        private const string ErrorColor = nameof(Color.red);
        private const string SuccessColor = nameof(Color.green);

        private const string ConditionString = "MAID_LOGS_ENABLED";

        #region Success

        [Conditional(ConditionString)]
        public static void LogSuccess(object message)
        {
            Debug.Log(FormatMessage(SuccessColor, message));
        }

        [Conditional(ConditionString)]
        public static void LogSuccess(object message, string category)
        {
            Debug.Log(FormatMessageWithCategory(SuccessColor, category, message));
        }

        [Conditional(ConditionString)]
        public static void LogFormatSuccess(string format, params object[] args)
        {
            Debug.Log(FormatMessage(SuccessColor, string.Format(format, args)));
        }

        [Conditional(ConditionString)]
        public static void LogFormatSuccess(string category, string format, params object[] args)
        {
            Debug.Log(FormatMessageWithCategory(SuccessColor, category, string.Format(format, args)));
        }

        #endregion

        #region Log

        [Conditional(ConditionString)]
        public static void Log(object message)
        {
            Debug.Log(FormatMessage(InfoColor, message));
        }

        [Conditional(ConditionString)]
        public static void Log(object message, string category)
        {
            Debug.Log(FormatMessageWithCategory(InfoColor, category, message));
        }

        [Conditional(ConditionString)]
        public static void LogFormat(string format, params object[] args)
        {
            Debug.Log(FormatMessage(InfoColor, string.Format(format, args)));
        }

        [Conditional(ConditionString)]
        public static void LogFormat(string category, string format, params object[] args)
        {
            Debug.Log(FormatMessageWithCategory(InfoColor, category, string.Format(format, args)));
        }

        #endregion

        #region Warning

        [Conditional(ConditionString)]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(FormatMessage(WarningColor, message));
        }

        [Conditional(ConditionString)]
        public static void LogWarning(object message, string category)
        {
            Debug.LogWarning(FormatMessageWithCategory(WarningColor, category, message));
        }

        [Conditional(ConditionString)]
        public static void LogWarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(FormatMessage(WarningColor, string.Format(format, args)));
        }

        [Conditional(ConditionString)]
        public static void LogWarningFormat(string category, string format, params object[] args)
        {
            Debug.LogWarningFormat(FormatMessageWithCategory(WarningColor, category, string.Format(format, args)));
        }

        #endregion

        #region Error

        [Conditional(ConditionString)]
        public static void LogError(object message)
        {
            Debug.LogError(FormatMessage(ErrorColor, message));
        }

        [Conditional(ConditionString)]
        public static void LogError(object message, string category)
        {
            Debug.LogError(FormatMessageWithCategory(ErrorColor, category, message));
        }

        [Conditional(ConditionString)]
        public static void LogErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(FormatMessage(ErrorColor, string.Format(format, args)));
        }

        [Conditional(ConditionString)]
        public static void LogErrorFormat(string category, string format, params object[] args)
        {
            Debug.LogErrorFormat(FormatMessageWithCategory(ErrorColor, category, string.Format(format, args)));
        }

        #endregion

        #region Exception

        [Conditional(ConditionString)]
        public static void LogException(Exception exception)
        {
            Debug.LogError(FormatMessage(ErrorColor, exception.Message));
        }

        [Conditional(ConditionString)]
        public static void LogException(Exception exception, string category)
        {
            Debug.LogError(FormatMessageWithCategory(ErrorColor, category, exception.Message));
        }

        #endregion

        #region Helpers

        private static string FormatMessage(string color, object message)
        {
            return $"<color={color}>{message}</color>";
        }

        private static string FormatMessageWithCategory(string color, string category, object message)
        {
            return $"<color={color}><b>[{category}]</b> {message}</color>";
        }

        #endregion
    }
}