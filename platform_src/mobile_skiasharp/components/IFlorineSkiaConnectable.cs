using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    interface IFlorineSkiaConnectable
    {
        void ConnectCanvasView(SKCanvasView CV);
    }
}
