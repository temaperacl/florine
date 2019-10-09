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
        public List<IFlorineSkiaDrawable> Layers = new List<IFlorineSkiaDrawable>();
        protected override bool NeedImage { get { return false; } }
        protected override void DrawImage(SKCanvas canvas, SKRect finalBoundingBox, SKPaint paint = null)
        {
            base.DrawImage(canvas, finalBoundingBox, paint);            
            for(int i = Layers.Count - 1; i >= 0; --i) {                
                Layers[i].Draw(canvas, finalBoundingBox, paint);
            }
        }
    }
}
