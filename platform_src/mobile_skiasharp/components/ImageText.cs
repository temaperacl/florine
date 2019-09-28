using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public class ImageText : IFlorineSkiaConnectable, IFlorineSkiaDrawable, Florine.IImage
    {
        public String Text { get; set; }
        public ImageText(String text) { Text = text; }
        public float FontSize = 64.0f;
        public enum WrapType
        {
            None,
            Shrink,
            WordWrap
        };
        public WrapType Overflow = WrapType.Shrink;
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

        public void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint = null)
        {
            if (Text == "") { return; }            
            // Figure out paint
            if(paint == null) 
            {
                paint = new SKPaint()
                {
                    TextSize = FontSize,
                    IsAntialias = true,
                    Color = new SKColor(0,0,0),
                    TextAlign = SKTextAlign.Center,
                    StrokeWidth = (FontSize / 12.0f),
                    Style = SKPaintStyle.Stroke,
                   
                };
                if (Overflow == WrapType.Shrink)
                {
                    float TextWidth = paint.MeasureText(Text);
                    if (TextWidth > boundingBox.Width)
                    {
                        paint.TextSize *= .95f * boundingBox.Width / TextWidth;
                    }
                }
            }
            List<string> lines = new List<string>();
            if (Overflow != WrapType.WordWrap)
            {
                lines.Add(Text);
            }
            else
            {
                // ...
                // SKFontMetrics FM = paint.FontMetrics;
            }
            // Draw
            for (int i = 0; i < lines.Count; ++i)
            {
                canvas.DrawText(
                                lines[i],
                                boundingBox.Left + boundingBox.Width / 2,
                                boundingBox.Bottom - boundingBox.Height / 4,
                                paint
                );
            }
            
            paint.Style = SKPaintStyle.Fill;
            paint.Color = new SKColor(250, 250, 250);
            for (int i = 0; i < lines.Count; ++i)
            {
                canvas.DrawText(
                            lines[i],
                            boundingBox.Left + boundingBox.Width / 2,
                            boundingBox.Bottom - boundingBox.Height / 4,
                            paint
                );
            }
            return;
        }
    }
}
