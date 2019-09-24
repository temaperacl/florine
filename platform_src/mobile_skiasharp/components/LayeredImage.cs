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
            base.DrawImage(canvas, finalBoundingBox, paint);            
            for(int i = TopLayers.Count - 1; i > 0; --i) {                
                TopLayers[i].Draw(canvas, finalBoundingBox, paint);
            }
            for (int i = Ovals.Count - 1; i > 0; --i)
            {
                Ovals[i].DrawOval(canvas, finalBoundingBox, paint);
            }
        }
    }
}
