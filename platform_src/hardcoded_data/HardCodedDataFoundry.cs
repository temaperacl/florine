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
        
        public virtual GameState LoadGameState() { return _state; }

        public virtual IList<Food> LoadFood() {
            foreach (Food f in _foodstuffs)
            {
                f.OptionPicture = LoadImageFromFood(f);
            }
            return _foodstuffs;
        }
        public virtual IList<Nutrient> LoadNutrients() { return _nutrients; }
        public virtual IPage GetPage(IPage GenericPage) { return HardCodedPageFromIPage(GenericPage); }
        public virtual bool SaveGameState(GameState _unused) { return false; }
        /* */
        public virtual IImage LoadImageFromFood(Food Parent)
        {
            return null;
        }
                
        /* Data Dumps */

        static Nutrient Carbs = new Nutrient() { Name = "Carbohydrates", Class = Nutrient.NutrientType.Macro };
        static Nutrient Proteins = new Nutrient() { Name = "Proteins", Class = Nutrient.NutrientType.Macro};
        static Nutrient Fats = new Nutrient() { Name = "Fats", Class = Nutrient.NutrientType.Macro };
        static Nutrient Vitamin_A = new Nutrient() { Name = "A", Class = Nutrient.NutrientType.Vitamin };
        static Nutrient Vitamin_B12 =new Nutrient() { Name = "B12", Class = Nutrient.NutrientType.Vitamin };
        static Nutrient Vitamin_C = new Nutrient() { Name = "C", Class = Nutrient.NutrientType.Vitamin };


        List<Nutrient> _nutrients = new List<Nutrient>()
        {
            Carbs,
            Proteins,
            Fats,
            Vitamin_A,
            Vitamin_B12,
            Vitamin_C

        };

        static List<Food> _foodstuffs = new List<Food>()
        {
            new Food() {
                Name = "Cereal",
                Nutrients = new NutrientSet() {
                    { Carbs, 100 },
                    {  Proteins, 50 },
                }
            },
            new Food() {
                Name = "Eggs",
                Nutrients = new NutrientSet() {
                    { Carbs, 200 },
                }
            },
            new Food() {
                Name = "Fruit",
                Nutrients = new NutrientSet() {
                    { Carbs, 150 },
                }
            },
            new Food() {
                Name = "Pancakes",
                Nutrients = new NutrientSet() {
                    { Carbs, 50 },
                }
            },
            new Food() {
                Name = "Poptarts",
                Nutrients = new NutrientSet() {
                    { Carbs, 550 },
                }
            },
            new Food() {
                Name = "Toast",
                Nutrients = new NutrientSet() {
                    { Fats, 150 },
                }
            },
        };
        
        private class HardCodedPage : IPage
        {
            public HardCodedPage(IPage source)            
            {
                MainType = source.MainType;
                SubType = source.SubType;
            }
            public GameState.PageType MainType { get; set; }
            public GameState.PageSubType SubType { get; set; }


            public IImage Background { get; set; }
            public IGameOptionSet PrimaryOptions { get; set; }
            public String Title { get; set; }
            public String Message { get; set; }
            public NutrientSet NutrientState { get { return null; } }
            public NutrientSet NutrientDelta { get { return null; } }
        };
        private class HardCodedOptionSet : List<IGameOption>, IGameOptionSet
        {
            public int SelectionLimit { get; set; }
            public IGameOption Finalizer { get; set; }
        }

        protected class HardcodedEmptyOption : IGameOption
        {
            public HardcodedEmptyOption(string name) { OptionName = name; }
            public IImage Picture { get { return null; } }
            public String OptionName { get; set; }
            public void AdjustNutrients(NutrientSet target) { }
        }
        
        private static HardcodedEmptyOption _emptyOption(string name)
        {
            return new HardcodedEmptyOption(name);
        }

        public IPage HardCodedPageFromIPage(IPage generic)
        {
            HardCodedPage hcPage = new HardCodedPage(generic);
            switch (generic.MainType) {
                case GameState.PageType.Start:
                    hcPage.Title = "Florine Game";
                    hcPage.Message = "Welcome to Florine where you will guide a fairy through her daily life.";
                    //hcPage.Background = "Start_Page";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("Start")
                    };
                    break;
                case GameState.PageType.Char_Creation:
                    // Cheat
                    hcPage.Title = "Character Creation";                    
                    //hcPage.Background = "Char_Creation";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("Continue")
                    };
                    break;
                case GameState.PageType.Game_Loader:
                    //hcPage.Title = "...";
                    //hcPage.Message = "Choose up to 2";
                    //hcPage.Background = "Start_Page";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("Continue")
                    };
                    break;
                case GameState.PageType.Day_Intro:
                    hcPage.Title = "A New Day!";
                    hcPage.Message = "Welcome to a new day. Let's see what today holds!";
                    //hcPage.Background = "Start_Page";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("Start Day")
                    };
                    break;
                case GameState.PageType.Select_Meal:
                    //hcPage.Title = "Load Game";
                    
                    HardCodedOptionSet PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("Done"),
                        SelectionLimit = 2
                    };
                    //Background
                    switch (generic.SubType) {
                        case GameState.PageSubType.None:
                            break;
                        case GameState.PageSubType.Setup:
                            break;
                        case GameState.PageSubType.Daily:
                            break;
                        case GameState.PageSubType.Breakfast:
                            PrimaryOptions.SelectionLimit = 2;
                            break;
                        case GameState.PageSubType.Lunch:
                            PrimaryOptions.SelectionLimit = 3;
                            break;
                        case GameState.PageSubType.Dinner:
                            PrimaryOptions.SelectionLimit = 3;
                            break;
                    }                    
                    
                    for (int idx = 0; idx < _foodstuffs.Count && idx < 6; ++idx) {
                        PrimaryOptions.Add(_foodstuffs[idx].GetOption());
                    }
                    hcPage.Message = "Choose Up To " + PrimaryOptions.SelectionLimit.ToString();
                    hcPage.PrimaryOptions = PrimaryOptions;
                    break;
                case GameState.PageType.Summarize_Meal:
                    hcPage.Title = "Results [Day update TBD]";
                    //hcPage.Message = "We shouldn't be seeing this.";
                    //hcPage.Background = "Start_Page";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("")
                    };                    
                    break;
                // TODO: Summarize Meal is actually 2 pages.
                case GameState.PageType.Select_Activity:
                    hcPage.Title = "Select Activity";
                    hcPage.Message = "TBD";
                    //hcPage.Background = "Start_Page";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("Continue")
                    };
                    break;
                case GameState.PageType.Summarize_Activity:
                    hcPage.Title = "Activity summary";
                    hcPage.Message = "[TBD]";
                    //hcPage.Background = "Start_Page";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("Continue")
                    };
                    break;
                case GameState.PageType.Summarize_Day:
                    hcPage.Title = "Daily Results";                    
                    //hcPage.Background = "Start_Page";
                    hcPage.PrimaryOptions = new HardCodedOptionSet()
                    {
                        Finalizer = _emptyOption("")
                    };
                    break;
                default:
                    hcPage.Title = "== Florine ==";
                    hcPage.Message = "Unexpected Value " + generic.MainType.ToString();                                        
                    break;
            }
            return hcPage;
        }
    }
}
