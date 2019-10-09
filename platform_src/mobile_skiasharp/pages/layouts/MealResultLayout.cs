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
                IG.Details[(float)Min] = new SKColor(250, 0, 0);
                IG.Details[Center] = new SKColor(0, 250, 0);
                IG.Details[(float)Max] = new SKColor(250, 0, 0);
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
                grid.Children.Add(_Text("Calories"), 9, 13, 2, 3);
                View CalorieView = _GradientBar(
                     0,
                     PC.TargetCalories,
                     PC.TargetCalories * 2,
                     PC.Calories,
                     true,
                     false
                );
                grid.Children.Add(CalorieView, 13, 19, 2, 3);

                grid.Children.Add(_Text("Nutrients"),     9, 13, 3, 4);
                grid.Children.Add(_Text("Macronutrients"), 9, 13, 4, 5);


                grid.Children.Add(_Text("Energy"), 9, 13, 5, 6);
                grid.Children.Add(
                    _GradientBar(
                        0.0,
                        100.0,
                        100.0,
                        PC.Energy,
                        false,
                        true
                     ),
                    13, 19, 5, 6
                );

                grid.Children.Add(_Text("Focus"), 9, 13, 6, 7);
                grid.Children.Add(
                    _GradientBar(
                        0.0,
                        100.0,
                        100.0,
                        PC.Focus,
                        false,
                        true
                     ),
                    13, 19, 6, 7
                );

            }
            base.PostLayout(IsTall, grid, GameController, GameFoundry, CurrentPage);
            return;
        }


        public override SKRectI GetResolution(
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


        //TBD: Wide
        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Background:
                    grid.Children.Add(v, 0, 20, 0, 20);
                    return;
                case PageComponentType.PickedOption:
                    PlaceOption(grid,
                                new SKRectI(1, 1, 9, 12),
                                new SKRectI(0, 0, 8, 3),
                                CurrentOption,
                                OptionCount,
                                v
                    );
                    return;
                case PageComponentType.Footer:
                    grid.Children.Add(v, 0, 20, 18, 20);
                    return;
                case PageComponentType.Message:
                    return;
                case PageComponentType.Player:
                    grid.Children.Add(v, 2, 10, 13, 18);
                    return;
            }
            base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
        }
    }
}
