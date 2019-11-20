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
        private class FlorineSkiaCVWrap : SKCanvasView
        {
            public IFlorineSkiaConnectable FlorineObj { get; set; }
            public FlorineSkiaCVWrap(IFlorineSkiaConnectable Conn) : base()
            {
                FlorineObj = Conn;
                FlorineObj.ConnectCanvasView(this);
            }
        }
        private SKCanvasView _Text(
            string Message
            )
        {
            return new FlorineSkiaCVWrap(new ImageText(Message));
        }
        
        /* ******************************************************** */
        View XBG = null;
        public override void PostLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {
            Player PC = GameController.CurrentState.Player;
            // Calories
            if (GameController.GetCurrentPage().SubType == GameState.PageSubType.Dinner)
            {
                grid.Children.Add(_Text("Calories"), 1, 10, 4, 6);
                View CalorieView = ImageGradient.AsView(
                     0,
                     PC.TargetCalories,
                     PC.TargetCalories * 2,
                     PC.Calories,
                     true,
                     false
                );

                grid.Children.Add(CalorieView, 10, 29, 4, 6);

                SortedDictionary<float, SKColor> MicroNutrients = new SortedDictionary<float, SKColor>();
                SortedDictionary<float, SKColor> MicroPotential = new SortedDictionary<float, SKColor>();
                SortedDictionary<float, SKColor> MacroNutrients = new SortedDictionary<float, SKColor>();
                float microNut = 0f;
                float microPot = 0f;
                float macroNut = 0f;
                foreach (KeyValuePair<Nutrient, NutrientAmount> kvp in PC.Nutrients)
                {
                    FlorineSkiaNutrient AdjNut = new FlorineSkiaNutrient(kvp.Key);
                    float curRatio = kvp.Key.RatioRDV(kvp.Value);

                    switch (kvp.Key.Class)
                    {
                        case Nutrient.NutrientType.Macro:
                            if (curRatio > 2f) { curRatio = 2f; }
                            if (curRatio <= 0f) { continue; }
                            curRatio /= 8f;
                            macroNut += curRatio;
                            MacroNutrients.Add(macroNut, AdjNut.RingColor);
                            break;
                        case Nutrient.NutrientType.Mineral:
                        case Nutrient.NutrientType.Vitamin:
                            if (curRatio > 1f) { curRatio = 1f; }
                            float fRestRatio = float.NaN;
                            if (curRatio < 1f)
                            {
                                fRestRatio = 1f - curRatio;
                                fRestRatio /= 7f;
                            }
                            curRatio /= 7f;
                            microPot += 1f / 7f;

                            if (curRatio > float.Epsilon)
                            {
                                microNut += curRatio;
                                MicroNutrients.Add(microNut, AdjNut.RingColor);
                            }
                            if (!float.IsNaN(fRestRatio))
                            {
                                microNut += fRestRatio;
                                MicroNutrients.Add(microNut, SKColors.Transparent);
                            }
                            SKColor newCol = new SKColor(
                                AdjNut.RingColor.Red,
                                AdjNut.RingColor.Green,
                                AdjNut.RingColor.Blue,
                                80
                                );
                            MicroPotential.Add(microPot, newCol);
                            break;
                    }
                }
                grid.Children.Add(_Text("Vitamins"), 1, 10, 6, 8);
                //grid.Children.Add(ImageGradient.AsDivBar(WhiteBar), 20, 29, 6, 8);
                grid.Children.Add(ImageGradient.AsDivBar(MicroPotential), 10, 29, 6, 8);
                float gridSize = 5;
                grid.Children.Add(ImageGradient.AsDivBar(MicroPotential,
                    new SKPaint()
                    {
                        PathEffect = SKPathEffect.Create2DLine(1,
                            MatrixMultiply(SKMatrix.MakeScale(gridSize, gridSize),
                            SKMatrix.MakeRotationDegrees(45))),
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 1,
                        Color = new SKColor(0, 0, 0, 20)
                    }), 10, 29, 6, 8);

                grid.Children.Add(ImageGradient.AsDivBar(MicroPotential,
                    new SKPaint()
                    {
                        PathEffect = SKPathEffect.Create2DLine(1,
                            MatrixMultiply(SKMatrix.MakeScale(gridSize, gridSize),
                            SKMatrix.MakeRotationDegrees(-45))),
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 1,
                        Color = new SKColor(0, 0, 0, 20)
                    }), 10, 29, 6, 8);
                grid.Children.Add(ImageGradient.AsDivBar(MicroNutrients), 10, 29, 6, 8);

                grid.Children.Add(_Text("Nutrients"), 1, 10, 8, 10);
                grid.Children.Add(ImageGradient.AsDivBar(MacroNutrients), 10, 29, 8, 10);
            }

            if (GameController.GetCurrentPage().SubType == GameState.PageSubType.Lunch)
            {
                IGameOptionSet iActivities = GameController.GetCurrentPage().AppliedOptions;
                int Amount = 0;
                foreach (IGameOption act in iActivities)
                {
                    Activity mainAct = act as Activity;
                    if (null != mainAct)
                    {
                        Amount += mainAct.Pay;
                    }
                    if (null != act.SubOptions)
                    {
                        foreach (IGameOption subopt in act.SubOptions)
                        {
                            Activity subAct = subopt as Activity;
                            if (subAct != null)
                            {
                                Amount += subAct.Pay;
                            }
                        }
                    }
                }

                if (Amount != 0)
                {
                    View moneyGrid = MoneyView.RenderView(Amount);
                    grid.Children.Add(moneyGrid, 10, 20, 15, 18);
                }
            }
            else if (GameController.GetCurrentPage().SubType == GameState.PageSubType.Dinner)
            {
                View BackView = new FlorineSkiaCVWrap(new FlOval()
                {
                    backgroundColor = new SKPaint() { Color = new SKColor(0, 80, 190, 230) },
                    Shape = FlOval.OvalType.Rectangle,
                    ovalRatio = float.NaN,
                    innerHighlight = new SKColor(100, 250, 250, 255),
                });
                View moneyGrid = MoneyView.RenderView(PC.Money, false);                
                View HappinessGrid = Happiness.RenderView(PC.Happiness, false);
                View TotalText = new FlorineSkiaCVWrap(new ImageText("Total") { FontSize = 48f, Overflow = ImageText.WrapType.None });
                

                View tdBackView = new FlorineSkiaCVWrap(new FlOval()
                {
                    backgroundColor = new SKPaint() { Color = new SKColor(0, 80, 190, 230) },
                    Shape = FlOval.OvalType.Rectangle,
                    ovalRatio = float.NaN,
                    innerHighlight = new SKColor(100, 250, 250, 255),
                });
                View tdmoneyGrid = MoneyView.RenderView(PC.MoneyToDate, false);
                View tdHappinessGrid = Happiness.RenderView(PC.HappinessToDate, false);
                View tdTotalText = new FlorineSkiaCVWrap(new ImageText("Today") { FontSize = 48f, Overflow = ImageText.WrapType.None });

                int TotalY = 13;
                int TodayY = 10;
                //grid.Children.Add(BackView, 0, 30, 10, 14);
                grid.Children.Add(TotalText, 2, 9,       TotalY + 1, TotalY + 3);
                grid.Children.Add(moneyGrid, 8, 17, TotalY, TotalY +3 );
                grid.Children.Add(HappinessGrid, 17, 28, TotalY, TotalY + 3);

                //grid.Children.Add(tdBackView, 0, 30, 14, 18);
                grid.Children.Add(tdTotalText, 2, 9, TodayY+1, TodayY+3);
                grid.Children.Add(tdmoneyGrid, 8, 17, TodayY, TodayY + 3);
                grid.Children.Add(tdHappinessGrid, 17, 28, TodayY, TodayY + 3);


            }

            int EnergyY = 20;
            int FocusY = EnergyY + 3;
            grid.Children.Add(_Text("Energy"), 15, 20, EnergyY, EnergyY+2);
            grid.Children.Add(
                ImageGradient.AsView(
                    0.0,
                    100.0,
                    100.0,
                    PC.Energy,
                    false,
                    true
                 ),
                20, 29, EnergyY, EnergyY + 2
            );

            grid.Children.Add(_Text("Focus"), 15, 20, FocusY, FocusY+2);
            grid.Children.Add(
                ImageGradient.AsView(
                    0.0,
                    100.0,
                    100.0,
                    PC.Focus,
                    false,
                    true
                 ),
                20, 29, FocusY, FocusY + 2
            );
            
            base.PostLayout(IsTall, grid, GameController, GameFoundry, SourcePage);
        }
        static SKMatrix MatrixMultiply(SKMatrix first, SKMatrix second)
        {
            SKMatrix target = SKMatrix.MakeIdentity();
            SKMatrix.Concat(ref target, first, second);
            return target;
        }


        public override void PreLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {
            if (SourcePage.SubType == GameState.PageSubType.Dinner)
            {
                SKCanvasView Back = new SKCanvasView();
                Back.PaintSurface += Back_PaintSurface;
                XBG = Back;
            }
            
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
                case PageComponentType.Background:
                    if (null == XBG)
                    {
                        XBG = v;
                    }
                    
                    
                        grid.Children.Add(
                                XBG,
                                0,
                                grid.ColumnDefinitions.Count,
                                0,
                                grid.RowDefinitions.Count);
                    
                    break;
                case PageComponentType.Message:
                    grid.Children.Add(v, 00, 30, 4, 16);
                    break;
                default:
                    base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
                    break;
            }
        }

        /* ************************************************************************************************** */

    }    
}
