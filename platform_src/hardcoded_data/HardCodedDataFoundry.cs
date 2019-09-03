using System;
using System.Collections.Generic;
using Florine;

namespace FlorineHardCodedData
{
    public class HardCodedDataFoundry : IPlatformFoundry
    {
        GameState _state = new GameState();

        Avatar _playerAvatar = new Avatar();

        public HardCodedDataFoundry()
        {
        }

        /* IPlatformFoundry */
        public GameState LoadGameState() { return _state; }
        public List<Food> LoadFood() { return _foodstuffs; }
        public List<Nutrient> LoadNutrients() { return _nutrients; }
        public IPage GetPage(IPage GenericPage) { return HardCodedPageFromIPage(GenericPage); }

        /* Data Dumps */

        Nutrient Carbs = new Nutrient() { Name = "Carbohydrates", Class = Nutrient.NutrientType.Macro };
        Nutrient Proteins = new Nutrient() { Name = "Proteins", Class = Nutrient.NutrientType.Macro};
        Nutrient Fats = new Nutrient() { Name = "Fats", Class = Nutrient.NutrientType.Macro };
        Nutrient Vitamin_A = new Nutrient() { Name = "A", Class = Nutrient.NutrientType.Vitamin };
        Nutrient Vitamin_B12 =new Nutrient() { Name = "B12", Class = Nutrient.NutrientType.Vitamin };
        Nutrient Vitamin_C = new Nutrient() { Name = "C", Class = Nutrient.NutrientType.Vitamin };


        List<Nutrients> _nutrients = new List<Nutrients>()
        {
            Carbs,
            Proteins,
            Fats,
            Vitamin_A,
            Vitamin_B12,
            Vitamin_C

        };

        List<Food> _foodstuffs = new List<Food>()
        {
            new Food() {
                Name = "Yummy Food",
                Nutrients = new NutrientSet() {
                    new Tuple<Nutrient, int> ( Carbs, 100 ),
                    new Tuple<Nutrient, int> ( Protiens, 50),
                }
            },
            new Food() {
                Name = "Tasty Fodo",
                Nutrients = new NutrientSet() {
                    new Tuple<Nutrient, int> ( Carbs, 200 ),
                }
            },
            new Food() {
                Name = "Healthy Food",
                Nutrients = new NutrientSet() {
                    new Tuple<Nutrient, int> ( Carbs, 150 ),
                }
            },
            new Food() {
                Name = "Other Food",
                Nutrients = new NutrientSet() {
                    new Tuple<Nutrient, int> ( Carbs, 50 ),
                }
            },
        };

        private class HardCodedPage : IPage
        {
            public PageType MainType { get; set; }
            public PageSubType SubType { get; set; }


            public IImage Background { get { return null; } }
            public IGameOptionSet PrimaryOptions { get { return null; } }
            public String Title { get { return MainType.ToString(); } }
            public String Message { get { return null; } }
            public NutrientSet NutrientState { get { return null; } }
            public NutrientSet NutrientDelta { get { return null; } }
        };

        public IPage HardCodedPageFromIPage(IPage generic)
        {
            switch (generic.MainType) {
                case GameState.PageType.Start:
                    return generic;
                case GameState.PageType.Char_Creation:
                    return generic;
                case GameState.PageType.Game_Loader:
                    return generic;
                case GameState.PageType.Day_Intro:
                    return generic;
                case GameState.PageType.Select_Meal:
                    return generic;
                case GameState.PageType.Summarize_Meal:
                    return generic;
                case GameState.PageType.Select_Activity:
                    return generic;
                case GameState.PageType.Summarize_Activity:
                    return generic;
                case GameState.PageType.Summarize_Day:
                    return generic;
                default:
                    return generic;
            }
        }
    }
}
