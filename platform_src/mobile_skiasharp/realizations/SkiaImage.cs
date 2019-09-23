using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    class Florine_SkiaPage : IImage
    {
        public int ImageKey { get; set; }
        public SkiaCanvasView AsView() {
            return null;
        }
    }
}
