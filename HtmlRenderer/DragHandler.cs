using System;
using System.Collections.Generic;
using System.Drawing;
using CefSharp;
using CefSharp.Enums;

namespace RainbowMage.HtmlRenderer
{
    class DragHandler : IDragHandler
    {
        OverlayForm form;

        public DragHandler(OverlayForm form)
        {
            this.form = form;
        }

        public bool OnDragEnter(IWebBrowser chromiumWebBrowser, IBrowser browser, IDragData dragData, DragOperationsMask mask)
        {
            return false;
        }

        public void OnDraggableRegionsChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IList<DraggableRegion> regions)
        {
            if (!frame.IsMain) return;

            var region = new Region();
            region.MakeEmpty();

            foreach (var sub in regions)
            {
                var rect = new Rectangle(sub.X, sub.Y, sub.Width, sub.Height);

                if (sub.Draggable)
                {
                    region.Union(rect);
                } else
                {
                    region.Exclude(rect);
                }
            }

            form.DraggableRegion = region;
        }
    }
}
