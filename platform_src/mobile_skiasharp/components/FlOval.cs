using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public class FlOval : IFlorineSkiaConnectable, IFlorineSkiaDrawable, Florine.IImage
    {
        public int ImageKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SKPaint backgroundColor = new SKPaint() { Color = new SKColor(0, 0, 0, 0) };
        public SKRect outerBounds = new SKRect();
        public SKRect innerBounds = new SKRect();
        public enum OvalType
        {
            Oval,
            Rectangle
        }
        public OvalType Shape { get; set; } = OvalType.Oval;
        public List<Tuple<float, SKColor>> innerRight = new List<Tuple<float, SKColor>>();
        public List<Tuple<float, SKColor>> innerLeft = new List<Tuple<float, SKColor>>();
        public List<Tuple<float, SKColor>> LeftRing { get { return innerLeft; } set { innerLeft = value; } }
        public List<Tuple<float, SKColor>> RightRing { get { return innerRight; } set { innerRight = value; } }
        public List<Tuple<float, SKColor>> outerRing = new List<Tuple<float, SKColor>>();
        public List<Tuple<float, SKColor>> Outer { get { return outerRing; } set { outerRing = value; } }
        public SKColor innerHighlight
        {
            set
            {
                innerRight.Add(new Tuple<float, SKColor>(180f, value));
                innerLeft.Add(new Tuple<float, SKColor>(180f, value));
            }
        }
        public SKImage mainImage { get { return coreImage.baseImage; } set { coreImage.baseImage = value; } }
        private AspectImage coreImage = new AspectImage();
        private float ringWidth = 10;
        private float _oR = .5f;
        public float ovalRatio { get { return _oR; } set { _oR = value; } }
             
        public void Draw(SKCanvas canvas, SKRect info, SKPaint paint = null)
        { DrawOval(canvas, info, paint); }
        public void DrawOval(SKCanvas canvas, SKRect info, SKPaint paint = null)
        {

            outerBounds = RatioBox(new SKRect(
                                              info.Left + info.Width * .05f,
                                              info.Top  + info.Height * .03f,
                                              info.Left + info.Width * .95f,
                                              info.Top  + info.Height * .97f));
            innerBounds = new SKRect(
                                              outerBounds.Left + 9,
                                              outerBounds.Top + 9,
                                              outerBounds.Right - 9,
                                              outerBounds.Bottom - 9
                                              );

            this.Paint(canvas);
        }

        public virtual void ConnectCanvasView(SKCanvasView CV)
        {
            CV.PaintSurface += AutoPaint;
        }

        public void AutoPaint(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            outerBounds = RatioBox(new SKRect(
                info.Width * .05f,
                info.Height * .03f,
                info.Width * .95f,
                info.Height * .97f));
            innerBounds = RatioBox(new SKRect(
                outerBounds.Left + 9,
                outerBounds.Top + 9, 
                outerBounds.Right - 9,
                outerBounds.Bottom - 9
               )
               );

            this.Paint(canvas);
        }

        protected SKRect RatioBox(SKRect boundingBox)
        {
            if (float.IsNaN(ovalRatio))
            {
                return boundingBox;
            }
            float BoundRatio = boundingBox.Height / boundingBox.Width;
            //float scaler = 1;
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
        private void subDrawCore(SKCanvas canvas, SKRect bounds, SKPaint paint)
        {
            switch (Shape) {
                case OvalType.Oval:
                    canvas.DrawOval(bounds,paint);
                    break;
                case OvalType.Rectangle:
                    canvas.DrawRoundRect(bounds, 5f, 5f, paint);
                    //canvas.DrawRect(bounds, paint);
                    break;
            }
        }
        //private SKPoitn
        private void subDrawRing(
            SKCanvas canvas,
            SKRect bounds,
            List<Tuple<float, SKColor>> RingValues,
            float fBase,
            SKPaint backingPaint = null,
            SKPaint edgePaint = null,
            float fEnd = float.NaN)
        {

            
            float fAngle = fBase;
            foreach (Tuple<float, SKColor> ringValue in RingValues)
            {
                SKPaint painter = new SKPaint()
                {
                    Color = ringValue.Item2,
                    IsStroke = true,
                    StrokeWidth = ringWidth,
                };
                if (null != edgePaint)
                {
                    using (SKPath path = new SKPath())
                    {
                        switch (Shape)
                        {
                            case OvalType.Oval:
                                path.AddArc(bounds, fAngle, ringValue.Item1);
                                break;
                            case OvalType.Rectangle:
                                path.AddRoundRect(bounds, 5f, 5f);
                                break;
                        }
                        
                        canvas.DrawPath(path, edgePaint);
                    }
                }
                using (SKPath path = new SKPath())
                {
                    switch (Shape)
                    {
                        case OvalType.Oval:
                            path.AddArc(bounds, fAngle, ringValue.Item1);
                            break;
                        case OvalType.Rectangle:
                            path.AddRoundRect(bounds, 5f, 5f);
                            break;
                    }
                    
                    canvas.DrawPath(path, painter);
                }
                fAngle += ringValue.Item1;
            }
            if (null != backingPaint)
            {
                if (float.IsNaN(fEnd))
                {
                    fEnd = fAngle + 180f;
                }
                using (SKPath path = new SKPath())
                {
                    switch (Shape)
                    {
                        case OvalType.Oval:
                            path.AddArc(bounds, fAngle, fEnd - fAngle);
                            break;
                        case OvalType.Rectangle:
                            path.AddRoundRect(bounds, 5f, 5f);
                            break;
                    }                    
                    canvas.DrawPath(path, backingPaint);
                }
            }


        }
        public void Paint(SKCanvas canvas)
        {
            if (null != backgroundColor)
            {
                subDrawCore(canvas, innerBounds, backgroundColor);                
            }

            SKRect innerRingBound = new SKRect(
                innerBounds.Left ,// + ringWidth / 2,
                innerBounds.Top ,// + ringWidth /2,
                innerBounds.Right ,// - ringWidth /2,
                innerBounds.Bottom );// - ringWidth / 2);


            SKPaint BackPaint = new SKPaint()
            {
                Color = backgroundColor.Color,
                IsStroke = true,
                StrokeWidth = ringWidth,
            };
            SKPaint EdgePaint = new SKPaint()
            {
                Color = new SKColor(0,0,0),
                IsStroke = true,
                StrokeWidth = ringWidth,
            };
            
            subDrawRing(
                canvas,
                outerBounds,
                outerRing,
                180f);
            subDrawRing(
                canvas,
                innerRingBound,
                innerRight,
                0f,
                BackPaint,
                EdgePaint,
                180f);
            subDrawRing(
                canvas,
                innerRingBound,
                innerLeft,
                180f,
                BackPaint,
                EdgePaint,
                360f);

            coreImage.Draw(canvas, innerBounds);
            
        }
    }
}
