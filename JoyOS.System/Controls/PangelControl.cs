using Microsoft.Xna.Framework;
using System;

namespace JoyOS.System.Controls
{
    public class PanelControl : Control
    {
        public void LayoutColumn(float xMargin, float yMargin, float ySpacing)
        {
            float num = yMargin;
            for (int i = 0; i < base.ChildCount; i++)
            {
                Control control = base[i];
                control.Position = new Vector2
                {
                    X = xMargin,
                    Y = num
                };
                num += control.Size.Y + ySpacing;
            }
            base.InvalidateAutoSize();
        }
        public void LayoutRow(float xMargin, float yMargin, float xSpacing)
        {
            float num = xMargin;
            for (int i = 0; i < base.ChildCount; i++)
            {
                Control control = base[i];
                control.Position = new Vector2
                {
                    X = num,
                    Y = yMargin
                };
                num += control.Size.X + xSpacing;
            }
            base.InvalidateAutoSize();
        }
    }
}
