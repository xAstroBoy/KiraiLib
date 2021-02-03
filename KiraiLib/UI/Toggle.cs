using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace KiraiMod
{
    public static partial class KiraiLib
    {
        public partial class UI
        {
            /// <summary> Dual state UI element often used for boolean operations </summary>
            public class Toggle : UIElement
            {
                /// <summary> Creates a button and optionally registers it for automatic management </summary>
                /// <param name="id"> Unique id to get this element later </param>
                /// <param name="label"> Text to show up on the toggle </param>
                /// <param name="tooltip"> Text to show up on the hint when hovered over </param>
                /// <param name="x"> Horizontal position </param>
                /// <param name="y"> Vertical position </param>
                /// <param name="state"> Initial state at the time the toggle is create </param>
                /// <param name="parent"> Parent transform </param>
                /// <param name="OnEnable"> Called when the value becomes true </param>
                /// <param name="OnDisable"> Called when the value becomes false </param>
                /// <param name="managed"> Make this element get automatically deleted on UI unload and be fetchable by id </param>
                /// <returns> New toggle element </returns>
                /// <remarks>Note: if managed is false then the id is ignored</remarks>
                public static Toggle Create(string id, string label, string tooltip, float x, float y, bool state, Transform parent, Action OnEnable, Action OnDisable, bool managed = true)
                {
                    if (overrides.TryGetValue(id, out var value))
                    {
                        if (value.Length == 1)
                        {
                            x = value[0] % 4 - 1;
                            y = 1 - value[0] / 4;
                        }
                        else if (value.Length == 2)
                        {
                            x = value[0];
                            y = value[1];
                        }
                    }

                    Toggle toggle = new Toggle(label, tooltip, x, y, state, parent, OnEnable, OnDisable);
                    if (managed) elements.Add(id, toggle);
                    return toggle;
                }

                /// <summary> Creates a button and optionally registers it for automatic management </summary>
                /// <param name="id"> Unique id to get this element later </param>
                /// <param name="label"> Text to show up on the toggle </param>
                /// <param name="tooltip"> Text to show up on the hint when hovered over </param>
                /// <param name="x"> Horizontal position </param>
                /// <param name="y"> Vertical position </param>
                /// <param name="state"> Initial state at the time the toggle is create </param>
                /// <param name="parent"> Parent transform </param>
                /// <param name="OnChange"> Called when the value changes </param>
                /// <param name="managed"> Make this element get automatically deleted on UI unload and be fetchable by id </param>
                /// <returns> New toggle element </returns>
                /// <remarks>Note: if managed is false then the id is ignored</remarks>
                public static Toggle Create(string id, string label, string tooltip, float x, float y, bool state, Transform parent, Action<bool> OnChange, bool managed = true)
                {
                    if (overrides.TryGetValue(id, out var value))
                    {
                        if (value.Length == 1)
                        {
                            x = value[0] % 4 - 1;
                            y = 1 - value[0] / 4;
                        }
                        else if (value.Length == 2)
                        {
                            x = value[0];
                            y = value[1];
                        }
                    }

                    Toggle toggle = new Toggle(label, tooltip, x, y, state, parent,
                        new Action(() => OnChange.Invoke(true)),
                        new Action(() => OnChange.Invoke(false))
                    );

                    if (managed)
                    {
                        if (elements.ContainsKey(id))
                            MelonLogger.LogError($"{id} is already a registered UI element");
                        else elements.Add(id, toggle);
                    }

                    return toggle;
                }

                private GameObject self;

                private GameObject on;
                private GameObject off;

                private Action OnEnable;
                private Action OnDisable;

                public bool state;

                /// <summary> Creates a new dual state UI element </summary>
                /// <param name="label"> Text to appear on the toggle </param>
                /// <param name="tooltip"> Tooltip to appear on hover. Appears at the top of the menu </param>
                /// <param name="x"> Horizontal position </param>
                /// <param name="y"> Vertical position </param>
                /// <param name="state"> Initial state </param>
                /// <param name="parent"> Parent transform </param>
                /// <param name="OnEnable"> Called when the value becomes true </param>
                /// <param name="OnDisable"> Called when the value becomes false </param>
                /// <remarks>Note: this class is very basic but fast. It is recommended to use a more abstract interface</remarks>
                public Toggle(string label, string tooltip, float x, float y, bool state, Transform parent, Action OnEnable, Action OnDisable)
                {
                    this.state = state;
                    this.OnEnable = OnEnable;
                    this.OnDisable = OnDisable;

                    GameObject button = UnityEngine.Object.Instantiate(BaseToggle.gameObject, parent);

                    button.transform.localPosition = new Vector3(
                        button.transform.localPosition.x + (ButtonSize * (x + 1)),
                        button.transform.localPosition.y + (ButtonSize * (y + 1)) + 18,
                        button.transform.localPosition.z
                    );

                    on = button.transform.Find("Toggle_States_HUDEnabled/ON").gameObject;
                    on.SetActive(state);
                    on.GetComponentInChildren<Image>().color = Colors.ToggleOn;

                    off = button.transform.Find("Toggle_States_HUDEnabled/OFF").gameObject;
                    off.SetActive(!state);
                    off.GetComponentInChildren<Image>().color = Colors.ToggleOff;

                    Text[] onTexts = on.GetComponentsInChildren<Text>();
                    onTexts[0].text = label + " On";
                    onTexts[1].text = label + " Off";
                    onTexts[0].resizeTextForBestFit = true;
                    onTexts[1].resizeTextForBestFit = true;

                    Text[] offTexts = off.GetComponentsInChildren<Text>();
                    offTexts[0].text = label + " On";
                    offTexts[1].text = label + " Off";
                    offTexts[0].resizeTextForBestFit = true;
                    offTexts[1].resizeTextForBestFit = true;

                    UiTooltip buttonTooltip = button.transform.GetComponent<UiTooltip>();
                    buttonTooltip.field_Public_String_0 = tooltip;
                    buttonTooltip.field_Public_String_1 = tooltip;

                    UnityEngine.UI.Button _button = button.transform.GetComponent<UnityEngine.UI.Button>();
                    _button.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    _button.onClick.AddListener(new Action(() =>
                    {
                        SetState();
                    }));

                    button.gameObject.active = true;

                    self = button;
                }

                /// <summary>
                /// Sets the state of the button
                /// </summary>
                /// <param name="n_state"> Value to be set to, defaults to the inverse of the current value </param>
                public void SetState(bool? n_state = null)
                {
                    if (state == n_state) return;

                    state = n_state ?? !state;

                    on.SetActive(state);
                    off.SetActive(!state);

                    if (!state) OnDisable.Invoke(); else OnEnable.Invoke();
                }

                /// <remarks> This should only be used if the button was made with the new keyword </remarks>
                public override void Destroy() => UnityEngine.Object.Destroy(self);
            }
        }
    }
}