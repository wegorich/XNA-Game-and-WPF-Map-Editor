using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using JoyOS.System.Screen;

namespace JoyOS.System.Controls
{
        public class ScrollTracker
        {
            public const GestureType GesturesNeeded = GestureType.VerticalDrag | GestureType.Flick | GestureType.DragComplete;
            private const float SpringMaxDrag = 400f;
            private const float SpringMaxOffset = 133.333328f;
            private const float SpringReturnRate = 0.1f;
            private const float SpringReturnMin = 2f;
            private const float Deceleration = 500f;
            private const float MaxVelocity = 2000f;
            public Rectangle CanvasRect;
            public Rectangle ViewRect;
            private Vector2 Velocity;
            private Vector2 ViewOrigin;
            private Vector2 UnclampedViewOrigin;
            public Rectangle FullCanvasRect
            {
                get
                {
                    Rectangle canvasRect = this.CanvasRect;
                    if (canvasRect.Width < this.ViewRect.Width)
                    {
                        canvasRect.Width = this.ViewRect.Width;
                    }
                    if (canvasRect.Height < this.ViewRect.Height)
                    {
                        canvasRect.Height = this.ViewRect.Height;
                    }
                    return canvasRect;
                }
            }
            public bool IsTracking
            {
                get;
                private set;
            }
            public bool IsMoving
            {
                get
                {
                    return this.IsTracking || this.Velocity.X != 0f || this.Velocity.Y != 0f || !this.FullCanvasRect.Contains(this.ViewRect);
                }
            }
            public ScrollTracker()
            {
                this.ViewRect = new Rectangle
                {
                    Width = TouchPanel.DisplayWidth,
                    Height = TouchPanel.DisplayHeight
                };
                this.CanvasRect = this.ViewRect;
            }
            public void Update(GameTime gametime)
            {
                float dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                Vector2 vector = new Vector2
                {
                    X = 0f,
                    Y = 0f
                };
                Vector2 vector2 = new Vector2
                {
                    X = (float)(this.CanvasRect.Width - this.ViewRect.Width),
                    Y = (float)(this.CanvasRect.Height - this.ViewRect.Height)
                };
                vector2.X = Math.Max(vector.X, vector2.X);
                vector2.Y = Math.Max(vector.Y, vector2.Y);
                if (this.IsTracking)
                {
                    this.ViewOrigin.X = ScrollTracker.SoftClamp(this.UnclampedViewOrigin.X, vector.X, vector2.X);
                    this.ViewOrigin.Y = ScrollTracker.SoftClamp(this.UnclampedViewOrigin.Y, vector.Y, vector2.Y);
                }
                else
                {
                    this.ApplyVelocity(dt, ref this.ViewOrigin.X, ref this.Velocity.X, vector.X, vector2.X);
                    this.ApplyVelocity(dt, ref this.ViewOrigin.Y, ref this.Velocity.Y, vector.Y, vector2.Y);
                }
                this.ViewRect.X = (int)this.ViewOrigin.X;
                this.ViewRect.Y = (int)this.ViewOrigin.Y;
            }
            public void HandleInput(InputHelper input)
            {
                //if (!this.IsTracking)
                //{
                //    for (int i = 0; i < input.TouchState.Count; i++)
                //    {
                //        if (input.TouchState[i].State == TouchLocationState.Pressed)
                //        {
                //            this.Velocity = Vector2.Zero;
                //            this.UnclampedViewOrigin = this.ViewOrigin;
                //            this.IsTracking = true;
                //            break;
                //        }
                //    }
                //}
                //using (List<GestureSample>.Enumerator enumerator = input.Gestures.GetEnumerator())
                //{
                //    while (enumerator.MoveNext())
                //    {
                //        GestureSample current = enumerator.Current;
                //        GestureType gestureType = current.GestureType;
                //        if (gestureType <= GestureType.VerticalDrag)
                //        {
                //            if (gestureType != GestureType.Tap)
                //            {
                //                if (gestureType == GestureType.VerticalDrag)
                //                {
                //                    this.UnclampedViewOrigin.Y = this.UnclampedViewOrigin.Y - current.Delta.Y;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (gestureType != GestureType.Flick)
                //            {
                //                if (gestureType == GestureType.DragComplete)
                //                {
                //                    this.IsTracking = false;
                //                }
                //            }
                //            else
                //            {
                //                if (Math.Abs(current.Delta.X) < Math.Abs(current.Delta.Y))
                //                {
                //                    this.IsTracking = false;
                //                    this.Velocity = -current.Delta;
                //                }
                //            }
                //        }
                //    }
                //}
            }
            private static float SoftClamp(float x, float min, float max)
            {
                float result;
                if (x < min)
                {
                    result = Math.Max(x - min, -400f) * 133.333328f / 400f + min;
                }
                else
                {
                    if (x > max)
                    {
                        result = Math.Min(x - max, 400f) * 133.333328f / 400f + max;
                    }
                    else
                    {
                        result = x;
                    }
                }
                return result;
            }
            private void ApplyVelocity(float dt, ref float x, ref float v, float min, float max)
            {
                float num = x;
                x += v * dt;
                v = MathHelper.Clamp(v, -2000f, 2000f);
                v = Math.Max(Math.Abs(v) - dt * 500f, 0f) * (float)Math.Sign(v);
                if (x < min)
                {
                    x = Math.Min(x + (min - x) * 0.1f + 2f, min);
                    v = 0f;
                }
                if (x > max)
                {
                    x = Math.Max(x - (x - max) * 0.1f - 2f, max);
                    v = 0f;
                }
            }
        }
    }

