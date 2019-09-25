using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public class ImageGradient : IFlorineSkiaConnectable, IFlorineSkiaDrawable, Florine.IImage
    {
        private SortedDictionary<float, SKColor> _details = new SortedDictionary<float, SKColor>();
        public SortedDictionary<float, SKColor> Details { get { return _details; } }
        public bool Horizontal { get; set; }
        public ImageGradient() {
            Horizontal = true;
        }
        public int ImageKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual void ConnectCanvasView(SKCanvasView CV) {
            CV.PaintSurface += (sender, e) => {
                Draw(
                    e.Surface.Canvas,
                    new SKRect(
                        0, 0, e.Info.Width, e.Info.Height
                    )
                );
            };
        }

        public void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint)
        {            
            // Figure out paint
            if(paint == null) 
            {
                paint = new SKPaint()
            } else {
                paint = paint.Clone();
            }

            paint.Shader = SKShader.CreateLinearGradient(
                new SKPoint(boundingBox.Left, boundingbox.Top),
                new SKPoint(
                    Horizontal?boundingBox.Right:boundingBox.Left,
                    Horizontal?boundingBox.Top:boundingBox.Bottom
                ),
                Details.Keys.ToArray(),
                Details.Values.ToArray(),
                SKShaderTileMode.Clamp
                ),
            );

            // Draw
            canvas.DrawText(
                            Text, 
                            boundingBox.Left, 
                            boundingBox.Top,
                            paint
            );
            return;
        }
    }
}
