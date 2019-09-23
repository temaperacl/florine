using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    public class LayeredImage : AspectImage
    {
        // SKRect, AspectImage ----
        //
        public List<FlOval>      Ovals = new List<FlOval>();
        public List<AspectImage> TopLayers = new List<AspectImage>();

        protected override void DrawImage(SKCanvas canvas, SKRect finalBoundingBox, SKPaint paint = null)
        {
            if (_image != null) { 
                canvas.DrawImage(_image, finalBoundingBox, paint);
            }
            foreach(AspectImage Img in TopLayers.Reverse()) {
                Img.DrawImage(canvas, finalBoundingBox, paint);
            }
            foreach(FlOval Oval in Ovals) {
                Ovals.DrawOval(canvas, finalBoundingBox, paint);
            }
        }
    }
}
