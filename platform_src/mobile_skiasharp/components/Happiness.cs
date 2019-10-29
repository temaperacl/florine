using System;
using System.Collections.Generic;
using System.Text;
using Florine;
using SkiaSharp;
using Xamarin.Forms;
using SkiaSharp.Views;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    class Happiness
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
        public static View RenderView(int Amount, bool Background = true)
        {
            Grid moneyGrid = new Grid();
            for (int i = 0; i < 10; ++i)
            {
                moneyGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < 16; ++i)
            {
                moneyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            ImageText MoneyText = new ImageText(Amount.ToString())
            {
                Overflow = ImageText.WrapType.None,
                FontSize = 48f,
            };

            FlOval MoneyBackground = new FlOval()
            {
                backgroundColor = new SKPaint() { Color = new SKColor(0, 80, 190, 230) },
                Shape = FlOval.OvalType.Rectangle,
                ovalRatio = float.NaN,
                innerHighlight = new SKColor(100, 250, 250, 255),
            };
            SKCanvasView MoneyRing = new SKCanvasView();
            MoneyRing.PaintSurface += MoneyRing_PaintSurface;
            if (Background)
            {
                moneyGrid.Children.Add(new FlorineSkiaCVWrap(MoneyBackground),
                    0,
                    moneyGrid.ColumnDefinitions.Count,
                    0,
                    moneyGrid.RowDefinitions.Count);
            }
            moneyGrid.Children.Add(MoneyRing, 3, 8, 0, 10);
            moneyGrid.Children.Add(new FlorineSkiaCVWrap(MoneyText), 6, 14, 0, 8);
            return moneyGrid;
        }
        private static void MoneyRing_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKPoint midPoint = new SKPoint(
                e.Info.Rect.MidX,
                e.Info.Rect.MidY
                );

            float Rad = 1f * (float)Math.Min(e.Info.Rect.MidX, e.Info.Rect.MidY);
            e.Surface.Canvas.Clear();
            e.Surface.Canvas.DrawCircle(midPoint, Rad, new SKPaint()
            { Color = SKColors.Black });
            e.Surface.Canvas.DrawCircle(midPoint, Rad - 2f, new SKPaint()
            { Color = SKColors.Purple });

            //
            SKPaint painter = new SKPaint()
            {
                Color = SKColors.Black,
                IsStroke = true,
                StrokeWidth = 3f,
            };
            using (SKPath path = new SKPath())
            {
                SKRect SmileBox = new SKRect(
                    midPoint.X - Rad * .7f,
                    midPoint.Y - Rad * .7f,
                    midPoint.X + Rad * .7f,
                    midPoint.Y + Rad * .7f
                    );

                SKRect EyeBoxLeft = new SKRect(
                    midPoint.X - Rad * .7f,
                    midPoint.Y - Rad * .5f,
                    midPoint.X - Rad * .2f,
                    midPoint.Y - Rad * .1f
                    );
                SKRect EyeBoxRight = new SKRect(
                    midPoint.X + Rad * .2f,
                    midPoint.Y - Rad * .5f,
                    midPoint.X + Rad * .7f,
                    midPoint.Y - Rad * .1f
                    );

                path.AddArc(SmileBox, 0f, 180f);
                path.AddArc(EyeBoxLeft, 180f, 180f);
                path.AddArc(EyeBoxRight, 180f, 180f);

                e.Surface.Canvas.DrawPath(path, painter);
            }
        }
    }
}
