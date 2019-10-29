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
        public override void PostLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {
            int EnergyY = 20;
            int FocusY = EnergyY + 3;
            Player PC = GameController.CurrentState.Player;
            grid.Children.Add(_Text("Energy"), 15, 20, EnergyY, EnergyY+2);
            grid.Children.Add(
                _GradientBar(
                    0.0,
                    100.0,
                    100.0,
                    PC.Energy,
                    false,
                    true
                 ),
                20, 30, EnergyY, EnergyY + 2
            );

            grid.Children.Add(_Text("Focus"), 15, 20, FocusY, FocusY+2);
            grid.Children.Add(
                _GradientBar(
                    0.0,
                    100.0,
                    100.0,
                    PC.Focus,
                    false,
                    true
                 ),
                20, 30, FocusY, FocusY + 2
            );
            base.PostLayout(IsTall, grid, GameController, GameFoundry, SourcePage);
        }
        protected override void LayoutComponentWide(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            base.LayoutComponentWide(grid, t, v, CurrentOption, OptionCount);
        }

        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Message:
                    grid.Children.Add(v, 0, 30, 4, 16);
                    break;
                default:
                    base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
                    break;
            }
        }
    }    
}
