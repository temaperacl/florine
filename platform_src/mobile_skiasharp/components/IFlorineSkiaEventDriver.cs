using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace FlorineSkiaSharpForms
{
    public interface IFlorineSkiaEventDriver {
        event EventHandler OnEventTriggered;
    }

    public class FlorineSkiaSimpleEventDriver {
        public event EventHandler OnEventTriggered;
        public virtual void RaiseEventTrigger(EventArgs e) 
        {
            if(null != OnEventTriggered) {
                OnEventTriggered(this, e);
            }
        }

    }
}
