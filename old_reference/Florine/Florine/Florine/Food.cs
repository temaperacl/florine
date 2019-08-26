using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace Florine
{
    public class Food
    {
        public String Name;
        public override string ToString()
        {
            return Name;
        }
        public Dictionary<string, int> Macronutrients = new Dictionary<string, int>();
        public Dictionary<string, int> Vitamins = new Dictionary<string, int>();
        public int Calories = 0;
        public AspectImage Image = new AspectImage();
        public FlOval OvalDisplay = new FlOval();
        public void AutoPaint(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            this.Image.Draw(args.Surface.Canvas, args.Info.Rect); 
        }
        public static List<Food> Discover(int number)
        {
            List<Food> x = new List<Food>();
            
            foreach(Food f in Foods.Values)
            {
                if (x.Count > number) { return x; }
                x.Add(f);
            }
            return x;
        }
        private static Dictionary<string, SKColor> NutrientColors = new Dictionary<string, SKColor>()
        {
            { "Carbs",   new SKColor(205,0,0) },
            { "Protien", new SKColor(0,205,0) },
            { "Fat",     new SKColor(0,0,205) },
        };

        public static Dictionary<string, Food> Foods = initFoodList();
        private static Dictionary<string, Food> initFoodList()
        {
            Dictionary<string, Food> foodlist = new Dictionary<string, Food>();
            ItemType foods = ItemType.FromCSV(ResourceLoader.LoadTextFile("Florine.Data.Foods.csv"));
            List<String> FoodTypes = foods.Items;
            
            foreach (string food in FoodTypes)
            {
                string resourceID = "Florine.Data.Img.Breakfast_" + food + ".png";
                Food curFood = new Food();
                curFood.Name = food;
                curFood.Image.baseImage = SKImage.FromBitmap(ResourceLoader.LoadBitmap(resourceID));
                curFood.OvalDisplay.mainImage = curFood.Image.baseImage;

                curFood.Calories = foods[food].Values["Calories"];
                foreach(string macroNutrient in new List<String>() {
                    "Carbs",
                    "Protien",
                    "Fat"
                })
                {
                    int amount = foods[food].Values[macroNutrient];
                    curFood.Macronutrients[macroNutrient] = amount;
                    curFood.OvalDisplay.innerRing.Add(new Tuple<float, SKColor>(
                        (float)amount,
                        NutrientColors[macroNutrient]
                    ));
                }
                foodlist[food] = curFood;
            }
            return foodlist;
        }
        public int this[string key]
        {
            get
            {
                string lkey = key.ToLower();
                if(key == "calories") { return this.Calories; }
                int value = 0;
                if (this.Macronutrients.TryGetValue(lkey,out value))
                {
                    return value;
                }
                if (this.Vitamins.TryGetValue(lkey, out value))
                {
                    return value;
                }
                return 0;
            }
        }

    }
}

