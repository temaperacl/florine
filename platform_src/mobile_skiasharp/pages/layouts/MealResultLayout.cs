﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florine;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public class MealResultLayout : PageLayout
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
        private SKCanvasView _GradientBar(
            double Min,
            double Target,
            double Max,
            double Current,
            bool CanHaveTooMuch,
            bool MaskExcess
        )
        {
            ImageGradient IG = new ImageGradient();
            float Center = (float)((Target - Min) / (Max - Min));
            float CurPoint = (float)((Current - Min) / (Max - Min));
            if (CurPoint > 1) { CurPoint = 1; }
            if (CurPoint < 0) { CurPoint = 0; }

            if (CanHaveTooMuch)
            {

                IG.Details[0f] = new SKColor(250, 0, 0);
                IG.Details[.35f] = SKColors.Yellow;
                IG.Details[.5f] = new SKColor(0, 250, 0);
                IG.Details[.65f] = SKColors.Yellow;
                IG.Details[1f] = new SKColor(250, 0, 0);
                /*
                IG.Details[(float)Min] = new SKColor(250, 0, 0);
                IG.Details[(float)((Center - Min) / 2 + Min)] = SKColors.Yellow;
                IG.Details[Center] = new SKColor(0, 250, 0);
                IG.Details[(float)((Max - Center) / 2 + Center)] = SKColors.Yellow;
                IG.Details[(float)Max] = new SKColor(250, 0, 0);
                */
            }
            else
            {
                IG.Details[(float)Min] = new SKColor(250, 0, 0);
                //IG.Details[Center] = new SKColor(125, 125, 0);
                IG.Details[(float)Max] = new SKColor(0, 250, 0);
            }

            if (MaskExcess)
            {
                IG.BarWidth = CurPoint;
            }
            else
            {
                IG.IndicatorLineLoc = CurPoint;
            }
            IG.BorderSize = 5;

            return new FlorineSkiaCVWrap(IG);
            //return BarCanvas;
        }
        private SKCanvasView _DivBar(
            SortedDictionary<float, SKColor> Items            
        )
        {
            ImageGradient IG = new ImageGradient()
            {
                Style = ImageGradient.GradientType.RelativeSharp
            };
            foreach (KeyValuePair<float, SKColor> kvp in Items)
            {
                IG.Details.Add(kvp.Key, kvp.Value);                
            }            
            return new FlorineSkiaCVWrap(IG);
            //return BarCanvas;
        }

        // Allow for pre-component rendering layout
        public override void PreLayout(
            bool IsTall,
            Grid grid,
            Controller GameController,
            IPlatformFoundry GameFoundry,
            IPage SourcePage
        )
        {
            return;
        }

        //Allow for post-component rendering layout
        public override void PostLayout(
            bool IsTall,
            Grid grid,
            Controller GameController,
            IPlatformFoundry GameFoundry,
            IPage CurrentPage
        )
        {            
            Player PC = GameController.CurrentState.Player;

            if (IsTall)
            {
                // Calories
                grid.Children.Add(_Text("Calories"), 15, 20, 4, 6);
                View CalorieView = _GradientBar(
                     0,
                     PC.TargetCalories,
                     PC.TargetCalories * 2,
                     PC.Calories,
                     true,
                     false
                );
                
                grid.Children.Add(CalorieView, 20, 29, 4, 6);
                SortedDictionary<float, SKColor> WhiteBar = new SortedDictionary<float, SKColor>()
                {
                    { 0f, new SKColor(255,255,255,60)},
                    { 1f, new SKColor(255,255,255,60) },
                };
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
                grid.Children.Add(_Text("Vitamins"), 15, 20, 6, 8);


                grid.Children.Add(_DivBar(WhiteBar), 20, 29, 6, 8);
                grid.Children.Add(_DivBar(MicroPotential), 20, 29, 6, 8);
                grid.Children.Add(_DivBar(MicroNutrients), 20, 29, 6, 8);
                

                grid.Children.Add(_Text("Nutrients"), 15, 20, 8, 10);
                grid.Children.Add(_DivBar(MacroNutrients), 20, 29, 8, 10);

                int EnergyY = 20;
                int FocusY = EnergyY + 3;
                
                grid.Children.Add(_Text("Energy"), 15, 20, EnergyY, EnergyY + 2);
                grid.Children.Add(
                    _GradientBar(
                        0.0,
                        100.0,
                        100.0,
                        PC.Energy,
                        false,
                        true
                     ),
                    20, 29, EnergyY, EnergyY + 2
                );

                grid.Children.Add(_Text("Focus"), 15, 20, FocusY, FocusY + 2);
                grid.Children.Add(
                    _GradientBar(
                        0.0,
                        100.0,
                        100.0,
                        PC.Focus,
                        false,
                        true
                     ),
                    20, 29, FocusY, FocusY + 2
                );
            }
            base.PostLayout(IsTall, grid, GameController, GameFoundry, CurrentPage);
            return;
        }


       /* public override SKRectI GetResolution(
            Dictionary<PageComponentType, int> ComponentCounts,
            bool IsTall
        )
        {
            if (IsTall)
            {
                return new SKRectI(0, 0, 20, 20);
            }
            else
            {
                return new SKRectI(0, 0, 20, 20);
            }
        }
        */

        //TBD: Wide
        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Background:
                    grid.Children.Add(v, 0, 30, 0, 30);
                    return;
                case PageComponentType.PickedOption:
                    PlaceOption(grid,
                                new SKRectI(1, 4, 12, 15),
                                new SKRectI(0, 0, 10, 4),
                                CurrentOption,
                                OptionCount,
                                v
                    );
                    return;
                //case PageComponentType.Footer:
                //    grid.Children.Add(v, 0, 20, 18, 20);
                //    return;
                case PageComponentType.Message:
                    return;
                //case PageComponentType.Player:
                //    grid.Children.Add(v, 2, 10, 13, 18);
                //    return;
            }
            base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
        }
    }
}
