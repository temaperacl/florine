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
    }

    class FlorineSkiaOption : IGameOption
    {
        IGameOption _parent;
        public FlorineSkiaOption(IGameOption Parent)
        {
            _parent = Parent;
        }
        public string OptionName => _parent.OptionName;
        public void AdjustNutrients(NutrientSet n) { _parent.AdjustNutrients(n); }
        public void ImpactPlayer(Player p) { _parent.ImpactPlayer(p); }
        public IGameOptionSet SubOptions { get; set; }
        public IImage Picture { get; set; }
        
    }    
}

