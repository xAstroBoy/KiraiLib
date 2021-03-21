using MelonLoader;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    partial class KiraiLib
    {
        /// <summary> Utilities for writing to the screen </summary>
        public static class Logger
        {
            private static Text log;

            internal static void Initialize()
            {
                GameObject gameObject = new GameObject("KiraiLibLog");
                log = gameObject.AddComponent<Text>();

                gameObject.transform.SetParent(GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud").transform, false);
                gameObject.transform.localPosition = new Vector3(15, 300);

                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 30);

                log.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                log.horizontalOverflow = HorizontalWrapMode.Wrap;
                log.verticalOverflow = VerticalWrapMode.Overflow;
                log.alignment = TextAnchor.UpperLeft;
                log.fontStyle = FontStyle.Bold;
                log.supportRichText = true;
                log.fontSize = 30;
            }

            private static IEnumerator LogAndRemove(string text, float duration)
            {
                if (Unloaded) yield break;

                lines.Add(text);
                log.text = string.Join("\n", lines);
                yield return new WaitForSecondsRealtime(duration);
                lines.Remove(text);
                log.text = string.Join("\n", lines);
            }

            /// <summary> Log information to the string </summary>
            /// <param name="text">Text to be logged</param>
            public static void Log(string text) => Display(text, 5);
            /// <summary> Log information to the string </summary>
            /// <param name="name">Name of your mod</param>
            /// <param name="text">Text to be logged</param>
            /// <param name="duration">Amount of time for the text to remain before being erased</param>
            public static void Log(string name, string text, float duration) => Display($"[{name}] {text}", duration);
            /// <summary> Log information to the string </summary>
            /// <param name="text">Text to be logged</param>
            /// <param name="duration">Amount of time for the text to remain before being erased</param>
            [Obsolete("Use Display instead")]
            public static void Log(string text, float duration) => Display(text, duration);
            /// <summary> Show something on the in-game log </summary>
            /// <param name="text">Text to be logged</param>
            /// <param name="duration">Amount of time for the text to remain before being erased</param>
            public static void Display(string text, float duration) => MelonCoroutines.Start(LogAndRemove(text, duration));

            [Obsolete("Use ClearDisplay instead", false)]
            public static void ClearLog() => ClearDisplay();
            /// <summary> Forcefully clear the logs from the screen </summary>
            public static void ClearDisplay()
            {
                lines.Clear();
                log.text = "";
            }

            private static void LogEx(LogLevel level, string message)
            {
                if (level >= display)
                {
                    string hex =
                        level == LogLevel.TRACE ? "888" :
                        level == LogLevel.DEBUG ? "08f" :
                        level == LogLevel.INFO  ? "ccf" :
                        level == LogLevel.WARN  ? "fa0" :
                        level == LogLevel.ERROR ? "f00" : "800";

                    Display($"<color=#{hex}>{message}</color>", 1.5f);
                }

                if (level >= console)
                {
                    string vt =
                        level == LogLevel.DEBUG ? Constants.CC_FgBlue   :
                        level == LogLevel.INFO  ? Constants.CC_FgCyan   :
                        level == LogLevel.WARN  ? Constants.CC_FgYellow :
                        level == LogLevel.ERROR || 
                        level == LogLevel.FATAL ? Constants.CC_FgRed    : Constants.CC_Reset;

                    MelonLogger.Msg($"{Constants.CC_FgMagenta}[{vt}{Enum.GetName(typeof(LogLevel), level),5}{Constants.CC_FgMagenta}]{Constants.CC_Reset} {message}");
                }
            }

            public static void Trace(string message) => LogEx(LogLevel.TRACE, message); 
            public static void Debug(string message) => LogEx(LogLevel.DEBUG, message);
            public static void Info(string message) => LogEx(LogLevel.INFO, message);
            public static void Warn(string message) => LogEx(LogLevel.WARN, message);
            public static void Error(string message) => LogEx(LogLevel.ERROR, message);
            public static void Fatal(string message) => LogEx(LogLevel.FATAL, message);

            /// <summary> Log level to appear on screen </summary>
            public static LogLevel display = LogLevel.INFO;
            /// <summary> Log level to appear in the logs </summary>
            public static LogLevel console = LogLevel.DEBUG;
            /// <summary> Filtering for logger </summary>
            public enum LogLevel
            {
                TRACE,
                DEBUG,
                INFO,
                WARN,
                ERROR,
                FATAL
            }
        }
    }
}
