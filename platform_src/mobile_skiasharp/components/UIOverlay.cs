using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public class UIOverlay : IFlorineSkiaConnectable, IFlorineSkiaDrawable, Florine.IImage
    {        
        public String Text { get; set; }
        public UIOverlay(String text) { Text = text; }
        public float FontSize = 64.0f;
        public enum WrapType
        {
            None,
            Shrink,
            WordWrap,
            DiamondWrap
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

        float CalculateDiamondLineLength(float width, float height, float elevationA, float elevationB)
        {
            // (e - h/2) = -.5 .. .5 ... e/h = 0 .. 1 ... (e/h - h/2h = e/h - 1/2
            return Math.Min(
                width * 1.8f * (.5f - (float)Math.Abs((elevationA / height) - .5)),
                width * 1.8f * (.5f - (float)Math.Abs((elevationB / height) - .5))
            );
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

            SKFontMetrics FM = paint.FontMetrics;
            float lineStart = FM.Leading - FM.Top + 5;
            float lineSpacing = FM.Leading - FM.Top + FM.Bottom;


            List<string> lines = new List<string>();
            if (Overflow == WrapType.None || Overflow == WrapType.Shrink)
            {
                lines.Add(Text);
                lineStart = boundingBox.Bottom - boundingBox.Height / 4;
            }
            if (Overflow == WrapType.WordWrap || Overflow == WrapType.DiamondWrap)
            {
                string[] words = Text.Trim().Split(new char[] { ' ' });
                StringBuilder curLine = new StringBuilder();
                float LineLength = 0;
                float maxLineLength = boundingBox.Width * .9f;
                if ( Overflow == WrapType.DiamondWrap )
                {
                    maxLineLength = CalculateDiamondLineLength(                    
                        boundingBox.Width,
                        boundingBox.Height,
                        lineSpacing * 1f,
                        lineSpacing * 2f
                    );
                }
                foreach (string word in words)
                {
                    if (word.Length == 0) { continue; }
                    float wordLen = paint.MeasureText(word);
                    if (LineLength + wordLen > maxLineLength)
                    {
                        lines.Add(curLine.ToString());
                        curLine.Clear();
                        LineLength = 0;
                            if (Overflow == WrapType.DiamondWrap)
                            {
                                maxLineLength = CalculateDiamondLineLength(
                                    boundingBox.Width,
                                    boundingBox.Height,
                                    lineSpacing * lines.Count + lineSpacing,
                                    lineSpacing * (lines.Count + 1f) + (lineSpacing)
                                );
                            }
                    }
                    curLine.Append(" ").Append(word);
                    LineLength += wordLen;
                }
                lines.Add(curLine.ToString());
                // ...
                lineStart = boundingBox.Height / 2 - lineSpacing * lines.Count / 2 + lineSpacing * .75f;
            }

            //canvas.DrawRect(0f, boundingBox.Top + lineStart, boundingBox.Width, lineSpacing * lines.Count, new SKPaint() { Color = new SKColor(0, 122, 122) });            

            // Draw
            for (int i = 0; i < lines.Count; ++i)
            {
                canvas.DrawText(
                                lines[i],
                                boundingBox.Left + boundingBox.Width / 2,
                                boundingBox.Top + lineStart + (lineSpacing * (i)),
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
                            boundingBox.Top + lineStart + (lineSpacing * (i)),
                            paint
                );
            }
            return;
        }
    }
}
