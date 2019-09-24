using System;
using System.Collections.Generic;
using System.Text;
using Florine;


namespace FlorineSkiaSharpForms
{
    class FlorineSkiaOptionSet : List<IGameOption>, IGameOptionSet
    {         
        public int SelectionLimit { get; set; }        
        public IGameOption Finalizer { get; set; }
        private List<IGameOption> _selected = new List<IGameOption>();
        public IGameOptionSet Selected {
            get {
                FlorineSkiaOptionSet resultSet = new FlorineSkiaOptionSet();
                foreach(IGameOption opt in _selected) {
                    resultsSet.Add(opt);
                }
                return resultSet;
            }
        }
        public bool ToggleOption(IGameOption opt) {
            if (_selected.Count >= SelectionLimit) { return false; }
            if (_selected.Contains(opt)) 
            {
                _selected.Remove(opt);
                return false;
            }
            foreach(nopt in this) {
                if(nopt == opt) {
                    _selected.Add(opt);
                    return true;
                }
            }
        }
    }

    class FlorineSkiaOption : IGameOption, IFlorineSkiaConnectable
    {
        IGameOption _parent;
        FlorineSkiaOptionSet _container;
        IGameOptionSet
        public FlorineSkiaOption(IGameOption Parent)
        {
            _parent = Parent;
        }
        public FlorineSkiaOption(IGameOption Parent, FlorineSkiaOptionSet Container)
        {
            _parent = Parent;
            _container = Container;
        }
        public bool Toggle() {
            if(null != _container) {
                bool CurrentState = _container.ToggleOption(this);
                FoodOptionImage img = Picture as FoodOptionImage;
                if(null != img) {
                    bool oldState = img.Enabled;
                    img.Enabled = CurrentState;
                    return (oldState != CurrentState);
                } 
                // Assume something changed
                return true; 
            }
            // No container - no toggling.
            return false;
        }
        public string OptionName => _parent.OptionName;
        public void AdjustNutrients(NutrientSet n) { _parent.AdjustNutrients(n); }
        public void ImpactPlayer(Player p) { _parent.ImpactPlayer(p); }
        public IGameOptionSet SubOptions { get; set; }
        public IImage Picture { get; set; }

        public void ConnectCanvasView(SKCanvasView CV) {
            IFlorineSkiaConnectable picConn = Picture as IFlorineSkiaConnectable;
            if(picConn != null) { picConn.ConnectCanvasView(CV); }
            CVTapWrap.Associate(CV, OptionTapHandler);                        
        }

        private void OptionTapHandler(object sender, CVTapWrap.TapEventArgs e)
        {
            if( Toggle() ) {
                // Toggle occured - any follow up?
                //
                if( null != e.Canvas ) {
                    // Redraw to pic up changes
                    e.Canvas.InvalidateSurface();
                }
            }
        }

        // TODO: Move to independent File
        // CVTapWrap ##########################################################
        // Wrapper for slightly easier association including explicit
        // propagation of Canvas object.
        //
        // Usage as TapGestureRecognizer or:
        // CVTapWrap wrapper = 
        //     CVTapWrap.Associate(<SKCanvasView>, [<TapEventHandler>]);        
        //
        // Where <SKCanvasView> is the CanvasView to grab
        // and <TapEventHandler> is an EventHandler that takes
        // (obj, CVTapWrap.TapEventArgs)
        // where TapEventArgs contains a .Canvas field that points to the
        // linked canvas.
        //
        // Alternatively or additionally , after creating/linking, the OnTap
        // event can be hooked into.
        //
        private class CVTapWrap : TapGestureRecognizer();
        {
            // Making this Static to make the usage slighly more intuitive
            public static CVTapWrap Associate(
                SKCanvasView CV,
                TapEventHandler func = null
            ) {
                CVTapWrap wrapper = new CVTapWrap(CV);
                if(null != func)
                {
                    wrapper.OnTap += func;
                }
                CV.GestureRecognizers.Add(wrapper);
                return wrapper;
            }

            // ############################################ Non Static CVTapWrap
            public SKCanvasView Tie { get; set; }
            public CVTapWrap(SKCanvasView CV) : base() {
                Tie = CV;
                this.Tapped += CanvasTapped;
            }

            // ############################################ Event Exposure
            private void CanvasTapped(object sender, EventArgs e) {
                RaiseTapEvent(e);
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
                public EventArgs OrginalEventArgs { get; private set; }
                public SKCanvasView Canvas { get; set; }
            }

            public delegate void TapEventHandler(object sender, TapEventArgs e);

            public event TapEventHandler OnTap;
        }
    }    
}

