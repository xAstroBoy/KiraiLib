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
            /// <summary> Stateless UI element often used for function calls </summary>
            public class HalfButton : UIElement
            {
                /// <summary> Creates a button and optionally registers it for automatic management </summary>
                /// <param name="id"> Unique id to get this element later </param>
                /// <param name="label"> Text to show up on the button </param>
                /// <param name="tooltip"> Text to show up on the hint when hovered over </param>
                /// <param name="x"> Horizontal position </param>
                /// <param name="y"> Vertical position </param>
                /// <param name="parent"> Parent transform </param>
                /// <param name="OnClick"> Called when the button is clicked </param>
                /// <param name="managed"> Make this element get automatically deleted on UI unload and be fetchable by id </param>
                /// <returns> New button element </returns>
                /// <remarks>Note: if managed is false then the id is ignored</remarks>
                public static HalfButton Create(string id, string label, string tooltip, float x, float y, bool lower, Transform parent, Action OnClick, bool managed = true)
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

                    HalfButton button = new HalfButton(label, tooltip, x, y, lower, parent, OnClick);

                    if (managed)
                    {
                        if (elements.ContainsKey(id))
                            MelonLogger.Error($"{id} is already a registered UI element");
                        else elements.Add(id, button);
                    }

                    return button;
                }

                private GameObject self;
                private Action OnClick;

                /// <summary> Creates a button and optionally registers it for automatic management </summary>
                /// <param name="label"> Text to appear on the button </param>
                /// <param name="tooltip"> Tooltip to appear on hover. Appears at the top of the menu </param>
                /// <param name="x"> Horizontal position </param>
                /// <param name="y"> Vertical position </param>
                /// <param name="lower"> Is the button supposed to occupy the lower half of the unit </param>
                /// <param name="parent"> Parent transform </param>
                /// <param name="OnClick"> Called when the button is clicked </param>
                /// <remarks>Note: this class is very basic but fast. It is recommended to use a more abstract interface</remarks>
                public HalfButton(string label, string tooltip, float x, float y, bool lower, Transform parent, Action OnClick)
                {
                    this.OnClick = OnClick;

                    GameObject button = UnityEngine.Object.Instantiate(BaseButton.gameObject, parent);

                    button.transform.localPosition = new Vector3(
                        button.transform.localPosition.x + (ButtonSize * (x - 1)),
                        button.transform.localPosition.y + (ButtonSize * y) + (ButtonSize / 4 * (lower ? -1 : 1)),
                        button.transform.localPosition.z
                    );

                    button.transform.localScale = new Vector3(1, 0.5f, 1);

                    Text text = button.transform.GetComponentInChildren<Text>();
                    text.color = Colors.ButtonText;
                    text.text = label;
                    text.transform.localScale = new Vector3(1, 2, 1);

                    button.transform.GetComponentInChildren<Image>().color = Colors.ButtonBackground;

                    UiTooltip buttonTooltip = button.transform.GetComponentInChildren<UiTooltip>();
                    buttonTooltip.field_Public_String_0 = tooltip;
                    buttonTooltip.field_Public_String_1 = tooltip;

                    UnityEngine.UI.Button buttonClick = button.transform.GetComponentInChildren<UnityEngine.UI.Button>();
                    buttonClick.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    buttonClick.onClick.AddListener(OnClick);

                    self = button;
                }

                /// <summary> Simulate a click as if it were actually clicked </summary>
                public void Click() => OnClick.Invoke();

                /// <remarks> This should only be used if the button was made with the new keyword </remarks>
                public override void Destroy() => UnityEngine.Object.Destroy(self);
            }
        }
    }
}