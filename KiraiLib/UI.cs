using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    public static partial class KiraiLib
    {
        /// <summary> </summary>
        public static partial class UI
        {
            /// <summary> The page when you first open the QuickMenu </summary>
            public static GameObject ShortcutMenu;

            /// <summary> The page when you click on a user </summary>
            public static GameObject UserInteractMenu;

            private static GameObject BaseToggle;
            private static GameObject BaseButton;
            private static GameObject BaseSlider;
            private static GameObject BaseLabel;

            private static int lastSelected = -1;

            /// <summary> ID for the current selected page. </summary>
            /// <remarks> Note: -1 to close custom pages </remarks>
            public static int selected = -1;

            private static readonly float ButtonSize = 420;

            /// <summary> Setup the UI for use </summary>
            /// <remarks> Note: this is called automatically but not until after VRChat_OnUiManagerInit </remarks>
            public static void Initialize()
            {
                ShortcutMenu = ShortcutMenu ?? QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu").gameObject;
                UserInteractMenu = UserInteractMenu ?? QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu").gameObject;
                BaseToggle = BaseToggle ?? QuickMenu.prop_QuickMenu_0.transform.Find("UIElementsMenu/ToggleHUDButton").gameObject;
                BaseButton = BaseButton ?? QuickMenu.prop_QuickMenu_0.transform.Find("NotificationInteractMenu/BlockButton").gameObject;
                BaseSlider = BaseSlider ?? VRCUiManager.prop_VRCUiManager_0.field_Private_Transform_0.transform.Find("Screens/Settings/AudioDevicePanel/VolumeSlider").gameObject;
                BaseLabel = BaseLabel ?? ShortcutMenu.transform.Find("WorldsButton/Text").gameObject;
            }

            /// <summary> Position overrides for buttons </summary>
            /// <remarks> If the length of the key is 1 then it is treated as an index, 2 is an x and a y, and anything else is ignored </remarks>
            public static Dictionary<string, float[]> overrides = new Dictionary<string, float[]>();
            /// <summary> All UIElements creates </summary>
            /// <remarks> Note: this excludes pages </remarks>
            public static Dictionary<string, UIElement> elements = new Dictionary<string, UIElement>();
            /// <summary> All pages generated </summary>
            public static List<GameObject> pages = new List<GameObject>();

            #region UIElement
            /// <summary> Base that all UI elements derive from </summary>
            public abstract class UIElement
            {
                /// <summary> Used internally to destroy an element</summary>
                public abstract void Destroy();
            }
            #endregion
            #region Page
            /// <summary> Creates a new page </summary>
            /// <returns> ID that will be used for Select </returns>
            /// <seealso cref="Select(int)"/>
            public static int CreatePage(string name = "KiraiLibPage")
            {
                int index = pages.Count;

                GameObject page = UnityEngine.Object.Instantiate(ShortcutMenu);
                page.name = name;

                for (int i = 0; i < page.transform.childCount; i++)
                    UnityEngine.Object.Destroy(page.transform.GetChild(i).gameObject);

                UnityEngine.Object.Destroy(page.GetComponent<ShortcutMenu>());

                page.transform.SetParent(QuickMenu.prop_QuickMenu_0.transform, false);
                page.gameObject.SetActive(false);

                pages.Add(page);

                return index;
            }
            #endregion
            #region Colors
            /// <summary> Styling used for new buttons </summary>
            public static class Colors
            {
                /// <summary> Color for the text on a toggle </summary>
                public static Color ToggleText = Color.white;

                /// <summary> Color for when a toggle is set to true </summary>
                public static Color ToggleOn = new Color(0.8f, 0.8f, 1f);

                /// <summary> Color for when a toggle is set to false </summary>
                public static Color ToggleOff = new Color(0.34f, 0f, 0.65f);

                /// <summary> Color for the text on a button </summary>
                public static Color ButtonText = Color.white;

                /// <summary> Color for the outline of a button </summary>
                public static Color ButtonBackground = Color.black;

                /// <summary> Color for the text above a slider </summary>
                public static Color SliderText = new Color(0.8f, 0.8f, 1f);

                /// <summary> Color for the text inside a slider </summary>
                public static Color SliderValueText = new Color(0.34f, 0f, 0.65f);

                /// <summary> Color for the left side of the slider </summary>
                public static Color SliderActive = new Color(0.8f, 0.8f, 1f);

                /// <summary> Color for the right side of the slider </summary>
                public static Color SliderInactive = new Color(0.34f, 0f, 0.65f);

                /// <summary> Color for the text on a label </summary>
                public static Color LabelText = Color.white;
            }
            #endregion

            // i would convert this to be event based however i cannot find a reliable way 
            // to hook OnMenuOpened and OnMenuClosed without it breaking every game update
            internal static void HandlePages()
            {
                if (QuickMenu.prop_QuickMenu_0 is null) return;

                if (!QuickMenu.prop_QuickMenu_0.prop_Boolean_0)
                    selected = -1;

                if (selected != -1)
                {
                    ShortcutMenu.active = false;
                    UserInteractMenu.active = false;
                }

                if (selected != lastSelected)
                {
                    if (lastSelected > -1)
                    {
                        if (pages.Count > lastSelected)
                            pages[lastSelected].active = false;
                    }
                    else ShortcutMenu.active = false;

                    if (selected > -1)
                    {
                        if (pages.Count > selected)
                            pages[selected].active = true;
                    }
                    else if (QuickMenu.prop_QuickMenu_0.prop_Boolean_0)
                        ShortcutMenu.active = true;

                    lastSelected = selected;
                }
            }

            /// <summary> Select a page by ID </summary>
            /// <remarks> Note: this is exactly the same as just assigning to selected </remarks>
            public static void Select(int id) => selected = id;

            /// <summary> Deletes all managed UI elements and notifies subscribers </summary>
            public static void Unload()
            {
                foreach (KeyValuePair<string, UIElement> kvp in elements)
                    kvp.Value.Destroy();
                elements.Clear();

                foreach (GameObject key in pages)
                    UnityEngine.Object.Destroy(key);
                pages.Clear();

                MelonLoader.MelonCoroutines.Stop(OnUpdateToken);
                OnUpdateToken = null;

                Callbacks.OnUIUnload();
            }

            /// <summary> Notifies all subscribers to reinitialize their UIs</summary>
            public static void Reload()
            {
                Unload();

                Callbacks.OnUIReload();

                OnUpdateToken = MelonLoader.MelonCoroutines.Start(OnUpdate());
            }
        }
    }
}
