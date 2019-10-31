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
        public SKPaint CorePaint { get; set; }
        public float IndicatorLineLoc { get; set; }
        public float BarWidth { get; set; }
        public enum GradientType
        {
            Smooth,
            RelativeSharp
        }
        public GradientType Style { get; set; } = GradientType.Smooth;
        public ImageGradient() {
            Horizontal = true;
            BorderSize = .05f;
            BarWidth = 1;
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
            if (null != paint) {
                paint = paint.Clone();
            }            
            if (paint == null) 
            {
                if (null != CorePaint)
                {
                    paint = CorePaint.Clone();
                }
                else
                {
                    paint = new SKPaint();
                }   
            }

            if (Style == GradientType.Smooth)
            {
                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(boundingBox.Left + BorderSize, boundingBox.Top + BorderSize),
                    new SKPoint(
                        (Horizontal ? boundingBox.Right : boundingBox.Left) - BorderSize,
                        (Horizontal ? boundingBox.Top : boundingBox.Bottom) - BorderSize
                    ),
                    new List<SKColor>(Details.Values).ToArray(),
                    new List<float>(Details.Keys).ToArray(),
                    SKShaderTileMode.Clamp
                );
                SKPaint BackgroundPaint = new SKPaint()
                {
                    Color = BackgroundColor,
                    StrokeWidth = BorderSize,
                };
                float borderHeight = BorderSize; // boundingBox.Height * BorderSize;
                float borderWidth = BorderSize;  // boundingBox.Width * BorderSize;

                //Scale for border
                float LineLoc =
                    (Horizontal ? boundingBox.Width : boundingBox.Height) * IndicatorLineLoc
                    + (Horizontal ? boundingBox.Left : boundingBox.Top);
                canvas.Clear(BackgroundColor);
                canvas.DrawRect(
                    new SKRect(
                        boundingBox.Left + borderWidth,
                        boundingBox.Top + borderHeight,
                        boundingBox.Right - borderWidth,
                        boundingBox.Bottom - borderHeight
                    ), paint
                );
                canvas.DrawLine(
                                (Horizontal ? LineLoc : 0),
                                (Horizontal ? 0 : LineLoc),
                                (Horizontal ? LineLoc : boundingBox.Right),
                                (Horizontal ? boundingBox.Bottom : LineLoc),
                                BackgroundPaint
                );

                float EndLoc =
                      (Horizontal ? boundingBox.Width : boundingBox.Height) * BarWidth
                    + (Horizontal ? boundingBox.Left : boundingBox.Top);


                canvas.DrawRect(
                    new SKRect(
                                (Horizontal ? EndLoc : 0),
                                (Horizontal ? 0 : EndLoc),
                                boundingBox.Right,
                                boundingBox.Bottom
                    ),
                    BackgroundPaint
                );
            }

            if (Style == GradientType.RelativeSharp)
            {
                float TotalLength = boundingBox.Width;
                float curLen = boundingBox.Left;
                foreach (KeyValuePair<float, SKColor> kvp in Details) {
                    float ItemLen = TotalLength * kvp.Key;
                    if (null == CorePaint)
                    {
                        canvas.DrawRect(
                            new SKRect(curLen, boundingBox.Top, ItemLen, boundingBox.Bottom),
                            new SKPaint() { Color = kvp.Value }
                        );
                    }
                    else
                    {
                        using (SKPath xPath = new SKPath())
                        {
                            xPath.AddRect(new SKRect(curLen, boundingBox.Top, ItemLen, boundingBox.Bottom));
                            canvas.DrawPath(
                                xPath,
                                paint
                            );
                        }
                    }
                    curLen = ItemLen;                    
                }
            }
            

            
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
