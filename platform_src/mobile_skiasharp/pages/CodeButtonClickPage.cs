using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;

using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public class CodeButtonClickPage : ContentPage
    {
        /*
        public class ToggleableImage : SkiaSharp.Views.Forms.SKCanvasView
        {            
            public static int MaxSelect = 3;
            public static List<Food> SelectedItems
            {
                get
                {
                    List<Food> x = new List<Food>();
                    foreach(ToggleableImage ti in _all.Where(item => item.Selected))
                    {
                        x.Add(ti.Value);
                    }
                    return x;
                }
            }
            private static List<ToggleableImage> _all = new List<ToggleableImage>();
            public Food Value;
            public bool Selected = false;
            public ToggleableImage()
            {
                _all.Add(this);
                this.PaintSurface += this.AutoPaint;
            }
            
            public void AutoPaint(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
            {
                if (Value != null)
                {
                    args.Surface.Canvas.Clear();
                    Value.OvalDisplay.AutoPaint(sender, args);
                }
            }
            public void Tgr_Tapped(object sender, EventArgs e)
            {
                
                if (this.Selected || ToggleableImage.SelectedItems.Count < ToggleableImage.MaxSelect)
                {
                    this.Selected = !this.Selected;
                    Value.OvalDisplay.backgroundColor.Color = new SKColor(
                        Value.OvalDisplay.backgroundColor.Color.Red,
                        Value.OvalDisplay.backgroundColor.Color.Green,
                        Value.OvalDisplay.backgroundColor.Color.Blue,
                        (byte)(this.Selected ? 255 : 0));
                    this.InvalidateSurface();
                }
            }
        }
        public CodeButtonClickPage()
        {
            this.Disappearing += CodeButtonClickPage_Disappearing;

            SkiaSharp.Views.Forms.SKCanvasView bv = new SkiaSharp.Views.Forms.SKCanvasView();
            bv.VerticalOptions = LayoutOptions.FillAndExpand;
            bv.HorizontalOptions = LayoutOptions.FillAndExpand;
            //bv.
            bv.PaintSurface += new FlOval() { backgroundColor = new SKPaint() { Color = new SKColor(125, 0, 0) } }.AutoPaint;
            TapGestureRecognizer ttr = new TapGestureRecognizer();
            ttr.Tapped += Tgr_Tapped1;
            bv.GestureRecognizers.Add(ttr);
            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            
            int foodIdx = 0;
            List<Food> foods = Food.Discover(12);
            for (int row = 1; row <= 3; ++row)
            {
                for (int col = 0; col <= 3; col += 2)
                {
                    if (foodIdx >= 0 && foodIdx < foods.Count)
                    {
                        ToggleableImage ib = new ToggleableImage()
                        {
                            Value = foods[foodIdx]
                        };
                        ib.VerticalOptions = LayoutOptions.FillAndExpand;
                        ib.HorizontalOptions = LayoutOptions.FillAndExpand;
                        TapGestureRecognizer tgr = new TapGestureRecognizer();
                        tgr.Tapped += ib.Tgr_Tapped;
                        ib.GestureRecognizers.Add(tgr);

                        grid.Children.Add(ib, col, row);
                        Grid.SetColumnSpan(ib, 2);

                        foodIdx++;
                    }
                    if(foodIdx >= foods.Count) { foodIdx = -1; }
                }
            }
            SkiaSharp.Views.Forms.SKCanvasView cb = new SkiaSharp.Views.Forms.SKCanvasView();
            cb.VerticalOptions = LayoutOptions.FillAndExpand;
            cb.HorizontalOptions = LayoutOptions.FillAndExpand;
            cb.PaintSurface += Cb_PaintSurface;
            grid.Children.Add(cb, 0, 0);
            grid.Children.Add(bv, 1, 4);

            Grid.SetColumnSpan(cb, 4);
            Grid.SetColumnSpan(bv, 2);

            Content = grid;
        }

        private void Tgr_Tapped1(object sender, EventArgs e)
        {
            MealReview mr = new MealReview(ToggleableImage.SelectedItems);
            Application.Current.MainPage = mr;
        }

        private void Cb_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(new SKColor(160,160,160));

            SKRect outerBounds = new SKRect(info.Width * .15f, info.Height * .1f, info.Width * .85f, info.Height * .9f);

            
            PlayerAvatar.Avatar.Draw(canvas, outerBounds);
        }

        private void CodeButtonClickPage_Disappearing(object sender, EventArgs e)
        {            
            //if (Navigation.NavigationStack.First() != Navigation.NavigationStack.Last())
           // {
            //    Navigation.RemovePage(this);
          //  }
        }

        private void Tgr_Tapped(object sender, EventArgs e)
        {
           // Navigation.PushAsync(new CodeButtonClickPage());
        }
 */
    }
}