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
        public enum SelectionType {
            SELECT_TOGGLE,
            SELECT_MOVE
        }
        private SelectionType _model = SelectionType.SELECT_TOGGLE;
        public SelectionType SelectionModel { get { return _model; } set { _model = value; }}
        public int SelectionLimit { get; set; }
        public IGameOption Finalizer { get; set; }
        private List<IGameOption> _selected = new List<IGameOption>();
        public class DescriptionUpdater : IFlorineSkiaConnectable, IFlorineSkiaDrawable
        {
            public DescriptionUpdater(IGameOptionSet parent) { _parent = parent; }
            private IGameOptionSet _parent;
            private LayeredImage _layers = new LayeredImage()
            {
                Layers = {
                   new FlOval() {
                       backgroundColor = new SKPaint() { Color = new SKColor(0,80,190, 230)},
                       Shape = FlOval.OvalType.Rectangle,
                       innerHighlight =  new SKColor(100,250,250, 255),
                   }
                }
            };
            private SKCanvasView _MainCanvas;
            private string _toDisp;
            public void DisplayIt(string S)
            {
                if (null == _MainCanvas) { return; }
                if (S == string.Empty)
                {
                    _MainCanvas.IsVisible = false;
                    _MainCanvas.InvalidateSurface();
                    return;
                }
                _toDisp = S;
                if (_layers.Layers.Count > 1) { _layers.Layers.RemoveAt(0); }
                _layers.Layers.Insert(
                    0,
                    new ImageText(_toDisp) {
                    //Overflow = ImageText.WrapType.DiamondWrap,
                    FontSize = 36.0f,                   
                } );
                if (null != _MainCanvas) { _MainCanvas.IsVisible = true; }
                _MainCanvas.InvalidateSurface();
            }
            public void ConnectCanvasView(SKCanvasView CV)
            {
                _MainCanvas = CV;
                _MainCanvas.IsVisible = false;
                _MainCanvas.PaintSurface += _MainCanvas_PaintSurface;
                FlorineSkiaTapWrap.Associate(CV, DescriptionTapHandler);
            }
            private void DescriptionTapHandler(object sender, FlorineSkiaTapWrap.TapEventArgs e)
            {
                _MainCanvas.IsVisible = false;
            }

                private void _MainCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
               
                    SKImageInfo info = args.Info;
                    SKSurface surface = args.Surface;
                    SKCanvas canvas = surface.Canvas;
                    canvas.Clear();
                Draw(canvas,
                    new SKRect(
                        0f,
                        0f,
                        (float)info.Width,
                        (float)info.Height
                        ),
                    null);

            }

            public void Draw(SKCanvas canvas, SKRect boundingBox, SKPaint paint)
            {
                _layers.Draw(canvas, boundingBox, paint);
            }
        }
        public DescriptionUpdater UpdaterHook;
        public FlorineSkiaOptionSet() {
            UpdaterHook = new DescriptionUpdater(this);
        }
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
        public bool OptionSelected(IGameOption opt)
        {
            return _selected.Contains(opt);
        }

        public bool ToggleOption(IGameOption opt) {

            if (_selected.Contains(opt))
            {
                UpdaterHook.DisplayIt(string.Empty);
                _selected.Remove(opt);
                return false;
            }
            if (_selected.Count > SelectionLimit || SelectionLimit == 0) { return false; }
            if (_selected.Count == SelectionLimit)
            {
                if ( SelectionModel == SelectionType.SELECT_MOVE )
                {
                    // Remove the first item on the list to clear room.
                    FlorineSkiaOption deadOpt = _selected[0] as FlorineSkiaOption;
                    _selected.RemoveAt(0);
                    if (null != deadOpt)
                    {
                        deadOpt.Redraw();
                    }
                    
                } else {
                    // Default refuse to add the option
                    return false;
                }
            }
            foreach (IGameOption nopt in this) {
                if (nopt == opt)
                {
                    _selected.Add(opt);
                    FlorineSkiaOption showOpt = opt as FlorineSkiaOption;
                    if (null != showOpt)
                    {
                        UpdaterHook.DisplayIt(showOpt.Description);
                    }
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
                SelectableOptionImage img = Picture as SelectableOptionImage;
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
        public string Description
        {
            get; set;
        }
        public IGameOption SourceOpt { get { return _parent; } }
        public void AdjustNutrients(NutrientSet n) { _parent.AdjustNutrients(n); }
        public void ImpactPlayer(Player p) { _parent.ImpactPlayer(p); }
        private IGameOptionSet _customSub;
        public IGameOptionSet SubOptions { get { if(null != _customSub) { return _customSub; } return _parent.SubOptions; } set { _customSub = value; } }
        public IImage Picture { get; set; }
        private List<SKCanvasView> _views = new List<SKCanvasView>();
        public void ConnectCanvasView(SKCanvasView CV) {
            IFlorineSkiaConnectable picConn = Picture as IFlorineSkiaConnectable;
            if(picConn != null) { picConn.ConnectCanvasView(CV); }
            FlorineSkiaTapWrap.Associate(CV, OptionTapHandler);
            _views.Add(CV);
        }

        public void Redraw()
        {
            if (null != _container)
            {
                bool CurrentState = _container.OptionSelected(this);
                SelectableOptionImage img = Picture as SelectableOptionImage;
                if (null != img)
                {
                    bool oldState = img.Enabled;
                    img.Enabled = CurrentState;                    
                }
            }
            foreach (SKCanvasView CV in _views)
            {
                CV.InvalidateSurface();
            }
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

