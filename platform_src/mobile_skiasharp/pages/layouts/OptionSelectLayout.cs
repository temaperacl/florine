using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florine;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public class LayoutOptionSelect : PageLayout
    {
        SKCanvasView Description;
        public override void LayoutComponent(Grid grid, PageComponent p, int CurrentOption, int OptionCount, bool isTall)
        {
            if (p.PCType == PageComponentType.Description)
            {
                Description = p.PCView as SKCanvasView;
            }
            else
            {
                base.LayoutComponent(grid, p, CurrentOption, OptionCount, isTall);
            }
        }

        public override void PostLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {
            if (null != Description)
            {
                grid.Children.Add(Description, 0*12, 29, 16, 28);
            }
            base.PostLayout(IsTall, grid, GameController, GameFoundry, SourcePage);
        }
    }
}
