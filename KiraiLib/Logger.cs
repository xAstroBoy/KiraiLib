using MelonLoader;
using System;
namespace KiraiMod
{
    partial class KiraiLib
    {
        /// <summary>
        /// Utilities for writing to the screen
        /// </summary>
        public class Logger
        {
            /// <summary>
            /// Log information to the string
            /// </summary>
            /// <param name="text">Text to be logged</param>
            public static void Log(string text)
            {
                Log(text, 5);
            }

            /// <summary>
            /// Log information to the string
            /// </summary>
            /// <param name="name">Name of your mod</param>
            /// <param name="text">Text to be logged</param>
            /// <param name="duration">Amount of time for the text to remain before being erased</param>
            public static void Log(string name, string text, float duration)
            {
                Log($"[{name}] {text}", duration);
            }

            /// <summary>
            /// Log information to the string
            /// </summary>
            /// <param name="text">Text to be logged</param>
            /// <param name="duration">Amount of time for the text to remain before being erased</param>
            public static void Log(string text, float duration)
            {
                MelonCoroutines.Start(LogAndRemove(text, duration));
            }

            /// <summary>
            /// Forcefully clear the logs from the screen
            /// </summary>
            public static void ClearLog()
            {
                lines.Clear();
                log.text = "";
            }
        }
    }
}
