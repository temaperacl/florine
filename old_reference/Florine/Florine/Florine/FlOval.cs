using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace Florine
{
    public class FlOval
    {
        public SKPaint backgroundColor = new SKPaint() { Color = new SKColor(0, 0, 200, 0) };
        public SKRect outerBounds = new SKRect();
        public SKRect innerBounds = new SKRect();
        public List<Tuple<float, SKColor>> innerRing = new List<Tuple<float, SKColor>>();
        public List<Tuple<float, SKColor>> outerRing = new List<Tuple<float, SKColor>>();
        public SKImage mainImage { get { return coreImage.baseImage; } set { coreImage.baseImage = value; } }
        private AspectImage coreImage = new AspectImage();
        private float ringWidth = 10;
        private float ovalRatio = .5f;
        public void AutoPaint(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            outerBounds = RatioBox(new SKRect(info.Width * .15f, info.Height * .1f, info.Width * .85f, info.Height * .9f));
            innerBounds = RatioBox(new SKRect(info.Width * .15f + 12, info.Height * .1f + 12, info.Width * .85f - 12, info.Height * .9f - 12));

            this.Paint(canvas);
        }
        protected SKRect RatioBox(SKRect boundingBox)
        {

            float BoundRatio = boundingBox.Height / boundingBox.Width;
            float scaler = 1;
            float newWidth = boundingBox.Width;
            float newHeight = boundingBox.Height;
            if (ovalRatio < BoundRatio)
            {
                newHeight = boundingBox.Width * ovalRatio;
            }
            else
            {
                newWidth = boundingBox.Height / ovalRatio;
            }
            
            float deltaW = boundingBox.Width - newWidth;
            float deltaH = boundingBox.Height - newHeight;
            SKRect actualBound = new SKRect(
                boundingBox.Left + (deltaW / 2),
                boundingBox.Top + (deltaH / 2),
                boundingBox.Left + newWidth + (deltaW / 2),
                boundingBox.Top + newHeight + (deltaH / 2));
            return actualBound;
            
        }
        public void Paint(SKCanvas canvas)
        {
            if (null != backgroundColor)
            {

                canvas.DrawOval(innerBounds, backgroundColor);
            }
            SKRect innerRingBound = new SKRect(
                innerBounds.Left + ringWidth / 2,
                innerBounds.Top + ringWidth /2,
                innerBounds.Right - ringWidth /2,
                innerBounds.Bottom - ringWidth / 2);
            float fAngle = 180f;
            foreach (Tuple<float, SKColor> outerValue in outerRing)
            {
                SKPaint painter = new SKPaint()
                {
                    Color = outerValue.Item2,
                    IsStroke = true,
                    StrokeWidth = 10,
                };
                using (SKPath path = new SKPath())
                {
                    path.AddArc(outerBounds, fAngle, outerValue.Item1);
                    canvas.DrawPath(path, painter);
                    fAngle += outerValue.Item1;
                }
            }

            fAngle = 180f;
            foreach (Tuple<float, SKColor> innerValue in innerRing)
            {
                SKPaint painter = new SKPaint()
                {
                    Color = innerValue.Item2,
                    IsStroke = true,
                    StrokeWidth = ringWidth,
                };
                using (SKPath path = new SKPath())
                {
                    path.AddArc(innerRingBound, fAngle, innerValue.Item1);
                    canvas.DrawPath(path, painter);
                    fAngle += innerValue.Item1;
                }
            }
            
            coreImage.Draw(canvas, innerBounds);
            
        }
    }
}
