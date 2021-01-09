using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    /// <summary>
    /// General utility library for VRChat
    /// </summary>
    public partial class KiraiLib
    {
        /// <summary>
        /// Logs a GameObject and all of its components to the console
        /// </summary>
        /// <param name="gameObject">The GameObject to log</param>
        /// <param name="max">Maximum amount of times to traverse down children</param>
        /// <param name="n_depth">This parameter should be ignored</param>
        public static void LogGameObject(GameObject gameObject, int max = -1, int? n_depth = null)
        {
            int depth = n_depth ?? 0;
            if (max != -1 && depth > max) return;

            MelonLogger.Log(ConsoleColor.Green, "".PadLeft(depth * 4, ' ') + gameObject.name);

            Component[] components = gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                MelonLogger.Log(
                    ConsoleColor.Cyan,
                    "".PadLeft((depth + 1) * 4, ' ') +
                    ((gameObject.name.Length + 2 < components[i].ToString().Length) ?
                    components[i].ToString().Substring(
                        gameObject.name.Length + 2,
                        components[i].ToString().Length - gameObject.name.Length - 3
                    ) : components[i].ToString())
                );
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                LogGameObject(gameObject.transform.GetChild(i).gameObject, max, depth + 1);
            }
        }

        /// <summary>
        /// Uses game methods to write text to the screen
        /// </summary>
        /// <param name="message">Message to be logged</param>
        [Obsolete("Use Log instead", false)]
        public static void HUDMessage(string message)
        {
            if (VRCUiManager.prop_VRCUiManager_0 == null) return;

            VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_String_0(message);
        }

        /// <summary> Fetches user </summary>
        /// <param name="title">Title shown at the top of the dialog box</param>
        /// <param name="text">Text used for the confirm button</param>
        /// <param name="placeholder">Example text in the text area</param>
        /// <param name="OnAccept">Action returning the user input if they confirm</param>
        public static void HUDInput(string title, string text, string placeholder, Action<string> OnAccept)
        {
            popup.Invoke(VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0, new object[] {
                    title,
                    "",
                    InputField.InputType.Standard,
                    false,
                    text,
                    UnhollowerRuntimeLib
                        .DelegateSupport
                        .ConvertDelegate<
                            Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>
                        >(
                            new Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>(
                                (a, b, c) =>
                                {
                                    OnAccept(a);
                                }
                            )
                        ),
                    null,
                    placeholder,
                    true,
                    null
                });
        }

        /// <summary> Gets a color based off of the current time, cycling through the rainbow </summary>
        /// <param name="speed">Multipler on the cycling speed</param>
        /// <remarks>Note: Black is not a part of the rainbow</remarks>
        public static Color GetRainbow(float speed = 1)
        {
            return new Color((float)Math.Sin(speed * Time.time) * 0.5f + 0.5f,
                (float)Math.Sin(speed * Time.time + (2 * 3.14 / 3)) * 0.5f + 0.5f,
                (float)Math.Sin(speed * Time.time + (4 * 3.14 / 3)) * 0.5f + 0.5f);
        }
    }
}
