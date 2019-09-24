using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    interface IFlorineSkiaDrawable
    {
        void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint);
    }
}
