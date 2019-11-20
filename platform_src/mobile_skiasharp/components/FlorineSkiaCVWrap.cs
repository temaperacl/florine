using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Florine;

namespace FlorineSkiaSharpForms
{
    public class FlorineSkiaCVWrap : SKCanvasView
    {
        public IFlorineSkiaConnectable FlorineObj { get; set; }
        public FlorineSkiaCVWrap(IFlorineSkiaConnectable Conn) : base()
        {
            FlorineObj = Conn;
            FlorineObj.ConnectCanvasView(this);
        }
    }
}
