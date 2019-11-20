using System;
using System.Collections.Generic;
using System.Text;
using Florine;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public class ImageText : IFlorineSkiaConnectable, IFlorineSkiaDrawable, Florine.IImage
    {
        public static Florine.Controller GameController;
        public String Text { get; set; }
        public ImageText(String text) { Text = text; }
        public bool AutoBackground { get; set; } = false;
        public float FontSize = 64.0f;
        public enum VerticalAlignment
        {
            Top,
            Center
        }
        public enum HorizontalAlignment
        {
            Left, Center, Right
        }

        public static SKCanvasView AsView(
            string Message, 
            float FS = 64f, 
            WrapType WT = WrapType.WordWrap,
            HorizontalAlignment hAlignment = HorizontalAlignment.Center) {
            return new FlorineSkiaCVWrap(new ImageText(Message) { FontSize = FS, Overflow = WT, HAlign = hAlignment });
        }
        public HorizontalAlignment HAlign { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment VAlign { get; set; } = VerticalAlignment.Center;
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
            if (paint == null)
            {
                paint = new SKPaint()
                {
                    TextSize = FontSize,
                    IsAntialias = true,
                    Color = new SKColor(0, 0, 0),
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
            float lineStart = FM.Leading - FM.Top + FM.Bottom + 25f;
            float lineSpacing = FM.Leading - FM.Top + FM.Bottom;


            List<string> lines = new List<string>();
            if (Overflow == WrapType.None || Overflow == WrapType.Shrink)
            {
                lines.Add(Text);
                lineStart = boundingBox.Bottom - boundingBox.Height / 4;
            }
            if (Overflow == WrapType.WordWrap || Overflow == WrapType.DiamondWrap)
            {
                string[] firstWords = Text.Trim().Split(new char[] { ' ' });
                StringBuilder curLine = new StringBuilder();
                float LineLength = 0;
                float maxLineLength = boundingBox.Width * .75f;
                if (Overflow == WrapType.DiamondWrap)
                {
                    maxLineLength = CalculateDiamondLineLength(
                        boundingBox.Width,
                        boundingBox.Height,
                        lineSpacing * 1f,
                        lineSpacing * 2f
                    );
                }
                List<string> words = new List<string>();
                foreach (string word in firstWords)
                {
                    if (word.Contains("\n"))
                    {
                        string[] subWords = word.Split(new char[] { '\n' });
                        for (int i = 0; i < subWords.Length; ++i)
                        {
                            if (i > 0) { words.Add("\n"); }
                            words.Add(subWords[i]);
                        }
                        
                    }
                    else
                    {
                        words.Add(word);
                    }
                }
                foreach (string word in words)
                {
                    if (word.Length == 0) { continue; }                    
                    float wordLen = paint.MeasureText(word);
                    if (LineLength + wordLen > maxLineLength || word == "\n")
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
                    if (word != "\n")
                    {
                        curLine.Append(" ").Append(word);
                        LineLength += wordLen;
                    }
                }
                lines.Add(curLine.ToString());
                // ...
                if (VAlign == VerticalAlignment.Center)
                {
                    lineStart = boundingBox.Height / 2 - lineSpacing * lines.Count / 2 + lineSpacing * .75f;
                }
            }

            //canvas.DrawRect(0f, boundingBox.Top + lineStart, boundingBox.Width, lineSpacing * lines.Count, new SKPaint() { Color = new SKColor(0, 122, 122) });            

            // Draw
            if (AutoBackground)
            {
                FlOval ff = new FlOval()
                {
                    backgroundColor = new SKPaint() { Color = new SKColor(0, 80, 190, 230) },
                    Shape = FlOval.OvalType.Rectangle,
                    ovalRatio = float.NaN,
                    innerHighlight = new SKColor(100, 250, 250, 255),
                };
                ff.Draw(canvas, new SKRect(
                    boundingBox.Left,
                    boundingBox.Top + lineStart - lineSpacing - 5f,
                    boundingBox.Right,
                    boundingBox.Top + lineStart + lineSpacing * lines.Count
                    ));
            }

            for (int i = 0; i < lines.Count; ++i)
            {
                float boundingMidX = GetXCoord(lines[i], boundingBox.Left, boundingBox.Width, paint);/// boundingBox.Left + boundingBox.Width / 2;
                float boundingMidY = boundingBox.Top + lineStart + (lineSpacing * (i));
                canvas.DrawText(
                                    lines[i],
                                    boundingMidX,
                                    boundingMidY,
                                    paint
                    );
            }

            paint.Style = SKPaintStyle.Fill;
            paint.Color = new SKColor(250, 250, 250);
            for (int i = 0; i < lines.Count; ++i)
            {
                canvas.DrawText(
                            lines[i],
                            GetXCoord(lines[i], boundingBox.Left, boundingBox.Width, paint),
                            boundingBox.Top + lineStart + (lineSpacing * (i)),
                            paint
                );

            }

            /* Filters */
            for (int i = 0; i < lines.Count; ++i)
            {
                float boundingMidX = GetXCoord(lines[i], boundingBox.Left, boundingBox.Width, paint);
                float boundingMidY = boundingBox.Top + lineStart + (lineSpacing * (i));
                List<TextFragment> Filters = FilterLine(lines[i], paint, boundingMidX, boundingMidY);
                foreach (TextFragment TF in Filters)
                {
                    canvas.DrawText(TF.Text, TF.Location, TF.Paint);
                }
            }
            return;
        }
        private float GetXCoord(string s, float bbL, float bbW, SKPaint p)
        {
            if (s.Length == 0) { return 0f; }
            switch (HAlign) {
                case HorizontalAlignment.Center:
                    return bbL + bbW / 2;
                case HorizontalAlignment.Left:
                    return bbL + p.MeasureText(s) / 2;
                case HorizontalAlignment.Right:
                    return bbL + bbW - p.MeasureText(s) / 2;
            }
            return 0f;
        }
        private List<TextFragment> FilterLine(
            string line,
            SKPaint paint,
            float midX,
            float midY)
        {
            List<TextFragment> AllFilters = new List<TextFragment>();
            foreach (Florine.Nutrient n in GameController.CurrentFoundry.LoadNutrients()) {
                FlorineSkiaNutrient fsn = new FlorineSkiaNutrient(n);
                if (line.Contains(fsn.Name))
                {
                    AllFilters.AddRange(NutrientFragments(line, fsn, paint, midX, midY));
                }
            }

            //FlorineSkiaNutrient            
            return AllFilters;
        }
        private List<TextFragment> NutrientFragments(
            string line,
            FlorineSkiaNutrient n,
            SKPaint paint,
            float midX,
            float midY)
        {
            List<TextFragment> AllFilters = new List<TextFragment>();
            int idx = idx = line.IndexOf(n.Name, 0);
            float wordLen = paint.MeasureText(n.Name) / 2f;
            float midLine = paint.MeasureText(line) / 2f;
            SKPaint fiberPaint = new SKPaint()
            {
                TextSize = FontSize,
                IsAntialias = true,
                Color = n.RingColor,
                TextAlign = SKTextAlign.Center,                
            };
            while (idx >= 0) {                                                
                float preLine = paint.MeasureText(line.Substring(0, idx));                
                
                TextFragment FiberItem = new TextFragment()
                {
                    Text = n.Name,
                    Location = new SKPoint(
                        midX - midLine + preLine + wordLen,
                        midY
                    ),
                    Paint = fiberPaint,
                    WordLen = wordLen * 2f
                };
                AllFilters.Add(FiberItem);
                idx = line.IndexOf(n.Name, idx+1);                
            }
            return AllFilters;
        }    
        public class TextFragment
        {
            public string Text { get; set; } = "";
            public SKPaint Paint { get; set; } = new SKPaint();
            public SKPoint Location { get; set; } = new SKPoint();
            public float WordLen { get; set; } = 0f;
        }
    }
}
