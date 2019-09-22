using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SkiaSharp;

// To hot to think about doing this in the right way.
namespace FlorineSkiaSharpForms
{
    public class AvatarCustomizer : ContentPage
    {
        private List<PlayerAvatar.AvatarElement> editables = new List<PlayerAvatar.AvatarElement>()
            {
               PlayerAvatar.AvatarElement.Wings,
               PlayerAvatar.AvatarElement.Skin,
               PlayerAvatar.AvatarElement.Pants,
               PlayerAvatar.AvatarElement.Shirt,
              // PlayerAvatar.AvatarElement.Shoes,
               PlayerAvatar.AvatarElement.Hair,
            };

        private class LinkedCanvasView<T> : SkiaSharp.Views.Forms.SKCanvasView
        {
            public T Link;
            public LinkedCanvasView(T value) : base()
            {
                Link = value;
            }
        }

        Grid optionGrid = new Grid();
        public AvatarCustomizer()
        {
            this.SizeChanged += AvatarCustomizer_SizeChanged;


            SkiaSharp.Views.Forms.SKCanvasView cb = new SkiaSharp.Views.Forms.SKCanvasView();
            cb.VerticalOptions = LayoutOptions.FillAndExpand;
            cb.HorizontalOptions = LayoutOptions.FillAndExpand;
            cb.PaintSurface += Cb_PaintSurface;
            TapGestureRecognizer tgr = new TapGestureRecognizer();
            tgr.Tapped += Tgr_Tapped;
            cb.GestureRecognizers.Add(tgr);
            _displays["avatar"] = cb;

            TapGestureRecognizer thair = new TapGestureRecognizer();
            thair.Tapped += Thair_Tapped;
            foreach (PlayerAvatar.AvatarElement ae in editables)
            {
                SkiaSharp.Views.Forms.SKCanvasView hair = new LinkedCanvasView<PlayerAvatar.AvatarElement>(ae);
                hair.VerticalOptions = LayoutOptions.FillAndExpand;
                hair.HorizontalOptions = LayoutOptions.FillAndExpand;
                hair.PaintSurface += Hair_PaintSurface;
                
                hair.GestureRecognizers.Add(thair);
                _displays[ae.ToString()] = hair;
            }

            for (int i = 0; i < 4; ++i)
            {
                optionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                optionGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
        }
        
        Dictionary<string, SkiaSharp.Views.Forms.SKCanvasView> _displays = new Dictionary<string, SkiaSharp.Views.Forms.SKCanvasView>();
        private void MakeGrid()
        {
            var grid = new Grid();
            Grid subgrid = new Grid();
            


            int row = 0;
            int col = 0;
            if (Height > Width)
            {
                //Portrait
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                row = 1;

                foreach(PlayerAvatar.AvatarElement ae in editables)
                {
                    subgrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                subgrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            else
            {
                // Landscape
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
                col = 1;


                foreach (PlayerAvatar.AvatarElement ae in editables)
                {
                    subgrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }
                subgrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                
            }

            
            subgrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for(int i = 0; i < editables.Count; ++i)
            {
                subgrid.Children.Add(_displays[editables[i].ToString()], i * row, i * col);
            }
            grid.Children.Add(_displays["avatar"], 1 * col, 1 * row);
            grid.Children.Add(subgrid, 2 * col, 2 * row);
            grid.Children.Add(optionGrid, 3 * col, 3 * row);
            //grid.Children.Add(_displays["hairB"], 1 * col, 1 * row+1);
            //grid.Children.Add(_displays["hairR"], 1 * col, 1 * row);
            this.Content = grid;
        }

        private void InvalidateAll()
        {
            foreach(SkiaSharp.Views.Forms.SKCanvasView view in _displays.Values)
            {
                view.InvalidateSurface();
            }
        }
        private void Thair_Tapped(object sender, EventArgs e)
        {
            LinkedCanvasView<PlayerAvatar.AvatarElement> obj = (LinkedCanvasView<PlayerAvatar.AvatarElement>)sender;
            TapGestureRecognizer tcol = new TapGestureRecognizer();
            tcol.Tapped += Tcol_Tapped;
            List<SKColor> ColorList = new List<SKColor>()
            {
                new SKColor(0,0,0),
                new SKColor(125,0,0),
                new SKColor(0,125,0),
                new SKColor(0,0,125),

                new SKColor(40,40,40),
                new SKColor(125,40,40),
                new SKColor(40,125,40),
                new SKColor(40,40,125),

                new SKColor(80,80,80),
                new SKColor(125,80,80),
                new SKColor(80,125,80),
                new SKColor(80,80,125),

                new SKColor(125,125,125),
                new SKColor(125,0,125),
                new SKColor(125,125,0),
                new SKColor(0,125,125)
            };

            int i = 0;
            optionGrid.Children.Clear();
            foreach (SKColor color in ColorList)
            {
                int col = i % 4;
                int row = (int)(i / 4);
                SkiaSharp.Views.Forms.SKCanvasView coloritem = new LinkedCanvasView<Tuple<PlayerAvatar.AvatarElement, SKColor>>(
                    new Tuple<PlayerAvatar.AvatarElement, SKColor>(obj.Link, color));
                coloritem.VerticalOptions = LayoutOptions.FillAndExpand;
                coloritem.HorizontalOptions = LayoutOptions.FillAndExpand;
                coloritem.PaintSurface += Coloritem_PaintSurface;

                coloritem.GestureRecognizers.Add(tcol);
                optionGrid.Children.Add(coloritem, col, row);
                ++i;
            }
            //PlayerAvatar.Avatar.ColorScheme[obj.Link] = new SKColor(125, 0, 0);
            //InvalidateAll();
        }

        private void Coloritem_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            LinkedCanvasView<Tuple<PlayerAvatar.AvatarElement, SKColor>> obj = (LinkedCanvasView<Tuple<PlayerAvatar.AvatarElement, SKColor>>)sender;
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(new SKColor(0, 0, 0, 0));

            SKRect outerBounds = new SKRect(info.Width * .15f, info.Height * .1f, info.Width * .85f, info.Height * .9f);
            PlayerAvatar.Avatar.DrawZoomElement(obj.Link.Item1,
                canvas,
                outerBounds,
                obj.Link.Item2,
                true
                );
        }

        private void Tcol_Tapped(object sender, EventArgs e)
        {
            LinkedCanvasView<Tuple<PlayerAvatar.AvatarElement, SKColor>> obj = (LinkedCanvasView<Tuple<PlayerAvatar.AvatarElement, SKColor>>)sender;
            PlayerAvatar.Avatar.ColorScheme[obj.Link.Item1] = obj.Link.Item2;
            _displays["avatar"].InvalidateSurface();
            //InvalidateAll();
        }

        private void Tgr_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new CodeButtonClickPage();
        }


        private void Hair_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            LinkedCanvasView<PlayerAvatar.AvatarElement> obj = (LinkedCanvasView<PlayerAvatar.AvatarElement>)sender;
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(new SKColor(0,0,0,0));

            SKRect outerBounds = new SKRect(info.Width * .15f, info.Height * .1f, info.Width * .85f, info.Height * .9f);
            PlayerAvatar.Avatar.DrawZoomElement(obj.Link,
                canvas,
                outerBounds,
                new SKColor(125, 0, 0),
                true
                );
        }

        private void Cb_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(new SKColor(160, 160, 160));

            SKRect outerBounds = new SKRect(info.Width * .15f, info.Height * .1f, info.Width * .85f, info.Height * .9f);


            PlayerAvatar.Avatar.Draw(canvas, outerBounds);
        }

        private void AvatarCustomizer_SizeChanged(object sender, EventArgs e)
        {
            MakeGrid();
        }
    }
}