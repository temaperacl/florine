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

    public class AvatarSelectionLayout : PageLayout
    {
        /*
        public override SKRectI GetResolution(Dictionary<PageComponentType, int> ComponentCounts, bool IsTall)
        {
            return new SKRectI(0, 0, 30, 30);
        }
        */
        private void PrevClothes(object sender, FlorineSkiaTapWrap.TapEventArgs e)
        {
            FlorineSkiaTapWrap fstw = sender as FlorineSkiaTapWrap;
            if (null != fstw)
            {
                string type = _rmT[fstw.Tie];
                SKCanvasView x = Opy[type][0];
                SKCanvasView y = Opy[type].Last();
                Opy[type].RemoveAt(Opy[type].Count - 1);
                Opy[type].Insert(0, y);
                gridList[type].Children.Remove(x);
                gridList[type].Children.Add(Opy[type][0], 1, 4, 0, 1);
            }
        }
        private void NextClothes(object sender, FlorineSkiaTapWrap.TapEventArgs e)
        {
            FlorineSkiaTapWrap fstw = sender as FlorineSkiaTapWrap;
            if (null != fstw)
            {
                string type = _rmT[fstw.Tie];
                SKCanvasView x = Opy[type][0];
                Opy[type].RemoveAt(0);
                Opy[type].Add(x);
                gridList[type].Children.Remove(x);
                gridList[type].Children.Add(Opy[type][0], 1, 4, 0, 1);
            }
        }

        
        private void SetComponentImage(object sender, FlorineSkiaTapWrap.TapEventArgs e)
        {
            FlorineSkiaTapWrap fstw = sender as FlorineSkiaTapWrap;
            if ( null != fstw && null != PlayerView  && null != ActualPlayer )
            {
                SKCanvasView scv = fstw.Tie;
                AspectImage ai;
                if (_rmI.TryGetValue(scv, out ai))
                {
                    switch (_rmT[scv]) {
                        case "clothes": ActualPlayer.Clothes = ai.baseImage; break;
                        case "hair": ActualPlayer.Hair = ai.baseImage; break;
                        case "wings": ActualPlayer.Wings = ai.baseImage; break;
                        case "face": ActualPlayer.Face = ai.baseImage; break;                        
                    }
                    
                    PlayerView.InvalidateSurface();
                }
            }
        }
        
        //public class ComponentSelector
        public override void PreLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {               
            base.PostLayout(IsTall, grid, GameController, GameFoundry, SourcePage);
        }
        PlayerAvatar ActualPlayer;
        SKCanvasView PlayerView;
        Dictionary<SKCanvasView, AspectImage> _rmI = new Dictionary<SKCanvasView, AspectImage>();
        Dictionary<SKCanvasView, string> _rmT = new Dictionary<SKCanvasView, string>();

        private string ActiveBodyType = "Box";
        private Dictionary<string, Grid> gridList = new Dictionary<string, Grid>();
        private Dictionary<string, List<SKCanvasView>> Opy = new Dictionary<string, List<SKCanvasView>>();
        private View SetupSelector(string type)
        {
            
            string Target = "";
            switch (type) {
                case "hair": Target = "01_hair/100%"; break;
                case "face": Target = "02_faces/100"; break;
                case "clothes": Target = "03_clothes/" + ActiveBodyType + "/100%"; break;
                case "wings": Target = "05_wings/100%"; break;
            }
            Dictionary<string, SKImage> clothes = ResourceLoader.ImageList("customization/" + Target);
            //int i = 0;
            Opy[type] = new List<SKCanvasView>();

            foreach (KeyValuePair<string, SKImage> kvp in clothes)
            {
                SKCanvasView canvas = new SKCanvasView();
                AspectImage ai = new AspectImage() { baseImage = kvp.Value };
                ai.ConnectCanvasView(canvas);
                _rmI[canvas] = ai;
                _rmT[canvas] = type;
                Opy[type].Add(canvas);

                //                 
                FlorineSkiaTapWrap.Associate(canvas, SetComponentImage);
                

                Grid subGrid = new Grid();
                subGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });                                
                subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                SKCanvasView Left = Oval(200, 0, 0, 200, 4);
                SKCanvasView Right = Oval(200, 0, 0, 200, 4);
                _rmT[Left] = type;
                _rmT[Right] = type;
                FlorineSkiaTapWrap.Associate(Left, PrevClothes);
                FlorineSkiaTapWrap.Associate(Right, NextClothes);
                subGrid.Children.Add(Left, 0, 2, 0, 1);
                subGrid.Children.Add(Right, 3, 5, 0, 1);
                gridList[type] = subGrid;
                


                /*int Coverage = 5;
                int X = (i % 6) * Coverage;
                int Y = (i / 6) * Coverage + 17;
                grid.Children.Add(canvas, X, X + Coverage, Y, Y + Coverage);
                i++;*/
            }
            gridList[type].Children.Add(Opy[type][0], 1, 4, 0, 1);

            return gridList[type];
        }
        protected override void LayoutComponentWide(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            base.LayoutComponentWide(grid, t, v, CurrentOption, OptionCount);
        }

        private SKCanvasView Oval(byte r, byte g, byte b, byte a, float Ratio)
        {
            SKCanvasView v = new SKCanvasView();
            FlOval Background = new FlOval() {
                backgroundColor = new SKPaint() { Color = new SKColor(r, g, b, a) },
                ovalRatio = Ratio
            };
            Background.ConnectCanvasView(v);
            return v;
        }
        public override void PostLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {
            ActualPlayer = GameController.CurrentState.Player.Avatar.Picture as PlayerAvatar;
            grid.Children.Add(Oval(0, 0, 200, 120,  2f),  0, 10, 2, 15);
            grid.Children.Add(Oval(0, 0, 200, 120,  2f), 10, 20, 2, 15);
            grid.Children.Add(Oval(0, 0, 200, 120,  2f), 20, 30, 2, 15);

            grid.Children.Add(Oval(0, 0, 200, 120, 1f), 0, 10, 16, 22);
            grid.Children.Add(Oval(0, 0, 200, 120, 1f), 10, 20, 16, 22);
            grid.Children.Add(Oval(0, 0, 200, 120, 1f), 20, 30, 16, 22);

            grid.Children.Add(SetupSelector("clothes"), 0, 10, 16, 22);
            grid.Children.Add(SetupSelector("wings"), 10, 20, 16, 22);
            grid.Children.Add(SetupSelector("hair"), 20, 30, 16, 22);
            grid.Children.Add(PlayerView, 11, 19, 3, 14);
        }
        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Title:
                case PageComponentType.Message:
                    grid.Children.Add(v, 0, 30, 1, 3);
                    break;
                case PageComponentType.Player:
                    PlayerView = v as SKCanvasView;                    
                    break;
                case PageComponentType.Footer:
                    grid.Children.Add(v, 0, 30, 27, 30);
                    break;
                case PageComponentType.Background:
                    break;
                default:
                    base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
                    break;
            }
        }
    }    
}
