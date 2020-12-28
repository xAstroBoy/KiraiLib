using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    public partial class KiraiLib
    {
        private static Text log;
        private static System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();

        internal static void NOP() {}

        static KiraiLib()
        {
            MelonCoroutines.Start(Setup());
        }

        private static IEnumerator Setup()
        {
            while (VRCUiManager.prop_VRCUiManager_0 is null) yield return null;

            GameObject gameObject = new GameObject("KiraiLibLog");
            log = gameObject.AddComponent<Text>();

            gameObject.transform.SetParent(GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/NotificationDotParent").transform, false);
            gameObject.transform.localPosition = new Vector3(0, 750);

            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 30);

            log.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            log.horizontalOverflow = HorizontalWrapMode.Overflow;
            log.verticalOverflow = VerticalWrapMode.Overflow;
            log.alignment = TextAnchor.UpperLeft;
            log.fontStyle = FontStyle.Bold;
            log.supportRichText = true;
            log.fontSize = 36;
        }

        private static IEnumerator LogAndRemove(string text, float duration)
        {
            lines.Add(text);
            log.text = string.Join("\n", lines);
            yield return new WaitForSecondsRealtime(duration);
            lines.Remove(text);
            log.text = string.Join("\n", lines);
        }
    }
}
