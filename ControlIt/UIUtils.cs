using ColossalFramework.UI;
using UnityEngine;

namespace ControlIt
{
    public class UIUtils
    {
        public static UIFont GetUIFont(string name)
        {
            UIFont[] fonts = Resources.FindObjectsOfTypeAll<UIFont>();

            foreach (UIFont font in fonts)
            {
                if (font.name.CompareTo(name) == 0)
                {
                    return font;
                }
            }

            return null;
        }

        public static UIPanel CreatePanel(string name)
        {
            UIPanel panel = UIView.GetAView().AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.name = name;

            return panel;
        }

        public static UILabel CreateLabel(UIComponent parent, string name, string text)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = name;
            label.text = text;
            label.font = GetUIFont("OpenSans-Regular");
            label.autoSize = false;
            label.height = 20f;
            label.verticalAlignment = UIVerticalAlignment.Top;
            label.relativePosition = new Vector3(0f, 0f);

            return label;
        }
    }
}
