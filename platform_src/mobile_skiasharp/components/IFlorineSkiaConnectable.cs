using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public interface IFlorineSkiaConnectable
    {
        void ConnectCanvasView(SKCanvasView CV);
    }
}
