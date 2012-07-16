using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using JoyOS.System.Screen;

namespace JoyOS.System.Controls
{
    public class Control
    {
        private Vector2 position;
        private Vector2 size;
        private bool sizeValid = false;
        private bool autoSize = true;
        private List<Control> children = null;
        public bool Visible = true;
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
                if (this.Parent != null)
                {
                    this.Parent.InvalidateAutoSize();
                }
            }
        }
        public Vector2 Size
        {
            get
            {
                if (!this.sizeValid)
                {
                    this.size = this.ComputeSize();
                    this.sizeValid = true;
                }
                return this.size;
            }
            set
            {
                this.size = value;
                this.sizeValid = true;
                this.autoSize = false;
                if (this.Parent != null)
                {
                    this.Parent.InvalidateAutoSize();
                }
            }
        }
        public Control Parent
        {
            get;
            private set;
        }
        public int ChildCount
        {
            get
            {
                return (this.children == null) ? 0 : this.children.Count;
            }
        }
        public Control this[int childIndex]
        {
            get
            {
                return this.children[childIndex];
            }
        }
        protected void InvalidateAutoSize()
        {
            if (this.autoSize)
            {
                this.sizeValid = false;
                if (this.Parent != null)
                {
                    this.Parent.InvalidateAutoSize();
                }
            }
        }
        public void AddChild(Control child)
        {
            if (child.Parent != null)
            {
                child.Parent.RemoveChild(child);
            }
            this.AddChild(child, this.ChildCount);
        }
        public void AddChild(Control child, int index)
        {
            if (child.Parent != null)
            {
                child.Parent.RemoveChild(child);
            }
            child.Parent = this;
            if (this.children == null)
            {
                this.children = new List<Control>();
            }
            this.children.Insert(index, child);
            this.OnChildAdded(index, child);
        }
        public void RemoveChildAt(int index)
        {
            Control control = this.children[index];
            control.Parent = null;
            this.children.RemoveAt(index);
            this.OnChildRemoved(index, control);
        }
        public void RemoveChild(Control child)
        {
            if (child.Parent != this)
            {
                throw new InvalidOperationException();
            }
            this.RemoveChildAt(this.children.IndexOf(child));
        }
        public virtual void Draw(DrawContext context)
        {
            Vector2 drawOffset = context.DrawOffset;
            for (int i = 0; i < this.ChildCount; i++)
            {
                Control control = this.children[i];
                if (control.Visible)
                {
                    context.DrawOffset = drawOffset + control.Position;
                    control.Draw(context);
                }
            }
        }
        public virtual void Update(GameTime gametime)
        {
            for (int i = 0; i < this.ChildCount; i++)
            {
                this.children[i].Update(gametime);
            }
        }
        public virtual void HandleInput(InputHelper input)
        {
            for (int i = 0; i < this.ChildCount; i++)
            {
                this.children[i].HandleInput(input);
            }
        }
        public virtual Vector2 ComputeSize()
        {
            Vector2 result;
            if (this.children == null || this.children.Count == 0)
            {
                result = Vector2.Zero;
            }
            else
            {
                Vector2 vector = this.children[0].Position + this.children[0].Size;
                for (int i = 1; i < this.children.Count; i++)
                {
                    Vector2 vector2 = this.children[i].Position + this.children[i].Size;
                    vector.X = Math.Max(vector.X, vector2.X);
                    vector.Y = Math.Max(vector.Y, vector2.Y);
                }
                result = vector;
            }
            return result;
        }
        protected virtual void OnChildAdded(int index, Control child)
        {
            this.InvalidateAutoSize();
        }
        protected virtual void OnChildRemoved(int index, Control child)
        {
            this.InvalidateAutoSize();
        }
        public static void BatchDraw(Control control, GraphicsDevice device, SpriteBatch spriteBatch, Vector2 offset, GameTime gameTime)
        {
            if (control != null && control.Visible)
            {
                spriteBatch.Begin();
                control.Draw(new DrawContext
                {
                    Device = device,
                    SpriteBatch = spriteBatch,
                    DrawOffset = offset + control.Position,
                    GameTime = gameTime
                });
                spriteBatch.End();
            }
        }
    }
}


