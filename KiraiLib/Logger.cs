using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    partial class KiraiLib
    {
        /// <summary>
        /// Utilities for writing to the screen
        /// </summary>
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
