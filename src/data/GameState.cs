using System;
using System.Collections.Generic;

namespace Florine
{
    public class GameState
    {
        // Data
        public Florine.Player Player { get; set; }  
        public NutrientSet DailyDelta 
        {
            get
            {
               if(null == _currentPage) { return null; }
               return _currentPage.NutrientState;
            }
            set
            {
               if(null == _currentPage) { return; }
               _currentPage.NutrientState = value;
            }
        }

        public NutrientSet CurrentDelta 
        { 
            get
            {
               if(null == _currentPage) { return null; }
               return _currentPage.NutrientDelta;
            }
            set
            {
               if(null == _currentPage) { return; }
               _currentPage.NutrientDelta = value;
            }
        }
		
        private GameState.DataPage _currentPage { get; set; }
        public int HoursLeftInDay { get; set; }
        public GameState()
        {
			Player = new Florine.Player();
			DailyDelta = new Florine.NutrientSet();
			CurrentDelta = new Florine.NutrientSet();
                        _currentPage = new GameState.DataPage();			
        }


        // Functions 
        public Florine.IPage CurrentPage {
            get {
                return _currentPage;
            }
            set {				
                    _currentPage = new DataPage(value);
            }
        }

        public void ReadyNextPage()
        {
            FTrack.Track();
            CurrentDelta = new NutrientSet();			
			if(_currentPage.MainType == PageType.Day_Intro) {
				DailyDelta = new NutrientSet();				
			}								
			_currentPage.AppliedOptions = null;
        }
		
        public void ApplyOption(IGameOption option) {
            FTrack.Track();
            // To Start, we just have the player to apply stuff to.
            if (Player != null) {
                Player.ApplyOption(option);								
            }
						
			if(CurrentDelta != null) {			
				CurrentDelta.ApplyOption(option);
			}
						
			if(DailyDelta != null) {
				DailyDelta.ApplyOption(option);
			}
			
			if(null == _currentPage.AppliedOptions) {
				_currentPage.AppliedOptions = new AppliedOptionSet();
			}
			_currentPage.AppliedOptions.Add(option);
                        //_currentPage.NutrientState = Player.Nutrients;
        }

        public IPage SetPage(PageType newMainType, PageSubType newSubType) {            
            _currentPage.MainType = newMainType;
            _currentPage.SubType = newSubType;	
			
			if(null != Player) {
				int HoursPassed = 0;
				switch(newMainType) {
					case PageType.Start:              HoursPassed = 0; break;
					case PageType.Char_Creation:      HoursPassed = 0; break;
					case PageType.Game_Loader:        HoursPassed = 0; break;
					case PageType.Day_Intro:          
						HoursPassed = HoursLeftInDay; 
						HoursLeftInDay = 24 + HoursPassed;
						break;
					case PageType.Select_Meal:        
						switch(newSubType) {
							case PageSubType.Breakfast: HoursPassed = 7; break;
							case PageSubType.Lunch:     HoursPassed = 0; break;
							case PageSubType.Dinner:    HoursPassed = 0; break;
							case PageSubType.Daily:		HoursPassed = 0; break;
						}
						break;
					case PageType.Summarize_Meal:     HoursPassed = 1; break;
					case PageType.Select_Activity:    HoursPassed = 0; break;
					case PageType.Summarize_Activity:
						switch(newSubType) {
							case PageSubType.Breakfast: HoursPassed = 3; break;
							case PageSubType.Lunch:     HoursPassed = 5; break;
							case PageSubType.Dinner:    HoursPassed = 4; break;
							case PageSubType.Daily:		HoursPassed = 0; break;
						}
						break;
					case PageType.Summarize_Day:      HoursPassed = 0; break;
				}
				Player.Tick(HoursPassed);
				HoursLeftInDay -= HoursPassed;
				if(newMainType == PageType.Day_Intro) { Player.StartNewDay(); }
			}
            return _currentPage;
        }

        // Support
        public enum PageType {
            Start,
            Char_Creation,
            Game_Loader,
            Day_Intro,
            Select_Meal,
            Summarize_Meal,
            Select_Activity,
            Summarize_Activity,
            Summarize_Day
        };
        public enum PageSubType {
            None,
            Setup,
            Daily,
            Breakfast,
            Lunch,
            Dinner
        };
		
        private class AppliedOptionSet : List<IGameOption>, IGameOptionSet
        {	
            public int SelectionLimit { get { return 0; } }
            public IGameOption Finalizer { get { return null; } }    
        };
		
        private class DataPage : IPage {
            public DataPage() { }
            public DataPage(IPage source) {
                    MainType = source.MainType;
                    SubType = source.SubType;
                    AppliedOptions= source.AppliedOptions;				
            }
            public PageType MainType { get; set; }
            public PageSubType SubType { get; set; }
			
			public IGameOptionSet AppliedOptions { get; set; }

            public IImage Background { get { return null; } }
            public IGameOptionSet PrimaryOptions { get { return null; } }
            public String Title { get { return MainType.ToString(); } }
            public String Message { 
				get {					
						if(null == AppliedOptions) { return null; }
						string _n = "";					
						foreach(IGameOption opt in AppliedOptions) 
						{
							if(opt is Activity)
							{
								_n += "\n" + ((Activity)opt).Description;
							}							
						}
					    if(_n.Length < 1) { return null; }
						return _n;
					} 
			}
            public NutrientSet NutrientState { get; set; }
            public NutrientSet NutrientDelta { get; set; }
        };

    }
}
