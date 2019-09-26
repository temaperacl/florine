using System;
using System.Collections.Generic;
using System.Text;
using Florine;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.ComponentModel;
using System.Collections;

namespace FlorineSkiaSharpForms
{    
    class FlorineSkiaOptionSet : List<IGameOption>, IGameOptionSet
    {         
        public int SelectionLimit { get; set; }        
        public IGameOption Finalizer { get; set; }
        private List<IGameOption> _selected = new List<IGameOption>();
        private class SelectedOptionGroup : List<IGameOption>, IGameOption, IGameOptionSet
        {
            public string OptionName => "Chosen Options";
            public IImage Picture => null;
            public IGameOptionSet SubOptions => this;
            public int SelectionLimit => SubOptions.Count;

            public IGameOption Finalizer => null;

            public void AdjustNutrients(NutrientSet n)
            {
                foreach (IGameOption opt in this)
                {
                    opt.AdjustNutrients(n);
                }
            }

            public void ImpactPlayer(Player p)
            {
                foreach (IGameOption opt in this)
                {
                    opt.ImpactPlayer(p);
                }
            }
        }

        public IGameOption Selected {
            get {
                SelectedOptionGroup resultSet = new SelectedOptionGroup();
                foreach(IGameOption opt in _selected) {
                    resultSet.SubOptions.Add(opt);
                }
                return resultSet;
            }
        }
        public bool ToggleOption(IGameOption opt) {
            
            if (_selected.Contains(opt)) 
            {
                _selected.Remove(opt);
                return false;
            }
            if (_selected.Count >= SelectionLimit) { return false; }
            foreach (IGameOption nopt in this) {
                if(nopt == opt) {
                    _selected.Add(opt);
                    return true;
                }
            }
            return false;
        }
    }

    class FlorineSkiaOption : FlorineSkiaSimpleEventDriver, IGameOption, IFlorineSkiaConnectable
    {
        IGameOption _parent;
        FlorineSkiaOptionSet _container;

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
            FlorineSkiaTapWrap.Associate(CV, OptionTapHandler);                        
        }

        private void OptionTapHandler(object sender, FlorineSkiaTapWrap.TapEventArgs e)
        {
            if( Toggle() ) {
                // Toggle occured - any follow up?
                //
                if( null != e.Canvas ) {
                    // Redraw to pic up changes
                    e.Canvas.InvalidateSurface();
                }
            }
            RaiseEventTrigger(new EventArgs());
        }
    }    
}

