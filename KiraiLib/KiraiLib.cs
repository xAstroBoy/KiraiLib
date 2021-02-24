using MelonLoader;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    public static partial class KiraiLib
    {
        private static Text log;
        private static System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();

        /// <summary> This function does absolutely nothing. </summary>
        public static void NoOp() { }

        private static object OnUpdateToken;
        private static bool Unloaded;

        private static MethodInfo popup;

        static KiraiLib()
        {
            MelonCoroutines.Start(SetupUI());

            popup = typeof(VRCUiPopupManager).GetMethod(
                nameof(VRCUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_1),
                BindingFlags.Public | BindingFlags.Instance);

            Callbacks.OnUIUnload += () =>
            {
                Unloaded = true;
                Logger.ClearLog();
            };

            Callbacks.OnUIReload += () => Unloaded = false;
        }

        private static IEnumerator SetupUI()
        {
            while (VRCUiManager.prop_VRCUiManager_0 is null) yield return null;

            #region Create Logger
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
            #endregion

            UI.Initialize();

            OnUpdateToken = MelonCoroutines.Start(OnUpdate());
        }

        private static IEnumerator OnUpdate()
        {
            for (;;)
            {
                yield return null;

                UI.HandlePages();
            }
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
    }
}
