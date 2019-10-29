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
        Player ActualPlayerBase;
        PlayerAvatar ActualPlayer;
        
        SKCanvasView PlayerView;
        Dictionary<SKCanvasView, SKColor> ColSel = new Dictionary<SKCanvasView, SKColor>();
        Dictionary<SKCanvasView, AspectImage> _rmI = new Dictionary<SKCanvasView, AspectImage>();
        Dictionary<SKCanvasView, string> _rmT = new Dictionary<SKCanvasView, string>();
        private void SetColor (object sender, FlorineSkiaTapWrap.TapEventArgs e)
        {
            FlorineSkiaTapWrap fstw = sender as FlorineSkiaTapWrap;
            if (null != fstw && null != PlayerView && null != ActualPlayer)
            {
                SKCanvasView scv = fstw.Tie;
                SKColor ai;                
                if (ColSel.TryGetValue(scv, out ai))
                {
                    float fR = ((float)ai.Red) / 255f;
                    float fG = ((float)ai.Green) / 255f;
                    float fB = ((float)ai.Blue) / 255f;
                    SKPaint newPaint = new SKPaint()
                    {
                        /*
                        ColorFilter =
                        SKColorFilter.CreateColorMatrix(new float[]
                        {
                            0.21f, 0.72f, 0.07f, 0, 0,
                            0.21f, 0.72f, 0.07f, 0, 0,
                            0.21f, 0.72f, 0.07f, 0, 0,
                            0,     0,     0,     1, 0
                        }),                      
                        */                        
                        ColorFilter =
                        SKColorFilter.CreateColorMatrix(new float[]
                        {
                            fR*0.21f, fR*0.72f, fR*0.07f, 0, 0,
                            fG*0.21f, fG*0.72f, fG*0.07f, 0, 0,
                            fB*0.21f, fB*0.72f, fB*0.07f, 0, 0,
                            0,     0,     0,     1, 0
                        })
                    };
                    switch (_rmT[scv])
                    {                        
                        case "hair": ActualPlayer.HairPaint = newPaint; break;
                        case "wings": ActualPlayer.WingPaint = newPaint; break;
                        case "body": ActualPlayer.BodyPaint = newPaint; break;
                    }
                    PlayerView.InvalidateSurface();
                }
            }
        }
        private View ColorSelector(string type, SKColor[] Targets)
        {
            Grid subGrid = new Grid();
            subGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i < Targets.Count(); ++i)
            {
                subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            int idx = 0;
            foreach (SKColor Target in Targets)
            {                
                /*
                FlOval ai = new FlOval()
                {
                    Shape = FlOval.OvalType.Rectangle,
                    backgroundColor = new SKPaint() { Color = Target },
                };
                */
                SKCanvasView canvas = new SKCanvasView();
                canvas.PaintSurface += ColorBoxFill;
                //ai.ConnectCanvasView(canvas);
                ColOpy[type].Add(canvas);
                ColSel[canvas] = Target;
                _rmT[canvas] = type;
                FlorineSkiaTapWrap.Associate(canvas, SetColor);
                subGrid.Children.Add(canvas, idx, 0);
                ++idx;
            }
            return subGrid;
        }

        private void ColorBoxFill(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvasView scv = sender as SKCanvasView;
            if (null == scv) { return; }
            SKColor ai;
            if (ColSel.TryGetValue(scv, out ai))
            {
                e.Surface.Canvas.Clear(ai);
            }
        }

        private string ActiveBodyType = "Box";
        private Dictionary<string, Grid> gridList = new Dictionary<string, Grid>();
        private Dictionary<string, List<SKCanvasView>> Opy = new Dictionary<string, List<SKCanvasView>>()
        {
            { "hair", new List<SKCanvasView>() },
            { "face", new List<SKCanvasView>() },
            { "clothes", new List<SKCanvasView>() },
            { "wings", new List<SKCanvasView>() },
            { "body", new List<SKCanvasView>() },
            { "misc", new List<SKCanvasView>() },
        };
        private Dictionary<string, List<SKCanvasView>> ColOpy = new Dictionary<string, List<SKCanvasView>>()
        {
            { "hair", new List<SKCanvasView>() },
            { "face", new List<SKCanvasView>() },
            { "clothes", new List<SKCanvasView>() },
            { "wings", new List<SKCanvasView>() },
            { "body", new List<SKCanvasView>() },
            { "misc", new List<SKCanvasView>() },
        };
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

                SKCanvasView Left = LeftArrow(200, 0, 0, 200, 4);
                SKCanvasView Right = RightArrow(200, 0, 0, 200, 4);
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

        private SKCanvasView LeftArrow(byte r, byte g, byte b, byte a, float Ratio)
        {
            SKCanvasView v = new SKCanvasView();
            v.PaintSurface += V_PaintSurface_Left;            
            return v;
        }

        private void V_PaintSurface_Left(object sender, SKPaintSurfaceEventArgs e)
        {            
            int MidY = e.Info.Rect.MidY;
            int iModWid = (int)(e.Info.Rect.Width * .35);
            int X0 = e.Info.Rect.Left + iModWid;
            int X1 = e.Info.Rect.Right - iModWid;
            int iModHeight = (int)(e.Info.Rect.Height * .35);
            int Y0 = e.Info.Rect.Top + iModHeight;
            int Y1 = e.Info.Rect.Bottom - iModHeight;  
            SKPoint P0 = new SKPoint(X1, Y0);
            SKPoint P1 = new SKPoint(X0, MidY);
            SKPoint P2 = new SKPoint(X1,Y1);
            SKPaint paint = new SKPaint()
            {
                Color = SKColors.Black,
                IsStroke = true,
                StrokeWidth = 5f
            };
            e.Surface.Canvas.DrawLine(P0, P1, paint);
            e.Surface.Canvas.DrawLine(P1, P2, paint);            
        }

        private SKCanvasView RightArrow(byte r, byte g, byte b, byte a, float Ratio)
        {
            SKCanvasView v = new SKCanvasView();
            v.PaintSurface += V_PaintSurface_Right;
            return v;
        }
        private void V_PaintSurface_Right(object sender, SKPaintSurfaceEventArgs e)
        {            
            int MidY = e.Info.Rect.MidY;
            int iModWid = (int)(e.Info.Rect.Width * .35);
            int X0 = e.Info.Rect.Left + iModWid;
            int X1 = e.Info.Rect.Right - iModWid;
            int iModHeight = (int)(e.Info.Rect.Height * .35);
            int Y0 = e.Info.Rect.Top + iModHeight;
            int Y1 = e.Info.Rect.Bottom - iModHeight;
            SKPoint P0 = new SKPoint(X0, Y0);
            SKPoint P1 = new SKPoint(X1, MidY);
            SKPoint P2 = new SKPoint(X0, Y1);
            SKPaint paint = new SKPaint()
            {
                Color = SKColors.Black,
                IsStroke = true,
                StrokeWidth = 5f
            };
            e.Surface.Canvas.DrawLine(P0, P1, paint);
            e.Surface.Canvas.DrawLine(P1, P2, paint);
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
            ActualPlayerBase = GameController.CurrentState.Player;
            ActualPlayer = GameController.CurrentState.Player.Avatar.Picture as PlayerAvatar;
            Entry NameEntry = new Entry()
            {
                Text = "Faerina",
                HorizontalTextAlignment = TextAlignment.Center,
//                FontSize = 8f,
                VerticalOptions = new LayoutOptions()
                {
                    Alignment = LayoutAlignment.End
                },
                
                
            };
            
            NameEntry.TextChanged += NameEntry_TextChanged;
            grid.Children.Add(NameEntry, 0, 30, 0, 4);

            grid.Children.Add(Oval(0, 0, 200, 120,  2f),  0, 10, 2, 15);
            grid.Children.Add(Oval(0, 0, 200, 120,  2f), 10, 20, 2, 15);
            grid.Children.Add(Oval(0, 0, 200, 120,  2f), 20, 30, 2, 15);
            grid.Children.Add(ColorSelector("body", new SKColor[] {
                new SKColor(253,196,179), // 1
                new SKColor(245,185,158), // 2
                new SKColor(228,131,86),  // 3
                new SKColor(217,118,76),  // 4          
                new SKColor(182,88,34),   // 5             
                new SKColor(143,70,29),   // 6
                new SKColor(113,50,20),   // 6
            }), 4, 26, 14, 16);
            
            grid.Children.Add(Oval(0, 0, 200, 120, 1f), 0, 10, 16, 22);
            grid.Children.Add(Oval(0, 0, 200, 120, 1f), 10, 20, 16, 22);
            grid.Children.Add(Oval(0, 0, 200, 120, 1f), 20, 30, 16, 22);
            
            grid.Children.Add(SetupSelector("clothes"), 0, 10, 16, 22);
            grid.Children.Add(SetupSelector("wings"), 10, 20, 16, 22);
            grid.Children.Add(SetupSelector("hair"), 20, 30, 16, 22);
            grid.Children.Add(ColorSelector("hair", new SKColor[] {
                new SKColor(202,191,177),
                new SKColor(255,240,225),
                new SKColor(230,206,168),
                new SKColor(222,188,153),
                new SKColor(181,82,57)
            }), 21, 29, 22, 23);
            grid.Children.Add(ColorSelector("hair", new SKColor[] {
                new SKColor(143,70,29),
                new SKColor(106,78,66),                
                new SKColor(83,61,53),
                new SKColor(9,8,6),
                new SKColor(145,75,67)
            }), 21, 29, 23, 24);
            grid.Children.Add(PlayerView, 11, 19, 3, 14);            
        }

        private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualPlayerBase.Name = e.NewTextValue;            
        }

        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Title:
                case PageComponentType.Message:
                    //grid.Children.Add(v, 0, 30, 1, 3);
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
