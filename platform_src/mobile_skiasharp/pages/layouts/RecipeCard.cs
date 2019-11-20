using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florine;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public class RecipeCard : PageLayout
    {
 

        // Allow for pre-component rendering layout
        public override void PreLayout(
            bool IsTall,
            Grid grid,
            Controller GameController,
            IPlatformFoundry GameFoundry,
            IPage SourcePage
        )
        {
            return;
        }

        private Food FindFood(string Name, Controller GC)
        {
            
            IList<Food> foodList = GC.CurrentFoundry.LoadFood();
            foreach (Food f in foodList)
            {
                if (f.Name == Name) { return f; }
            }
            return null;    
        }
        //Allow for post-component rendering layout
        public override void PostLayout(
            bool IsTall,
            Grid grid,
            Controller GameController,
            IPlatformFoundry GameFoundry,
            IPage CurrentPage
        )
        {            
            Player PC = GameController.CurrentState.Player;

            if (IsTall)
            {
                grid.Children.Add(FlOval.AsView(),
                    0, grid.ColumnDefinitions.Count,
                    0, grid.RowDefinitions.Count);
                Food f = FindFood("Mexican Rice",GameController);

                List<string> Ingredients = new List<string>()
                {
                    "1 Medium tomato",
                    "1/4 small onion",
                    "1 cup rice",
                    "1 clove garlic",
                    "2 tbsp corn oil",
                    "2 cups water"
                };
                List<string> Steps = new List<string>()
                {
                    "Mince the tomato, garlic, and onion",
                    "Put pot on stove over medium heat",
                    "Once pot is hot, add oil",
                    "Once oil is hot, add rice",
                    "Stir rice constantly until toasted (not burnt)",
                    "Add the vegetables to the rice and stir, then add the water and stir",
                    "Add salt and pepper to taste",
                    "Once water starts to boil, turn heat to low and put lid on pot. Wait ~20 minutes, or until water is almost all absorbed",
                    "Remove from heat and rest 5 minutes",
                    "Fluff with spoon and serve!"
                };
                grid.Children.Add(ImageText.AsView(f.Name),
                    0, grid.ColumnDefinitions.Count,
                    2, 4);



                for (int i = 0; i < Ingredients.Count; ++i)
                {
                    int VShift = 1 * (int)(i / 2);
                    int Shift = (i % 2) * grid.ColumnDefinitions.Count/2 + 3 * (1- (i%2));
                    grid.Children.Add(ImageText.AsView(
                        Ingredients[i], 
                        32f,
                        ImageText.WrapType.Shrink,
                        ImageText.HorizontalAlignment.Left
                        ),
                    Shift, grid.ColumnDefinitions.Count/2 + Shift,
                    7 + VShift, 9 + VShift);
                }

                
                String StepCounts = "1.\n2.\n3.\n4.\n5.\n\n6.\n\n7.\n8.\n\n\n\n9.\n10.\n";
                
                grid.Children.Add(
                    ImageText.AsView(
                        string.Join("\n", StepCounts),
                        32f,
                        ImageText.WrapType.WordWrap,
                        ImageText.HorizontalAlignment.Left),
                    3, grid.ColumnDefinitions.Count - 1,
                    (int)(6 + Ingredients.Count / 2), grid.RowDefinitions.Count - 1);

                grid.Children.Add(
                    ImageText.AsView(
                        string.Join("\n",Steps),
                        32f,
                        ImageText.WrapType.WordWrap,
                        ImageText.HorizontalAlignment.Left),
                    5, grid.ColumnDefinitions.Count,
                    (int)(6 + Ingredients.Count / 2), grid.RowDefinitions.Count - 1);

                int TX1 = 10;
                int TX2 = 15;
                int BR1 = TX2;
                int BR2 = 27;
                grid.Children.Add(FoodPic(f), 2, 11, 4, 7);
                // Calories
                grid.Children.Add(ImageText.AsView("Calories",24f,ImageText.WrapType.Shrink), TX1, TX2, 4, 5);
                View CalorieView = ImageGradient.AsView(
                     0,
                     2000,
                     4000,
                     325.5985,
                     true,
                     false
                );
                
                grid.Children.Add(CalorieView, BR1, BR2, 4, 5);
                SortedDictionary<float, SKColor> WhiteBar = new SortedDictionary<float, SKColor>()
                {
                    { 0f, new SKColor(255,255,255,60)},
                    { 1f, new SKColor(255,255,255,60) },
                };
                SortedDictionary<float, SKColor> MicroNutrients = new SortedDictionary<float, SKColor>();
                SortedDictionary<float, SKColor> MicroPotential = new SortedDictionary<float, SKColor>();
                SortedDictionary<float, SKColor> MacroNutrients = new SortedDictionary<float, SKColor>();
                float microNut = 0f;
                float microPot = 0f;
                float macroNut = 0f;
                
                foreach (KeyValuePair<Nutrient, NutrientAmount> kvp in f.Nutrients)
                {
                    FlorineSkiaNutrient AdjNut = new FlorineSkiaNutrient(kvp.Key);
                    float curRatio = kvp.Key.RatioRDV(kvp.Value);                    
                    
                    switch (kvp.Key.Class)
                    {
                        case Nutrient.NutrientType.Macro:
                            if (curRatio > 2f) { curRatio = 2f; }
                            if (curRatio <= 0f) { continue; }
                            curRatio /= 8f;
                            macroNut += curRatio;
                            MacroNutrients.Add(macroNut, AdjNut.RingColor);
                            break;
                        case Nutrient.NutrientType.Mineral:
                        case Nutrient.NutrientType.Vitamin:
                            if (curRatio > 1f) { curRatio = 1f; }
                            float fRestRatio = float.NaN;
                            if (curRatio < 1f)
                            {
                                fRestRatio = 1f - curRatio;
                                fRestRatio /= 7f;
                            }
                            curRatio /= 7f;
                            microPot += 1f / 7f;

                            if (curRatio > float.Epsilon)
                            {
                                microNut += curRatio;
                                MicroNutrients.Add(microNut, AdjNut.RingColor);
                            }
                            if (!float.IsNaN(fRestRatio))
                            {
                                microNut += fRestRatio;
                                MicroNutrients.Add(microNut, SKColors.Transparent);
                            }
                            SKColor newCol = new SKColor(
                                AdjNut.RingColor.Red,
                                AdjNut.RingColor.Green,
                                AdjNut.RingColor.Blue,
                                80
                                );
                            MicroPotential.Add(microPot, newCol);
                            break;
                    }
                }
                grid.Children.Add(ImageText.AsView("Vitamins", 24f, ImageText.WrapType.Shrink),TX1, TX2, 5, 6);


                grid.Children.Add(ImageGradient.AsDivBar(WhiteBar), BR1, BR2, 5, 6);
                grid.Children.Add(ImageGradient.AsDivBar(MicroPotential), BR1, BR2, 5, 6);
                float gridSize = 5;
                grid.Children.Add(ImageGradient.AsDivBar(MicroPotential,
                    new SKPaint()
                    {
                        PathEffect = SKPathEffect.Create2DLine(1,
                            MatrixMultiply(SKMatrix.MakeScale(gridSize, gridSize),
                            SKMatrix.MakeRotationDegrees(45))),
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 1,
                        Color = new SKColor(0,0,0,20)
                    }), BR1, BR2, 5, 6);
                   
                grid.Children.Add(ImageGradient.AsDivBar(MicroPotential,
                    new SKPaint()
                    {
                        PathEffect = SKPathEffect.Create2DLine(1,
                            MatrixMultiply(SKMatrix.MakeScale(gridSize, gridSize),
                            SKMatrix.MakeRotationDegrees(-45))),
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 1,
                        Color = new SKColor(0, 0, 0, 20)
                    }), BR1, BR2, 5, 6);
                    
                grid.Children.Add(ImageGradient.AsDivBar(MicroNutrients), BR1, BR2, 5, 6);
                

                grid.Children.Add(ImageText.AsView("Nutrients", 24f, ImageText.WrapType.Shrink),TX1, TX2, 6, 7);
                grid.Children.Add(ImageGradient.AsDivBar(MacroNutrients), BR1, BR2,6, 7);

                            }

            base.PostLayout(IsTall, grid, GameController, GameFoundry, CurrentPage);
            return;
        }
        static SKMatrix MatrixMultiply(SKMatrix first, SKMatrix second)
        {
            SKMatrix target = SKMatrix.MakeIdentity();
            SKMatrix.Concat(ref target, first, second);
            return target;
        }

        private SKCanvasView FoodPic(Food food)
        {
            List<Tuple<float, SKColor>> MacroNuts = new List<Tuple<float, SKColor>>();
            List<Tuple<float, SKColor>> MicroNuts = new List<Tuple<float, SKColor>>();
            foreach (KeyValuePair<Nutrient, NutrientAmount> kvp in food.Nutrients)
            {
                FlorineSkiaNutrient AdjNut = new FlorineSkiaNutrient(kvp.Key);
                float RelativeAmount = kvp.Key.RatioRDV(kvp.Value);

                //45 * ((float)(kvp.Value) / (float)(kvp.Key.DailyTarget));
                //if (RelativeAmount > 45.0f) { RelativeAmount = 45.0f; }
                if (kvp.Key.Class == Nutrient.NutrientType.Macro)
                {
                    RelativeAmount *= 180f / 4f;
                    MacroNuts.Add(new Tuple<float, SKColor>(
                        RelativeAmount,
                        AdjNut.RingColor
                        ));
                }
                else
                {
                    if (RelativeAmount > 1f) { RelativeAmount = 1f; }
                    RelativeAmount *= 180 / 6f;
                    MicroNuts.Add(new Tuple<float, SKColor>(
                        RelativeAmount,
                        AdjNut.RingColor
                        ));

                }
            }
            LayeredImage newImg= new LayeredImage()
            {
                Layers = {
                                new ImageText("Image Not Available") { Overflow = ImageText.WrapType.DiamondWrap, FontSize = 24f },
                                new FlOval()
                            {
                                mainImage = null,
                                backgroundColor = new SKPaint() { Color = new SKColor(230, 230, 230) },
                                RightRing = MicroNuts,
                                LeftRing = MacroNuts,

                            }
                            }
            };
            return new FlorineSkiaCVWrap(newImg);
        }
     
        //TBD: Wide
        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Background:
                    grid.Children.Add(v, 0, 30, 0, 30);
                    return;
                case PageComponentType.PickedOption:
                    return;
                //case PageComponentType.Footer:
                //    grid.Children.Add(v, 0, 20, 18, 20);
                //    return;
                case PageComponentType.Message:
                    return;
                //case PageComponentType.Player:
                //    grid.Children.Add(v, 2, 10, 13, 18);
                //    return;
            }
            base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
        }
    }
}
