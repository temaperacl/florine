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

        public IImage Background
        {
            get
            {
                //if (null == _parent.Background) { return null; }
                string timeFrame = "day";
                string location = "work";
                switch (MainType) {
                    case GameState.PageType.Day_Intro:
                    case GameState.PageType.Select_Meal:
                    case GameState.PageType.Summarize_Meal:
                        location = "kitchen";
                        break;
                    case GameState.PageType.Summarize_Activity:
                        location = "work";
                        if(GameState.PageSubType.Daily == SubType) {
                            location = "busstop";
                        }
                        break
                    case GameState.PageType.Select_Activity:
                        location = "busstop";
                        break;
                }
                switch (SubType) {
                    case GameState.PageSubType.Breakfast:
                        timeFrame = "day";
                        break;
                    case GameState.PageSubType.Dinner:
                        timeFrame = "evening";
                        break;
                    case GameState.PageSubType.Lunch:
                        timeFrame = "day";
                        break;
                    case GameState.PageSubType.Daily:
                        timeFrame = "evening";
                        break;
                    case GameState.PageSubType.None:
                        timeFrame = "day";
                        break;
                    case GameState.PageSubType.Setup:
                        return null;

                }

                return new AspectImage()
                {
                    baseImage = ResourceLoader.LoadImage("Images/backdrops/" + timeFrame + "_" + location + ".png"),
                    scaling = AspectImage.ScalingType.Stretch
                };
            }
        }


        public IGameOptionSet AppliedOptions { get { return _renderOptionSet(_parent.AppliedOptions); } }
        public IGameOptionSet PrimaryOptions { get { return _renderOptionSet(_parent.PrimaryOptions); } }

        /* Option Set Transformations ===================================================================== */
        private IGameOptionSet _renderOptionSet(IGameOptionSet opts)
        {
            if (null == opts) { return null; }
            FlorineSkiaOptionSet newSet = new FlorineSkiaOptionSet()
            {
                SelectionLimit = opts.SelectionLimit,
            };

            if (opts.Finalizer != null)
            {
                newSet.Finalizer = _renderOption(opts.Finalizer, newSet);
            }
            foreach (IGameOption opt in opts)
            {
                newSet.Add(_renderOption(opt, newSet));
            }

            return newSet;
        }
        private IGameOption _renderOption(IGameOption opt, FlorineSkiaOptionSet Container)
        {
            FlorineSkiaOption newOpt = new FlorineSkiaOption(opt, Container);
            if (null != newOpt.SubOptions)
            {
                newOpt.SubOptions = _renderOptionSet(opt.SubOptions);
            }

            // Absurdly Hackity
            String pathType = "food";
            if (opt is Activity) {
                //Activity
                pathType = "Activity";
                newOpt.Picture = new AspectImage() {
                    baseImage = ResourceLoader.LoadImage("Images/" + pathType + "/" + opt.OptionName + ".png")
                };
            } else {
                // Food
                SKImage ResultImage = ResourceLoader.LoadImage("Images/" + pathType + "/" + opt.OptionName + ".png");
                newOpt.Picture = new FoodOptionImage()
                {
                    FoodImage = ((null == ResultImage) ?
                        (IFlorineSkiaDrawable)(new ImageText(opt.OptionName))
                        : (IFlorineSkiaDrawable)(new FlOval()
                        {
                            mainImage = ResultImage,
                            backgroundColor = new SKPaint() { Color = new SKColor(230, 230, 230) }
                        }))
                };

            }

            return newOpt;
        }
    }
}
