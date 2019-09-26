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
        public SKColor BackgroundColor { get; set;}
        public float BorderSize { get; set; }
        public float IndicatorLineLoc { get; set; }
        public float BarWidth { get; set; }
        public ImageGradient() {
            Horizontal = true;
            BorderSize = 0;
            BackgroundColor = new SKColor(0,0,0);
        }
        public int ImageKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual void ConnectCanvasView(SKCanvasView CV) {
            CV.PaintSurface += (sender, e) => {
                Draw(
                    e.Surface.Canvas,
                    new SKRect(
                        0, 0, e.Info.Width, e.Info.Height
                    ),
                    null
                );
            };
        }

        public void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint)
        {            
            // Figure out paint
            if(paint == null) 
            {
                paint = new SKPaint();
            } else {
                paint = paint.Clone();
            }

            
            paint.Shader = SKShader.CreateLinearGradient(
                new SKPoint(boundingBox.Left + BorderSize, boundingBox.Top + BorderSize),
                new SKPoint(
                    (Horizontal?boundingBox.Right:boundingBox.Left) - BorderSize,
                    (Horizontal?boundingBox.Top:boundingBox.Bottom) - BorderSize;
                ),
                new List<SKColor>(Details.Values).ToArray(),
                new List<float>(Details.Keys).ToArray(),
                SKShaderTileMode.Clamp                
            );
            paint BackgroundPaint = new SKPaint()
            {
                Color = BackgroundColor,
                StrokeWidth = BorderSize
            };

            //Scale for border
            float LineLoc =
                (Horizontal?boundingBox.Width:BoundingBox.Height) * IndicatorLineLoc 
                + (Horizontal?boundingBox.Left:BoundingBox.Top);
            canvas.Clear(BackgroundColor);
            canvas.DrawRect(boundingBox, paint);
            canvas.DrawLine(
                            (Horizontal?LineLoc:0),
                            (Horizontal?0:LineLoc),
                            (Horizontal?LineLoc:boundingBox.Right),
                            (Horizontal?boundingBox.Bottom:LineLoc),
                            BackgroundPaint
            );
            float EndLoc = 
                  (Horizontal?boundingBox.Width:BoundingBox.Height) * BarWidth
                + (Horizontal?boundingBox.Left:BoundingBox.Top);

            canvas.DrawRect(
                new SKRect(
                            (Horizontal?EndLoc:0),
                            (Horizontal?0:EndLoc),
                            boundingbox.Right,
                            boundingBox.Bottom
                ),
                BackgroundPaint
            );

            

            
            // Draw
            // And IndicatorLine
            // And BarWidth
            /*
            canvas.DrawText(
                            Text, 
                            boundingBox.Left, 
                            boundingBox.Top,
                            paint
            );
            */
            return;
        }
    }
}
