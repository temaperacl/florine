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
        public SKColor BackgroundColor { get; set; }
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
        public ImageGradient()
        {
            Horizontal = true;
            BorderSize = .05f;
            BarWidth = 1;
            BackgroundColor = new SKColor(0, 0, 0);
        }
        public int ImageKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual void ConnectCanvasView(SKCanvasView CV)
        {
            CV.PaintSurface += (sender, e) =>
            {
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
            if (null != paint)
            {
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
                foreach (KeyValuePair<float, SKColor> kvp in Details)
                {
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

        /* Utility */
        public static SKCanvasView AsView(
          double Min,
          double Target,
          double Max,
          double Current,
          bool CanHaveTooMuch,
          bool MaskExcess,
          SKPaint PaintOverride = null
      )
        {
            ImageGradient IG = new ImageGradient() { CorePaint = PaintOverride };
            float Center = (float)((Target - Min) / (Max - Min));
            float CurPoint = (float)((Current - Min) / (Max - Min));
            if (CurPoint > 1) { CurPoint = 1; }
            if (CurPoint < 0) { CurPoint = 0; }

            if (CanHaveTooMuch)
            {

                IG.Details[0f] = new SKColor(250, 0, 0);
                IG.Details[.35f] = SKColors.Yellow;
                IG.Details[.5f] = new SKColor(0, 250, 0);
                IG.Details[.65f] = SKColors.Yellow;
                IG.Details[1f] = new SKColor(250, 0, 0);
                /*
                IG.Details[(float)Min] = new SKColor(250, 0, 0);
                IG.Details[(float)((Center - Min) / 2 + Min)] = SKColors.Yellow;
                IG.Details[Center] = new SKColor(0, 250, 0);
                IG.Details[(float)((Max - Center) / 2 + Center)] = SKColors.Yellow;
                IG.Details[(float)Max] = new SKColor(250, 0, 0);
                */
            }
            else
            {
                IG.Details[(float)Min] = new SKColor(250, 0, 0);
                //IG.Details[Center] = new SKColor(125, 125, 0);
                IG.Details[(float)Max] = new SKColor(0, 250, 0);
            }

            if (MaskExcess)
            {
                IG.BarWidth = CurPoint;
            }
            else
            {
                IG.IndicatorLineLoc = CurPoint;
            }
            IG.BorderSize = 5;

            return new FlorineSkiaCVWrap(IG);
            //return BarCanvas;
        }
        public static SKCanvasView AsDivBar(
            SortedDictionary<float, SKColor> Items,
            SKPaint PaintOverride = null
        )
        {
            ImageGradient IG = new ImageGradient()
            {
                CorePaint = PaintOverride,
                Style = ImageGradient.GradientType.RelativeSharp
            };
            foreach (KeyValuePair<float, SKColor> kvp in Items)
            {
                IG.Details.Add(kvp.Key, kvp.Value);
            }
            return new FlorineSkiaCVWrap(IG);
            //return BarCanvas;
        }
    }
}
