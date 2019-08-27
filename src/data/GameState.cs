using System;
namespace Florine
{
    public class GameState
    {
        public Florine.Player Player { get; set; }
        public Florine.IPage CurrentPage { get; set; }
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


    }
}
