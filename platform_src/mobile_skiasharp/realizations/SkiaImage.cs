using System;
using System.Collections.Generic;
using System.Text;
using Florine;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    class Florine_SkiaImage : IImage
    {
        public int ImageKey { get; set; }
        public SKCanvasView AsView() {
            return null;
        }
    }
}
