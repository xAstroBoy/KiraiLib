using MelonLoader;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    /// <summary> General utility library for VRChat </summary>
    public partial class KiraiLib
    {
        /// <summary> Logs a GameObject and all of its components to the console </summary>
        /// <param name="gameObject">The GameObject to log</param>
        /// <param name="max">Maximum amount of times to traverse down children</param>
        /// <param name="n_depth">This parameter should be ignored</param>
        public static void LogGameObject(GameObject gameObject, int max = -1, int? n_depth = null)
        {
            int depth = n_depth ?? 0;
            if (max != -1 && depth > max) return;

            MelonLogger.Msg(ConsoleColor.Green, "".PadLeft(depth * 4, ' ') + gameObject.name);

            Component[] components = gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                MelonLogger.Msg(
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
        [Obsolete("Use Logger.Log instead", false)]
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
            // temp fix
            QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/SettingsButton").GetComponent<Button>().onClick.Invoke();
            
            popup.Invoke(VRCUiPopupManager.prop_VRCUiPopupManager_0, new object[] {
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

        /// <summary> Shows a keypad for integer input </summary>
        /// <param name="title"> The text at the top of the menu </param>
        /// <param name="confirm"> The text on the button to confirm input </param>
        /// <param name="placeholder"> Example text that shows up when nothing is entered </param>
        /// <param name="OnAccept"> Callback with the number entered as a string </param>
        /// <param name="OnCancel"> Callback when the user clicks cancel </param>
        public static void HUDKeypad(string title, string confirm, string placeholder, Action<string> OnAccept, Action OnCancel = null)
        {
            VRCUiPopupManager
                .prop_VRCUiPopupManager_0
                .Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_0(
                    title,
                    "",
                    InputField.InputType.Standard,
                    true,
                    confirm,
                    UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<
                        Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>
                    >(
                        new Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>(
                            (a, b, c) =>
                            {
                                OnAccept?.Invoke(a);
                            }
                        )
                    ),
                    UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<
                        Il2CppSystem.Action
                    >(
                        OnCancel
                    ),
                    placeholder,
                    true,
                    null
                    );
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

        /// <summary> Secure hashing algoritm 256 bit helper </summary>
        /// <param name="bytes"> The bytes to hash </param>
        /// <param name="caps"> use ABCDEF instead of abcdef </param>
        /// <returns> 32 byte long hash </returns>
        public static string SHA256(byte[] bytes, bool caps = false)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(bytes);
            foreach (byte theByte in crypto)
                hash.Append(theByte.ToString($"x2"));
            if (caps) return hash.ToString().ToUpper();
            else return hash.ToString();
        }

        /// <summary> 32 byte long cyclic redundancy check implementation </summary>
        // (from StackOverflow)
        public static class CRC32
        {
            private static readonly uint[] ChecksumTable = new uint[0x100];
            private static readonly uint Polynomial = 0xEDB88320;

            static CRC32()
            {
                for (uint index = 0; index < 0x100; ++index)
                {
                    uint item = index;
                    for (int bit = 0; bit < 8; ++bit)
                        item = ((item & 1) != 0) ? (Polynomial ^ (item >> 1)) : (item >> 1);
                    ChecksumTable[index] = item;
                }
            }

            private static byte[] ComputeHash(System.IO.Stream stream)
            {
                uint result = 0xFFFFFFFF;

                int current;
                while ((current = stream.ReadByte()) != -1)
                    result = ChecksumTable[(result & 0xFF) ^ (byte)current] ^ (result >> 8);

                byte[] hash = BitConverter.GetBytes(~result);
                Array.Reverse(hash);
                return hash;
            }

            private static byte[] ComputeHash(byte[] data)
            {
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream(data))
                    return ComputeHash(stream);
            }

            /// <summary>
            /// Convert data into a hash using a lossy algorithm
            /// </summary>
            /// <param name="data">Data to be hashed</param>
            /// <returns>8 character long string</returns>
            public static string Hash(byte[] data)
            {
                string hash = "";
                foreach (byte b in ComputeHash(data))
                {
                    hash += b.ToString("X");
                }
                return hash;
            }
        }
    }
}
