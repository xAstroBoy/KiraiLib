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
            /// <summary> Linear state UI element often used for adjustment</summary>
            public class Slider : UIElement
            {
                private Text valueText;
                private UnityEngine.UI.Slider uiSlider;

                private GameObject self;
                private Action<float> OnChange;

                public static Slider Create(string id, string label, float x, float y, float min, float max, float initial, Transform parent, Action<float> OnChange, bool managed = true)
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

                    Slider slider = new Slider(label, x, y, min, max, initial, parent, OnChange);

                    if (managed)
                    {
                        if (elements.ContainsKey(id))
                            MelonLogger.Error($"{id} is already a registered UI element");
                        else elements.Add(id, slider);
                    }

                    return slider;
                }

                public Slider(string label, float x, float y, float min, float max, float initial, Transform parent, Action<float> OnChange)
                {
                    QuickMenu qm = QuickMenu.prop_QuickMenu_0;
                    this.OnChange = OnChange;

                    GameObject slider = UnityEngine.Object.Instantiate(BaseSlider, parent).gameObject;

                    slider.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    slider.transform.localPosition = new Vector3(ButtonSize * (x - 0.25f), ButtonSize * (y + 2.5f), 0.01f);
                    slider.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    Action<float> OnValueChanged = new Action<float>(value => {
                        _SetValue(value);
                    });

                    uiSlider = slider.GetComponentInChildren<UnityEngine.UI.Slider>();
                    uiSlider.minValue = min;
                    uiSlider.maxValue = max;
                    uiSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
                    uiSlider.onValueChanged.AddListener(OnValueChanged);

                    slider.GetComponent<Image>().color = Colors.SliderInactive;
                    slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = Colors.SliderActive;

                    GameObject textGO = new GameObject("Text");
                    textGO.transform.SetParent(parent.transform, false);

                    Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");

                    Text textText = textGO.AddComponent<Text>();
                    textText.supportRichText = true;
                    textText.text = label;
                    textText.font = font;
                    textText.fontSize = 64;
                    textText.color = Colors.SliderText;
                    textText.alignment = TextAnchor.MiddleCenter;
                    textText.transform.localPosition = slider.transform.localPosition;
                    textText.transform.localPosition += new Vector3(0, 80, 0);
                    textText.GetComponent<RectTransform>().sizeDelta = new Vector2(textText.fontSize * label.Length, 100f);
                    textText.fontStyle = FontStyle.Bold;

                    UnityEngine.UI.Button textButton = textGO.AddComponent<UnityEngine.UI.Button>();
                    textButton.onClick.AddListener(new Action(() => {
                        HUDKeypad($"Set {label}", "Set", "Enter value...", new Action<string>(raw => {
                            if (float.TryParse(raw, out float val))
                            {
                                // this is not the best solution by far but given the amount of times
                                // on value changed is called its probably better than an if statement
                                uiSlider.onValueChanged.RemoveAllListeners();
                                SetValue(val);
                                uiSlider.onValueChanged.AddListener(OnValueChanged);
                            }
                        }));
                    }));

                    textGO.GetComponent<RectTransform>();

                    valueText = slider.transform.Find("Fill Area/Label").GetComponent<Text>();
                    valueText.color = Colors.SliderValueText;
                    valueText.fontStyle = FontStyle.Bold;

                    uiSlider.Set(initial, true);

                    self = slider.gameObject;
                }

                /// <summary> Set the value of the slider </summary>
                /// <param name="value">The value to set to</param>
                /// <remarks> This function has bounds checking </remarks>
                public void SetValue(float value)
                {
                    uiSlider.value = value;
                    _SetValue(value);
                }

                private void _SetValue(float value)
                {
                    valueText.text = value.ToString("0.00");
                    OnChange.Invoke(value);
                }

                /// <remarks> This should only be used if the button was made with the new keyword </remarks>
                public override void Destroy() => UnityEngine.Object.Destroy(self);
            }
        }
    }
}