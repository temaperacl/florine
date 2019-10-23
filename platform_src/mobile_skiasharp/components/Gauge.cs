using System;
using System.Collections.Generic;
using System.Text;
using Florine;
using SkiaSharp;
using SkiaSharp.Views;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    class Gauge : IFlorineSkiaConnectable, IFlorineSkiaDrawable, Florine.IImage
    {
        public float Min { get; set; } = 0;
        public float Max { get; set; } = 12;
        public float Value { get; set; } = 0;
        public bool Ticks { get; set; } = true;

        public int ImageKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ConnectCanvasView(SKCanvasView CV)
        {
            CV.PaintSurface += CV_PaintSurface;
        }

        private void CV_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            Draw(
                e.Surface.Canvas,
                e.Info.Rect,
                null
                );
        }

        public void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint)
        {
            SKPoint Center = new SKPoint(boundingBox.MidX, boundingBox.MidY);
            float Radius = Math.Min(boundingBox.Width, boundingBox.Height) / 2;
            SKRect Square = new SKRect(
                boundingBox.MidX - Radius,
                boundingBox.MidY - Radius,
                boundingBox.MidX + Radius,
                boundingBox.MidY + Radius);
            canvas.Clear();
            canvas.DrawCircle(
                Center, Radius, new SKPaint() { Color = SKColors.Black }
                );
            canvas.DrawCircle(
                Center, Radius-2, new SKPaint() { Color = SKColors.White }
                );
            float DPV = 360 / (Max - Min);
            // Pointer

            if (Ticks)
            {
                for (float i = Min; i < Max; i += 1f) {
                    float tick_angle = DPV * (i - Min) - 90f;
                    float tick_x = (float)(boundingBox.MidX + (Radius) * Math.Cos(Math.PI * tick_angle / 180f));
                    float tick_y = (float)(boundingBox.MidY + (Radius) * Math.Sin(Math.PI * tick_angle / 180f));
                    float tick_ix = (float)(boundingBox.MidX + (Radius * .8f) * Math.Cos(Math.PI * tick_angle / 180f));
                    float tick_iy = (float)(boundingBox.MidY + (Radius * .8f) * Math.Sin(Math.PI * tick_angle / 180f));
                    canvas.DrawLine(new SKPoint(tick_x, tick_y), new SKPoint(tick_ix, tick_iy), new SKPaint()
                    {
                        Color = SKColors.Black,
                        StrokeWidth = 2,
                        IsStroke = true
                    });
                }
            }

            float angle = DPV * (Value - Min) - 90f;
            float x = (float)(boundingBox.MidX + (Radius) * Math.Cos(Math.PI * angle / 180f));
            float y = (float)(boundingBox.MidY + (Radius) * Math.Sin(Math.PI * angle / 180f));
               
                //canvas.DrawPath(path, new SKPaint()
                canvas.DrawLine(new SKPoint(x,y), Center, new SKPaint()
                {
                    Color = SKColors.Black,
                    StrokeWidth = 3,
                    IsStroke = true
                });
            
        }
    }
}
