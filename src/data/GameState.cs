using System;
namespace Florine
{
    public class GameState
    {
        // Data
        public Florine.Player Player { get; set; }
        private GameState.DataPage _currentPage { get; set; }

        public GameState()
        {
            _currentPage = new GameState.DataPage();
        }


        // Functions 
        public Florine.IPage CurrentPage {
            get {
                return _currentPage;
            }
        }

        public void ApplyOption(IGameOption option) {
            // To Start, we just have the player to apply stuff to.
            if (Player != null) {
                Player.ApplyOption(option);
            }
        }

        public IPage SetPage(PageType newMainType, PageSubType newSubType) {
            _currentPage.MainType = newMainType;
            _currentPage.newSubType = newMainType;
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
        
        private class DataPage : IPage {
            public PageType MainType { get; set; }
            public PageSubType { get; set; }
        };

    }
}
