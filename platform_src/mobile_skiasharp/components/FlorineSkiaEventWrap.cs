using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    // TODO: Move to independent File
    // FlorineSkiaTapWrap ##########################################################
    // Wrapper for slightly easier association including explicit
    // propagation of Canvas object.
    //
    // Usage as TapGestureRecognizer or:
    // FlorineSkiaTapWrap wrapper =
    //     FlorineSkiaTapWrap.Associate(<SKCanvasView>, [<TapEventHandler>]);
    //
    // Where <SKCanvasView> is the CanvasView to grab
    // and <TapEventHandler> is an EventHandler that takes
    // (obj, FlorineSkiaTapWrap.TapEventArgs)
    // where TapEventArgs contains a .Canvas field that points to the
    // linked canvas.
    //
    // Alternatively or additionally , after creating/linking, the OnTap
    // event can be hooked into.
    //
    class FlorineSkiaTapWrap
    {
         // Making this Static to make the usage slighly more intuitive
         public static FlorineSkiaTapWrap Associate(
             SKCanvasView CV,
             TapEventHandler func = null
         ) {
             FlorineSkiaTapWrap wrapper = new FlorineSkiaTapWrap(CV);
             if(null != func)
             {
                 wrapper.OnTap += func;
             }
             CV.GestureRecognizers.Add(wrapper._tapRecognizer);
             return wrapper;
         }

         // ############################################ Non Static FlorineSkiaTapWrap
         private TapGestureRecognizer _tapRecognizer = new TapGestureRecognizer();
         public SKCanvasView Tie { get; set; }
         public FlorineSkiaTapWrap(SKCanvasView CV) : base() {
             Tie = CV;
             _tapRecognizer.Tapped += CanvasTapped;
         }

         // ############################################ Event Exposure
         private void CanvasTapped(object sender, EventArgs e) {
             RaiseTapEvent(new TapEventArgs(e));
         }

         protected virtual void RaiseTapEvent(TapEventArgs e) {
             e.Canvas = Tie;
             if(null != OnTap) {
                 OnTap(this, e);
             }
         }

         public class TapEventArgs : EventArgs {
             public TapEventArgs(EventArgs e) : base()
             {
                 OriginalEventArgs = e;
             }
             public EventArgs OriginalEventArgs { get; private set; }
             public SKCanvasView Canvas { get; set; }
         }

         public delegate void TapEventHandler(object sender, TapEventArgs e);

         public event TapEventHandler OnTap;
    }
}
