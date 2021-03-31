using MelonLoader;
using System.Collections;
using System.Net.Http;
using System.Reflection;

namespace KiraiMod
{
    public static partial class KiraiLib
    {
        private static System.Collections.Generic.List<string> lines = new System.Collections.Generic.List<string>();

        private static object OnUpdateToken;
        private static bool Unloaded;

        private static MethodInfo popup;
        private static HttpClient http;

        static KiraiLib()
        {
            SDK.Initialize();

            http = new HttpClient();

            MelonCoroutines.Start(SetupUI());

            popup = typeof(VRCUiPopupManager).GetMethod(
                nameof(VRCUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_0),
                BindingFlags.Public | BindingFlags.Instance);

            Events.OnUIUnload += () =>
            {
                Unloaded = true;
                Logger.ClearDisplay();
            };

            Events.OnUIReload += () => Unloaded = false;
        }

        private static IEnumerator SetupUI()
        {
            while (VRCUiManager.prop_VRCUiManager_0 is null) yield return null;

            Logger.Initialize();
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
    }
}
