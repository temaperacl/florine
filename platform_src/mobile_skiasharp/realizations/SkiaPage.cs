using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Florine;

namespace FlorineSkiaSharpForms
{
    class Florine_SkiaPage : IPage
    {
        private IPage _parent;
        private Controller _controller;
        private SkiaSharpFormsFoundry _foundry;
        public Florine_SkiaPage(
            IPage Parent,
            SkiaSharpFormsFoundry Foundry,
            Controller GameController
            )
        {
            _parent = Parent;
            _foundry = Foundry;
            _controller = GameController;
        }

        public GameState.PageType MainType { get { return _parent.MainType; } }
        public GameState.PageSubType SubType { get { return _parent.SubType; } }
        public String Title { get { return _parent.Title; } }
        public String Message { get { return _parent.Message; } }
        public NutrientSet NutrientState { get { return _parent.NutrientState; } }
        public NutrientSet NutrientDelta { get { return _parent.NutrientDelta; } }

        public IImage Background { get { return null; } }


        public IGameOptionSet AppliedOptions { get { return _renderOptionSet(_parent.AppliedOptions); } }
        public IGameOptionSet PrimaryOptions { get { return _renderOptionSet(_parent.PrimaryOptions); } }

        /* Option Set Transformations ===================================================================== */
        private IGameOptionSet _renderOptionSet(IGameOptionSet opts)
        {
            if (null == opts) { return null; }
            FlorineSkiaOptionSet newSet = new FlorineSkiaOptionSet()
            {
                SelectionLimit = opts.SelectionLimit,
                Finalizer = _renderOption(opts.Finalizer)
            };
            foreach (IGameOption opt in opts)
            {
                newSet.Add(_renderOption(opt));
            }
            return newSet;
        }
        private IGameOption _renderOption(IGameOption opt)
        {
            FlorineSkiaOption newOpt = new FlorineSkiaOption(opt);
            newOpt.SubOptions = _renderOptionSet(opt.SubOptions);
            /* Image time ! */            
            newOpt.Picture = new FlOval()
            {
                mainImage = ResourceLoader.LoadImage("Images/food/" + opt.OptionName + ".png"),
            };            


            return newOpt;
        }        
    }
}
