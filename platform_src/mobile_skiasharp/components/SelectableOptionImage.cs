using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public class SelectableOptionImage : IFlorineSkiaDrawable, Florine.IImage, IFlorineSkiaConnectable
    {
        public int ImageKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Enabled { get; set; }
        public IFlorineSkiaDrawable FoodImage { get; set; }

        private FlOval _Highlight = new FlOval() {
            innerRing = {
                new Tuple<float, SKColor>(360f, new SKColor(0,0,230))
            },
            backgroundColor = new SKPaint() { Color = new SKColor(0, 0, 0, 0) }
        };
   
        public void ConnectCanvasView(SKCanvasView CV)
        {
            CV.PaintSurface += (sender, e) => {
            Draw(
                    e.Surface.Canvas,
                    new SKRect(
                        0, 0, e.Info.Width, e.Info.Height
                    )
                );
            };
        }

        public void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint = null)
        {
            canvas.Clear();
            if (null != FoodImage) { FoodImage.Draw(canvas, boundingBox,  paint); }
            if(Enabled) { _Highlight.Draw(canvas, boundingBox, paint); }
        }
    }
}
