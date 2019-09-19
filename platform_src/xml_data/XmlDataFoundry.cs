using System;
using System.Collections.Generic;
using Florine;

namespace FlorineXmlData
{
    public class XmlDataFoundry : IPlatformFoundry
    {
        GameState _state = new GameState();

        Avatar _playerAvatar = new Avatar();

        public XmlDataFoundry()
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
        public virtual Activity AutomaticActivity(GameState gs) { return null; }
        /* */
        public virtual IImage LoadImageFromFood(Food Parent)
        {
            return null;
        }
        
        List<Nutrient> _nutrients;
        static List<Food> _foodstuffs;
        
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
			public IGameOptionSet AppliedOptions { get; set; }
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
            public void ImpactPlayer(Player target) { }
			public void AdjustNutrients(NutrientSet n) { }
			public IGameOptionSet SubOptions { get; set; }
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
