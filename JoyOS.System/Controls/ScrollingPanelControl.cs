using Microsoft.Xna.Framework;
using System;
using JoyOS.System.Screen;

namespace JoyOS.System.Controls
{
        public class ScrollingPanelControl : PanelControl
        {
            private ScrollTracker scrollTracker = new ScrollTracker();
            public override void Update(GameTime gametime)
            {
                Vector2 vector = this.ComputeSize();
                this.scrollTracker.CanvasRect.Width = (int)vector.X;
                this.scrollTracker.CanvasRect.Height = (int)vector.Y;
                this.scrollTracker.Update(gametime);
                base.Update(gametime);
            }
            public override void HandleInput(InputHelper input)
            {
                this.scrollTracker.HandleInput(input);
                base.HandleInput(input);
            }
            public override void Draw(DrawContext context)
            {
                context.DrawOffset.Y = (float)(-(float)this.scrollTracker.ViewRect.Y);
                base.Draw(context);
            }
        }
    }
