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

    public class MessageOnlyLayout : PageLayout
    {
        public override void PreLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {
            SKCanvasView Back = new SKCanvasView();
            Back.PaintSurface += Back_PaintSurface;
            grid.Children.Add(
                Back,
                0,
                grid.ColumnDefinitions.Count, 
                0,
                grid.RowDefinitions.Count);
            base.PreLayout(IsTall, grid, GameController, GameFoundry, SourcePage);
        }

        private void Back_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear(new SKColor(50, 50, 50));
        }

        protected override void LayoutComponentWide(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            base.LayoutComponentWide(grid, t, v, CurrentOption, OptionCount);
        }

        
        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Title:
                    grid.Children.Add(v, 0, 30, 0, 3);
                    break;
                case PageComponentType.Message:
                    grid.Children.Add(v, 1, 29, 3, 27);
                    break;
                case PageComponentType.Footer:
                    grid.Children.Add(v, 6, 24, 27, 30);
                    break;                
                default:                    
                    break;
            }
        }
        
    }    
}
