using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public interface IFlorineSkiaDrawable
    {
        void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint);
    }
}
