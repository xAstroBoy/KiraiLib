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
            /// <summary> Stateless UI element often used for displaying lists</summary>
            public class Label : UIElement
            {
                private GameObject self;
                private Text text;

                private Action OnClick;

                public static Label Create(string id, string text, int index, Transform parent, Action OnClick, bool managed = true)
                {
                    int x = 0;
                    int y = index * -70;

                    return Create(id, text, x, y, parent, OnClick, managed);
                }

                public static Label Create(string id, string text, float x, float y, Transform parent, Action OnClick, bool managed = true)
                {
                    if (overrides.TryGetValue(id, out var value))
                    {
                        if (value.Length == 1)
                        {
                            x = 0;
                            y = value[0] * -70;
                        }
                        else if (value.Length == 2)
                        {
                            x = value[0];
                            y = value[1];
                        }
                    }

                    Label label = new Label(text, x, y, parent, OnClick);

                    if (managed)
                    {
                        if (elements.ContainsKey(id))
                            MelonLogger.Error($"{id} is already a registered UI element");
                        else elements.Add(id, label);
                    }

                    return label;
                }

                public Label(string label, float x, float y, Transform parent, Action OnClick)
                {
                    this.OnClick = OnClick;

                    self = UnityEngine.Object.Instantiate(BaseLabel, parent.transform);

                    self.transform.SetParent(parent.transform, false);
                    self.transform.localPosition = new Vector3(x, y);
                    self.transform.localScale = Vector3.one;
                    self.transform.localRotation = Quaternion.identity;

                    text = self.GetComponent<Text>();
                    text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    text.fontSize = 64;
                    text.alignment = TextAnchor.UpperLeft;
                    text.color = Color.white;
                    text.supportRichText = true;
                    text.horizontalOverflow = HorizontalWrapMode.Overflow;
                    text.text = label;
                    text.color = Colors.LabelText;

                    UnityEngine.UI.Button button = self.AddComponent<UnityEngine.UI.Button>();
                    button.onClick.AddListener(OnClick);

                    self.transform.GetComponent<RectTransform>().sizeDelta = new Vector3(735, 0);
                }
                
                /// <summary> Simulate a click as if it were actually clicked </summary>
                public void Click() => OnClick.Invoke();

                /// <remarks> This should only be used if the button was made with the new keyword </remarks>
                public override void Destroy() => UnityEngine.Object.Destroy(self);
            }
        }
    }
}