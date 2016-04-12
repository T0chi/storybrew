﻿using OpenTK;
using OpenTK.Input;
using System;

namespace StorybrewEditor.UserInterface
{
    public class ScrollArea : Widget
    {
        private StackLayout scrollContainer;
        private bool dragged;

        public ScrollArea(WidgetManager manager, Widget scrollable) : base(manager)
        {
            ClipChildren = true;
            Add(scrollContainer = new StackLayout(manager)
            {
                FitChildren = true,
                Children = new Widget[]
                {
                    scrollable
                },
            });
            OnClickDown += (sender, e) =>
            {
                if (e.Button != MouseButton.Left) return false;
                dragged = true;
                return true;
            };
            OnClickUp += (sender, e) =>
            {
                if (e.Button != MouseButton.Left) return;
                dragged = false;
            };
            OnDrag += (sender, e) =>
            {
                if (!dragged) return;
                scroll(e.YDelta);
            };
            OnMouseWheel += (sender, e) =>
            {
                scroll(e.DeltaPrecise * 64);
                return true;
            };
        }

        private void scroll(float amount)
        {
            var scrollableDistance = Math.Max(0, scrollContainer.Height - Height);
            scrollContainer.Offset = new Vector2(0, Math.Max(-scrollableDistance, Math.Min(scrollContainer.Offset.Y + amount, 0)));
        }

        protected override void Layout()
        {
            base.Layout();
            scrollContainer.Size = new Vector2(Size.X, scrollContainer.PreferredSize.Y);
            scroll(0);
        }
    }
}