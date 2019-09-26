﻿using System;
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
            // Figure out paint
            if(paint == null) 
            {
                paint = new SKPaint()
                {
                    TextSize = 64.0f,
                    IsAntialias = true,
                    Color = new SKColor(0,0,0),
                    TextAlign = SKTextAlign.Center,
                    StrokeWidth = 5,
                    Style = SKPaintStyle.Stroke,
                   
                };
            }
            // Draw
            canvas.DrawText(
                            Text, 
                            boundingBox.Left+boundingBox.Width/2, 
                            boundingBox.Bottom - boundingBox.Height/4,
                            paint
            );
            paint.Style = SKPaintStyle.Fill;
            paint.Color = new SKColor(250, 250, 250);
            canvas.DrawText(
                            Text,
                            boundingBox.Left + boundingBox.Width / 2,
                            boundingBox.Bottom - boundingBox.Height / 4,
                            paint
            );
            return;
        }
    }
}
