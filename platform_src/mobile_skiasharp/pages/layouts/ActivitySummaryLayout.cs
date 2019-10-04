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

    public class ActivitySummaryLayout: PageLayout
    {

        protected override void LayoutComponentWide(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            base.LayoutComponentWide(grid, t, v, CurrentOption, OptionCount);
        }

        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Message:
                    grid.Children.Add(v, 0, 8, 1, 10);
                    break;
                default:
                    base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
                    break;
            }
        }
    }    
}
