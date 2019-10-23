﻿using System;
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
                        if (GameState.PageSubType.Daily == SubType) {
                            location = "busstop";
                        }
                        break;
                    case GameState.PageType.Select_Activity:
                        location = "busstop";
                        break;
                }
                if (_controller.CurrentState.Player.HoursIntoDay < 7
                    || _controller.CurrentState.Player.HoursIntoDay > 19)
                {
                    timeFrame = "evening";
                }
                else
                {
                    timeFrame = "day";
                }
                /*
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
                */

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
                newOpt.Description = ((Activity)opt).Description;
                Container.SelectionModel = FlorineSkiaOptionSet.SelectionType.SELECT_MOVE;
                pathType = "activities";
                string tokenName = opt.OptionName;                
                SKImage ResultImage = ResourceLoader.LoadImage("Images/" + pathType + "/" + tokenName.ToLower() + ".png");
                newOpt.Picture = new SelectableOptionImage()
                {
                    FoodImage = ((null == ResultImage) ?
                        (IFlorineSkiaDrawable)(new ImageText(tokenName))
                        : (IFlorineSkiaDrawable)(new FlOval()
                        {
                            mainImage = ResultImage,
                            backgroundColor = new SKPaint() { Color = new SKColor(230, 230, 230) }
                        }))
                };
            } else {
                // Food
                FlorineSkiaOption SourceOpt = opt as FlorineSkiaOption;
                Food.FoodOption food_data = opt as Food.FoodOption;
                if (null == food_data && SourceOpt != null)
                {
                    food_data = SourceOpt.SourceOpt as Food.FoodOption;
                }
                
                if (null != food_data)
                {
                    newOpt.Description = food_data.Parent.Description;
                }
                else
                {
                    newOpt.Description = "Desc Missing";
                }
                List<Tuple<float, SKColor>> MacroNuts = new List<Tuple<float, SKColor>>();
                List<Tuple<float, SKColor>> MicroNuts = new List<Tuple<float, SKColor>>();

                if (null != food_data && food_data.Parent.IsKnown)
                {
                    // Populate nutrient info bars.
                    // List<Tuple<float, SKColor>>                    
                    foreach (KeyValuePair<Nutrient, NutrientAmount> kvp in food_data.Parent.Nutrients)
                    {
                        FlorineSkiaNutrient AdjNut = new FlorineSkiaNutrient(kvp.Key);
                        float RelativeAmount = kvp.Key.RatioRDV(kvp.Value);
                        if (RelativeAmount > 2f) { RelativeAmount = 2f; }
                            //45 * ((float)(kvp.Value) / (float)(kvp.Key.DailyTarget));
                            //if (RelativeAmount > 45.0f) { RelativeAmount = 45.0f; }
                        if (kvp.Key.Class == Nutrient.NutrientType.Macro)
                        {
                            RelativeAmount *= 90f / 4f;
                            MacroNuts.Add(new Tuple<float, SKColor>(
                                RelativeAmount,
                                AdjNut.RingColor
                                ));
                        }
                        else
                        {
                            MicroNuts.Add(new Tuple<float, SKColor>(
                                RelativeAmount,
                                AdjNut.RingColor
                                ));
                            RelativeAmount *= 90f / 6f;
                        }
                    }
                    
                }
                string tokenName = opt.OptionName;
                switch (tokenName)
                {
                    case "Grilled Cheese":
                        tokenName = "grilledcheese"; break;
                    case "Pancakes":
                        tokenName = "pancakes"; break;
                    case "Fruit":
                        tokenName = "fruit"; break;
                    case "Eggs":
                        tokenName = "eggs"; break;
                    case "Cereal":
                        tokenName = "cereal"; break;
                    case "Toaster Pastry":
                        tokenName = "toastedpastry"; break;
                    case "Toast":
                    case "White Toast":
                    case "Wheat Toast":
                    case "Multigrain Toast":
                        tokenName = "toast"; break;
                    case "Strawberry Yogurt":
                        tokenName = "yogurt"; break;
                    case "Eggs, Bacon, and Toast":
                        tokenName = "toasteggsbacon"; break;
                    case "Hamburger":
                        tokenName = "hamburger"; break;
                    case "Chocolate Ice Cream":
                        tokenName = "icecream"; break;
                    case "Instant Noodles":
                        tokenName = "instantnoodles"; break;
                    case "Lamb Chops":
                        tokenName = "lambchops"; break;
                    case "Meatball Sub":
                        tokenName = "meatballhero"; break;
                    case "Peanut Butter & Jelly":
                        tokenName = "pbj"; break;
                    case "Pepperoni Pizza Slice":
                        tokenName = "pizza"; break;
                    case "Vegetable Soup":
                        tokenName = "soup"; break;
                    case "Spaghetti":
                        tokenName = "spaghetti"; break;
                    case "Spaghetti w/ Meatballs":
                        tokenName = "spaghettiwmeatball"; break;
                    case "Sushi Roll, Tuna":
                        tokenName = "sushi"; break;
                    case "Tacos":
                        tokenName = "tacos"; break;
                    case "Donuts":
                        tokenName = "donuts"; break;
                }
                SKImage ResultImage = ResourceLoader.LoadImage("Images/" + pathType + "/" + tokenName + ".png");
                newOpt.Picture = new SelectableOptionImage()
                {
                    FoodImage = ((null == ResultImage) ?
                        (IFlorineSkiaDrawable)(new ImageText(opt.OptionName))
                        : (IFlorineSkiaDrawable)(new FlOval()
                        {
                            mainImage = ResultImage,
                            backgroundColor = new SKPaint() { Color = new SKColor(230, 230, 230) },
                            RightRing = MicroNuts,
                            LeftRing = MacroNuts,

                        }))
                };

            }

            return newOpt;
        }
    }
}
