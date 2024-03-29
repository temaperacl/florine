using System;
using System.Collections.Generic;
using Florine;

namespace FlorineHardCodedData
{
    public class HardCodedDataFoundry : IPlatformFoundry
    {
        GameState _state = new GameState();
        static Random r = new Random();
        Avatar _playerAvatar = new Avatar();

        public HardCodedDataFoundry()
        {
        }

        /* IPlatformFoundry */

        public virtual GameState LoadGameState() { return _state; }

        public virtual IList<Food> LoadFood() {
            foreach (KeyValuePair<string, List<NutrientAmount>> kvp in FoodTable) {
                List<NutrientAmount> l = kvp.Value;
                _foodIdx[kvp.Key] = _foodIdx.Count;
                NutrientAmount n = l[12];
                string nDesc = kvp.Key;
                switch (kvp.Key) {
                    case "White Toast":
                        nDesc += "\n\nA couple slices of plain white toast";
                        break;
                    case "Wheat Toast":
                        nDesc += "\n\nTwo slices of wheat toast";
                        break;
                    case "Fruit": nDesc += "\n\nAssorted fresh fruit: a banana, an orange, and a cup of grapes"; break;
                    case "Pancakes": nDesc += "\n\nA stack of four buttery, syrupy pancakes"; break;
                    case "Meatball Sub": nDesc += "\n\nA 6\" sub loaded with meatballs and tomato sauce "; break;
                    case "Hamburger": nDesc += "\n\nA quarter-pound cheeseburger with lots of toppings"; break;
                    case "Instant Noodles": nDesc += "\n\nA cup of instant noodles"; break;
                    case "Pepperoni Pizza Slice": nDesc += "\n\nA single slice from a medium pepperoni pizza"; break;
                    case "Vegetable Soup": nDesc += "\n\nA small bowl of soup, loaded with veggies"; break;
                    case "Spaghetti": nDesc += "\n\nA big plate of spaghetti with vegetarian tomato sauce"; break;
                    case "Spaghetti w/ Meatballs": nDesc += "\n\nA big plate of spaghetti with chunky tomato sauce and meatballs"; break;
                    case "Sushi Serving": nDesc += "\n\nSeaweed wrapped around white rice and raw tuna"; break;
                    case "Taco w/ meat": nDesc += "\n\nA medium, meat-filled taco"; break;
                    case "Tacos": nDesc += "\n\nA couple of cruchy tacos loaded with meat"; break;
                    case "Chocolate Ice Cream": nDesc += "\n\nThree scoops of chocolate ice cream. No toppings!"; break;
                    case "Donuts": nDesc += "\n\nA couple of fancy donuts"; break;
                    case "Cinnamon Roll": nDesc += "\n\nA warm, medium-sized glazed cinnamon roll"; break;
                    case "Hamburger Combo": nDesc += "\n\nA medium burger, fries, and soda"; break;
                    case "Turkey Sandwich": nDesc += "\n\nA simple turkey sandwich with lettuce, tomato, and mayo"; break;
                    case "Sandwich with Chips": nDesc += "\n\nA simple turkey sandwich with lettuce, tomato, and mayo. Comes with a side of chips!"; break;
                    case "Salad": nDesc += "\n\nA plain mixed greens salad with tomatoes and carrots"; break;
                    case "Fruit Smoothie": nDesc += "\n\nA medium yogurt-based smoothie with lots of different fruits"; break;

                }
                _foodstuffs.Add(new Food() {
                    Name = kvp.Key,
                    Nutrients = new NutrientSet() {
                        {Proteins,   l[0] * n},
                        {Carbs,      l[1] * n},
                        {Fats,       l[2] * n},
                        {Fiber,      l[3] * n},
                        {Folic_Acid, l[5] * n},
                        {Vitamin_D,  l[6] * n},
                        {Calcium,    l[7] * n},
                        {Iron,       l[8] * n},
                        {Potassium,  l[9] * n},
                        {Vitamin_B12,l[10] * n},
                        {Vitamin_A,  l[11] * n}
                    },
                    Description = nDesc // Food Description
                });
            }

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

        public virtual Activity AutomaticActivity(GameState gs)
        {
            return _onlyPath.GetActivity(gs);
        }

        /* Data Dumps */

        static Nutrient Carbs = new Nutrient()
        {
            Name = "Carbohydrates",
            Class = Nutrient.NutrientType.Macro,
            Units = NutrientUnit.g,
            DailyTarget = 130
        };
        static Nutrient Proteins = new Nutrient()
        {
            Name = "Protein",
            Class = Nutrient.NutrientType.Macro,
            Units = NutrientUnit.g,
            DailyTarget = 46
        };
        static Nutrient Fats = new Nutrient()
        {
            Name = "Fat",
            Class = Nutrient.NutrientType.Macro,
            Units = NutrientUnit.g,
            DailyTarget = 57
        };
        static Nutrient Fiber = new Nutrient()
        {
            Name = "Fiber",
            Class = Nutrient.NutrientType.Macro,
            Units = NutrientUnit.g,
            DailyTarget = 28
        };

        static Nutrient Vitamin_A = new Nutrient()
        {
            Name = "Vitamin A",
            Class = Nutrient.NutrientType.Vitamin,
            Units = NutrientUnit.mg,
            DailyTarget = 700
        };
        static Nutrient Vitamin_B12 = new Nutrient()
        {
            Name = "Vitamin B12",
            Class = Nutrient.NutrientType.Vitamin,
            Units = NutrientUnit.mcg,
            DailyTarget = 2.4
        };
        static Nutrient Potassium = new Nutrient()
        {
            Name = "Potassium",
            Class = Nutrient.NutrientType.Vitamin,
            Units = NutrientUnit.mcg,
            DailyTarget = 4700
        };
        static Nutrient Iron = new Nutrient() {
            Name = "Iron",
            Class = Nutrient.NutrientType.Vitamin,
            Units = NutrientUnit.mg,
            DailyTarget = 18
        };
        static Nutrient Calcium = new Nutrient() {
            Name = "Calcium",
            Class = Nutrient.NutrientType.Vitamin,
            Units = NutrientUnit.mg,
            DailyTarget = 1000
        };
        static Nutrient Vitamin_D = new Nutrient() {
            Name = "Vitamin D",
            Class = Nutrient.NutrientType.Vitamin,
            //Units = NutrientUnit.IU,
            //DailyTarget = 600
            Units = NutrientUnit.mcg,
            DailyTarget=600 * .025
        };
        static Nutrient Folic_Acid = new Nutrient() {
            Name = "Folic Acid",
            Class = Nutrient.NutrientType.Vitamin,
            Units = NutrientUnit.mcg,
            DailyTarget = 400
        };


        List<Nutrient> _nutrients = new List<Nutrient>()
        {
            Proteins,
            Carbs,
            Fats,
            Fiber,
            Folic_Acid,
            Vitamin_D,
            Calcium,
            Iron,
            Potassium,
            Vitamin_B12,
            Vitamin_A
        };

        public class EndDayActivity : Activity
        {
            public override void ImpactPlayer(Player target)
            {
                target.ReadyToEndDay = true;
                base.ImpactPlayer(target);
            }
        }

        public class HardCodedPayActivity : Activity
        {
            
            public override void ImpactPlayer(Player target)
            {
                target.Money += Pay;
                base.ImpactPlayer(target);
            }
        }

        public class HardCodedActivityPath : BasicActivityPath
        {
            public override Florine.Activity ResolveActivityForGameState(GameState gs)
            {
                switch (gs.CurrentPage.MainType)
                {
                    case GameState.PageType.Summarize_Meal:
                        switch (gs.CurrentPage.SubType) {
                            case GameState.PageSubType.Lunch:
                                if (Day == 1)
                                {
                                    gs.Player.Happiness = 378;
                                    gs.Player.Energy = 60;
                                    gs.Player.Focus = 70;
                                }
                                else if (Day == 2)
                                {
                                    gs.Player.Happiness += 802;
                                    gs.Player.Energy = 100;
                                    gs.Player.Focus = 100;
                                }
                                break;
                        }
                        break;
                    case GameState.PageType.Summarize_Activity:
                        if (gs.CurrentPage.SubType == GameState.PageSubType.Breakfast)
                        {
                            if (Day == 1)
                            {
                                return new Activity()
                                {
                                    Impact = new NutrientSet(),
                                    OptionName = "At Work",
                                    Description = gs.Player.Name + " is doing well at work this morning. She completed 27 measurements!",
                                };
                            }
                            else
                            {
                                return new Activity()
                                {
                                    Impact = new NutrientSet(),
                                    OptionName = "At Work",
                                    Description = gs.Player.Name + " had a wonderful morning! She did a great job training a new coworker.",
                                };
                            }
                        }
                        else if (gs.CurrentPage.SubType == GameState.PageSubType.Lunch)
                        {
                            int nPay = 0;
                            string WorkDesc = gs.Player.Name + " worked today";
                            switch(Day) {
                                case 1:
                                    gs.Player.Energy = 30;
                                    gs.Player.Focus = 20;
                                    WorkDesc = gs.Player.Name + " really just wanted a nap this afternoon.";
                                    break;
                                case 2:
                                    WorkDesc = gs.Player.Name + " was on fire today! She wrangled six dragonflies.";
                                    gs.Player.Energy = 60;
                                    gs.Player.Focus = 80;
                                    break;
                            } 
                            switch (Day) {
                                case 1:
                                    nPay = 3;
                                    break;
                                case 2:
                                    nPay = 5;
                                    break;
                                default:
                                    nPay = r.Next(2, 6);
                                    break;
                            }
                            return new HardCodedPayActivity()
                            {
                                Impact = new NutrientSet(),
                                OptionName = "At Work",
                                Description = WorkDesc,
                                Pay = nPay
                            };
                        }
                        else if (gs.CurrentPage.SubType == GameState.PageSubType.Daily)
                        {
                            if (gs.CurrentPage.AppliedOptions.Count == 0
                                || gs.CurrentPage.AppliedOptions[0].SubOptions == null
                                || gs.CurrentPage.AppliedOptions[0].SubOptions.Count == 0)
                            {
                                return new EndDayActivity()
                                {
                                    Impact = new NutrientSet(),
                                    OptionName = "Bed Time",
                                    Description = "After a nice evening at home, " + gs.Player.Name + " feels renewed "
                                };
                            }
                            gs.Player.Tick(2);
                            switch (gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName)
                            {
                                case "Cooking":
                                    return new Activity()
                                    {
                                        OptionName = gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName,
                                        Description = "== Coming Soon ==",
                                        Impact = new NutrientSet()
                                    };                                    
                                case "Dancing":
                                    return new Activity()
                                    {
                                        OptionName = gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName,
                                        Description = "== Coming Soon ==",
                                        Impact = new NutrientSet()
                                    };
                                case "Gym":
                                    return new Activity()
                                    {
                                        OptionName = gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName,
                                        Description = "== Coming Soon ==",
                                        Impact = new NutrientSet()
                                    };
                                case "Home":
                                    return new EndDayActivity()
                                    {
                                        OptionName = gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName,
                                        Description = "After a nice evening at home, " + gs.Player.Name + " feels renewed ",
                                        Impact = new NutrientSet()
                                    };
                                case "Social":
                                    return new Activity()
                                    {
                                        OptionName = gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName,
                                        Description = gs.Player.Name + " spends a fun night with her friends",
                                        Impact = new NutrientSet()
                                    };
                                case "Studying":
                                    return new Activity()
                                    {
                                        OptionName = gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName,
                                        Description = "== Coming Soon ==",
                                        Impact = new NutrientSet()
                                    };
                                case "Shopping":
                                    return new Activity()
                                    {
                                        OptionName = gs.CurrentPage.AppliedOptions[0].SubOptions[0].OptionName,
                                        Description = "== Coming Soon ==",
                                        Impact = new NutrientSet()
                                    };                                    
                            }
                            return null;
                        }
                        break;                        
                    case GameState.PageType.Select_Activity:
                        break;
                }

                //Select_Activity
                return null;
            }
        }

        public IGameOptionSet GetDailyActivities()
        {
            HardCodedOptionSet activities = new HardCodedOptionSet()
            {
                SelectionLimit = 1,
                Finalizer = new EndActivity() {
                    Impact = new NutrientSet(),
                    OptionName = "Choose"                    
                }
            };

            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Cooking",
                    Description = "Cooking\n\nPrepare a meal for tomorrow",
                }
            );            
            /*
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Dancing",
                    Description = "A Night out on the town!",
                }
            );
            */
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Gym",
                    Description = "Gym\n\nWorking out at the gym",
                }
            );
            activities.Add(
                new EndActivity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Home",
                    Description = "Stay Home\n\nEat dinner and end the day",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Social",
                    Description = "Socialize\n\nTalking and meeting up with friends",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Studying",
                    Description = "Study\n\nLearn and grow",
                }
            );            
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Shopping",
                    Description = "Shopping\n\nCheck out the latest fashions and recipes",
                }
            );

            return activities;
        }

        public class EndActivity : Activity
        {
            public override void ImpactPlayer(Player target)
            {
                target.Tick(23- (int)target.HoursIntoDay);
            }
        }

		static HardCodedActivityPath _onlyPath = new HardCodedActivityPath();
		public bool GetNextGameState(GameState CurrentState,
                          IGameOption selectedOpt,
                          out GameState.PageType nextType,
						  out GameState.PageSubType nextSubType
						  ) {
			nextType = CurrentState.CurrentPage.MainType;
			nextSubType = CurrentState.CurrentPage.SubType;

			switch(CurrentState.CurrentPage.MainType) {
				case GameState.PageType.Start:
                    // TODO: Actual Switch
                    //nextType = GameState.PageType.Char_Creation;
                    //nextSubType = GameState.PageSubType.Setup;
                    nextType = GameState.PageType.Day_Intro;
                    nextSubType = GameState.PageSubType.Daily;
                    return true;
				case GameState.PageType.Char_Creation:
					nextType = GameState.PageType.Day_Intro;
					nextSubType = GameState.PageSubType.Daily;
					return true;
				case GameState.PageType.Day_Intro:
					nextType = GameState.PageType.Select_Meal;
					nextSubType = GameState.PageSubType.Breakfast;
					return true;
				case GameState.PageType.Select_Meal:
					nextType = GameState.PageType.Summarize_Meal;
					return true;
				case GameState.PageType.Summarize_Meal:
					nextType = GameState.PageType.Summarize_Activity;
                    //if (CurrentState.CurrentPage.SubType == GameState.PageSubType.Dinner)
                    //{
                    //    nextType = GameState.PageType.Select_Activity;
                    //    nextSubType = GameState.PageSubType.Daily;
                    //}
                    return true;
				case GameState.PageType.Select_Activity:
                    if (selectedOpt == null 
                        || selectedOpt.SubOptions.Count == 0
                        || selectedOpt.SubOptions[0].OptionName == "Home")
                    {
                        nextType = GameState.PageType.Select_Meal;
                        nextSubType = GameState.PageSubType.Dinner;
                    }
                    else
                    {
                        nextType = GameState.PageType.Summarize_Activity;
                        nextSubType = GameState.PageSubType.Daily;
                    }
					return true;
				case GameState.PageType.Summarize_Activity:
					switch(CurrentState.CurrentPage.SubType) {
						case GameState.PageSubType.Breakfast:
							nextType = GameState.PageType.Select_Meal;
							nextSubType = GameState.PageSubType.Lunch;
							return true;
						case GameState.PageSubType.Dinner:
                            nextType = GameState.PageType.Day_Intro;
                            nextSubType = GameState.PageSubType.Daily;
                            return true;
						case GameState.PageSubType.Lunch:
                            nextType = GameState.PageType.Select_Activity;
                            nextSubType = GameState.PageSubType.Daily;
                            
							return true;
						case GameState.PageSubType.Daily:
                            // Loop if possible
                            if (
                                false == CurrentState.Player.ReadyToEndDay
                            )
                            {
                                nextType = GameState.PageType.Select_Activity;
                                nextSubType = GameState.PageSubType.Daily;
                                return true;
                            }
                            nextType = GameState.PageType.Select_Meal;
                            nextSubType = GameState.PageSubType.Dinner;
                            
							return true;
					}
					break;
				case GameState.PageType.Summarize_Day:
					nextType = GameState.PageType.Day_Intro;
					nextSubType = GameState.PageSubType.Daily;
					return true;
			}
			return false;
		}

		static Dictionary<string, List<NutrientAmount>> FoodTable =
			new Dictionary<string, List<NutrientAmount>>()
        {
                {"White Toast",       new List<NutrientAmount>() { 2.25, 13.6, 1,     0.725,72.4,26,0,29.8,0.832,32.8,0.005,0,2} },
                {"Grilled Cheese",    new List<NutrientAmount>() { 5   , 71,   9,     2,385,0,0,0,1.8,0,0,0,1} },
                {"Pancakes",          new List<NutrientAmount>() { 8.36, 113.3,10.986,1.6,585.514,102.4,0,126.26,9.146,146.19,4.48,725,1} },
                //{"Fruit",             new List<NutrientAmount>() { 1.03, 19.4, 0.3,   2.34,84.33,22.19,0,23.73,0.23,278.3,0,7.51,1} },
                {"Fruit",             new List<NutrientAmount>() { 3.1, 58.2, 0.868,7.038,253.012,66.58,0,71.2,.705,835,0,22.54,1} },
                {"Eggs",              new List<NutrientAmount>() { 22  , 3.54, 24.2,  0,319.96,79.2,3.96,145,2.88,290,1.67,354,2} },
                {"Cereal",            new List<NutrientAmount>() { 3.99, 24.2, 2.22,  3.1,132.74,236,1.12,132,10.9,212,2.23,327,1} },
                {"Toaster Pastry",    new List<NutrientAmount>() { 2.09, 15.1, 2.73,  0.4,93.3,25.6,0,31.2,2.27,36,1.12,181,2 } },
                {"Wheat Toast",       new List<NutrientAmount>() { 3.11, 13.4, 1.02,  1.13,75.22,20.6,0,39.6,0.982,53.5,0,0,2 } },
                {"Multigrain Toast",  new List<NutrientAmount>() { 4.79, 15.5, 1.52,  2.67,94.84, 23.1,0, 36.6,0.898, 82.5,0,   0,2 } },
                {"Mexican Rice",      new List<NutrientAmount>() { 7.85,41.09,14.42,  4.55,325.6,61.56,0,23.46, 1.37,475.1,0,51.7,1 } },
                {"Strawberry Yogurt", new List<NutrientAmount>() { 8.38, 23.5, 2.12,  0,146.6,18.7,1.7,291,.119,372,0.901,73.1,1 } },
                {"Eggs, Bacon, and Toast",
                    new List<NutrientAmount>() { 29.96, 17.276,28.01,0.725,429.034,105.2,3.992,175.68,3.788,362.7,1.762,354.88,1 } },

                {"Chocolate Ice Cream",
                    new List<NutrientAmount>() { 2.51, 18.6, 7.26, .792, 149.78, 10.6, .132, 71.9, .614, 164, .191, 275, 3} },
                {"Instant Noodles",
                    new List<NutrientAmount>() { 7,41,11,3,291,0,0,19.8,2.7,0,0,135,1 } },
                {"Lamb Chops",
                    new List<NutrientAmount>() { 22.2, 0, 20.4, 0, 272.4, 16, 0.089, 17.8, 1.6, 288, 2.18, 0, 1 } },
                {"Meatball Sub",
                    new List<NutrientAmount>() { 20.4, 54.4, 17.7, 4.39, 458.5, 165, 0, 368, 4.85, 575, 1.15, 610, 1 } },
                {"Peanut Butter & Jelly",
                    new List<NutrientAmount>() { 10.2, 42.4, 14, 2.79, 336.4, 81.8, 0, 92.1, 2.44, 209, 0, 0, 1 } },
                {"Pepperoni Pizza Slice",
                    new List<NutrientAmount>() { 10.3, 28.1, 10.5, 2.02, 248.1, 81, 0, 135, 2.22, 172, 0.44, 61.6, 1 } },
                {"Vegetable Soup",
                    new List<NutrientAmount>() { 4.26, 14.6, 2.04, 2.81, 93.8, 30.4, 0, 32.8, 1.12, 424, 0.094, 86.6, 1 } },
                {"Spaghetti",
                    new List<NutrientAmount>() { 8.93, 47.5, 7.14, 4.46, 289.98, 107, 0, 39.7, 2.48, 434, 0, 37.2, 3} },
                {"Spaghetti w/ Meatballs",
                    new List<NutrientAmount>() { 14.3, 42.7, 11.1, 3.97, 327.9, 96.7, 0, 42.2, 2.7, 471, .372, 34.7, 3 } },
                {"Sushi Serving",
                    new List<NutrientAmount>() { 2.22, 4.59, 0.075, .21, 27.915, .9, .12, 1.2, .093, 35.7, .153, 2.4, 6 } },
                {"Tacos",
                    new List<NutrientAmount>() { 12, 22.2, 16.5, 3.1, 285.3, 28.8, 0, 117, 1.63, 316, 0.8282, 52.9, 2 } },
                {"Donuts",
                    new List<NutrientAmount>() { 7.78, 48.4, 31.1, 1.355, 504.62, 80.2, 0, 48.2, 2, 113.9, 0.227, 38.5, 1 } },

                {"Hamburger",
                    new List<NutrientAmount>() { 29, 39.7,28.3,2.79,529.5,101,.362,287,4.18,436,2.51,278.5,1 } },
                {"Cinnamon Roll",
                    new List<NutrientAmount>() { 4,43.7,23.9,1.08,405.9,64.8,0,165,1.23,91.8,.114,0,1 } },
                {"Hamburger Combo",
                    new List<NutrientAmount>() { 32.78,138,44.18,6.18,1080.74,101,0,312.02,5.136,1007.6,2.51,278.5,1 } },
                {"Turkey Sandwich",
                    new List<NutrientAmount>() { 19.4,36.5,18.7,2.4,391.9,76.8,.24,120,3.17,487,.864,21.6,1 } },
                {"Sandwich with Chips",
                    new List<NutrientAmount>() { 21.39,60.3,33.9,3.65,631.86,84.97,.24,127.31,3.514,761,.864,21.6,1 } },
                {"Salad",
                    new List<NutrientAmount>() { .896,3.74,.165,1.39,20.029,49.6,0,20.9,.565,191,0,952,1 } },
                {"Fruit Smoothie",
                    new List<NutrientAmount>() { 19.8,111,8.38,10.4,598.62,86.4,4.32,657,1.9,1470,2.16,156,1 } },
        };
        List<Food> _foodstuffs = new List<Food>();
        Dictionary<string, int> _foodIdx = new Dictionary<string, int>();

		private class HardCodedPage : IPage
		{
			public HardCodedPage(IPage source)
			{
				MainType = source.MainType;
				SubType = source.SubType;
				AppliedOptions = source.AppliedOptions;
				Title = source.Title;
				Message = source.Message;
				if(Message == null) { Message = ""; }
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
        public class NoSelectFoodOption : IGameOption
        {
            public NoSelectFoodOption(string name) { OptionName = name; }
            public IImage Picture { get { return null; } }
            public String OptionName { get; set; }
            public void ImpactPlayer(Player target) { }
            public void AdjustNutrients(NutrientSet nutrients) { }
            public IGameOptionSet SubOptions { get; set; }
            public bool Enabled { get; set; } = true;
        }
        protected class HardcodedEmptyOption : IGameOption
		{
			public HardcodedEmptyOption(string name) { OptionName = name; }
			public IImage Picture { get { return null; } }
			public String OptionName { get; set; }
			public void ImpactPlayer(Player target) { }
			public void AdjustNutrients(NutrientSet nutrients) { }
			public IGameOptionSet SubOptions { get; set; }
            public bool Enabled { get; set; } = true;
		}

		private static HardcodedEmptyOption _emptyOption(string name)
		{
			return new HardcodedEmptyOption(name);
		}

        private static int _Day = 0;
        private static bool HalfMark = false;
        private static int Day
        {
            get { return _Day; }
            set { if (HalfMark) { _Day++; HalfMark = false; } else { HalfMark = true; } }
        }
		public IPage HardCodedPageFromIPage(IPage generic)
		{
			HardCodedPage hcPage = new HardCodedPage(generic);
            switch (generic.MainType) {
                case GameState.PageType.Start:
                /*hcPage.Title = "Florine Game";
                hcPage.Message = "Welcome to Florine where you will guide a fairy through her daily life.";
                //hcPage.Background = "Start_Page";
                hcPage.PrimaryOptions = new HardCodedOptionSet()
                {
                    Finalizer = _emptyOption("Start")
                };
                break;
                */
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
                    if (Day == 0)
                    {
                        hcPage.Message = "Welcome to a new day. Let's see what today holds!";
                    }
                    else  if (_state.Player.Yesterday.Calories > _state.Player.TargetCalories * 1.3)
                    {
                        hcPage.Message = "Tip:\n" + _state.Player.Name + " will earn more happiness points if her Calories finish in the green.";
                    }
                    else if (Iron.RatioRDV(_state.Player.Yesterday.Nutrients["Iron"]) < .5)
                    {
                        hcPage.Message = "Tip:\n" + _state.Player.Name + " can get Iron from meat, peas, and fortified grains.";
                    }
                    else if (Folic_Acid.RatioRDV(_state.Player.Yesterday.Nutrients["Folic Acid"]) < .5)
                    {
                        hcPage.Message = "Tip:\n If " + _state.Player.Name + " is having trouble getting enough Folic Acid, try eating more greens.";
                    } else {
                        switch (Day)
                        {
                            default:
                                hcPage.Message = "Welcome to a new day. Let's see what today holds!";
                                break;
                            //case 2:
                            //    hcPage.Message = _state.Player.Name + " doesn�t need to eat a lot to feel full - try eating more Fiber.";
                            //    break;
                            case 1:
                                hcPage.Message = "Tip:\n" + _state.Player.Name + " will earn more happiness points if her Calories finish in the green.";
                                break;
                            case 2:
                                hcPage.Message = "Tip:\n" + _state.Player.Name + " can get Iron from meat, peas, and fortified grains.";
                                break;
                                //case 3:
                                //    hcPage.Message = "Tip:\n If " + _state.Player.Name + " is having trouble getting enough Folic Acid, try eating more greens.";
                                //    break;
                        }
                    }
                    Day++;
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
                    hcPage.Title = generic.SubType.ToString();
                    
                    //Background
                    switch (generic.SubType) {
                        case GameState.PageSubType.None:
                            break;
                        case GameState.PageSubType.Setup:
                            break;
                        case GameState.PageSubType.Daily:
                            hcPage.Title = "Daily Activity";
                            break;
                        case GameState.PageSubType.Breakfast:
                            PrimaryOptions.SelectionLimit = 2;
                            if (Day == 1)
                            {
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Eggs, Bacon, and Toast"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["White Toast"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Toaster Pastry"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Fruit"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Pancakes"]].GetOption());

                            }
                            else if (Day == 2)
                            {
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Cereal"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Fruit"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Wheat Toast"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Pancakes"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Eggs"]].GetOption());

                            }
                            break;
                        case GameState.PageSubType.Lunch:
                            PrimaryOptions.SelectionLimit = 3;
                            if (Day == 1)
                            {
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Cinnamon Roll"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Hamburger Combo"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Instant Noodles"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Turkey Sandwich"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Tacos"]].GetOption());

                            }
                            else if (Day == 2)
                            {
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Sandwich with Chips"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Hamburger"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Salad"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Fruit Smoothie"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Tacos"]].GetOption());

                            }
                            else
                            {


                                List<string> LunchOptions = new List<string>() {
                                "Hamburger",
                                "Instant Noodles",
                                "Meatball Sub",
                                "Peanut Butter & Jelly",
                                "Pepperoni Pizza Slice",
                                "Vegetable Soup",
                                "Spaghetti",
                                "Spaghetti w/ Meatballs",
                                "Sushi Serving",
                                "Tacos",
                                "Donuts",
                            };
                                for (int i = 0; i < 5; ++i)
                                {
                                    int lOpt = r.Next(LunchOptions.Count);
                                    PrimaryOptions.Add(_foodstuffs[_foodIdx[LunchOptions[lOpt]]].GetOption());
                                    LunchOptions.RemoveAt(lOpt);
                                }
                            }
                            break;
                        case GameState.PageSubType.Dinner:
                            List<string> DinnerOptions = new List<string>() {
                                "Hamburger",
                               
                                "Instant Noodles",
                                "Lamb Chops",
                                
                                "Pepperoni Pizza Slice",
                                "Vegetable Soup",
                                "Spaghetti",
                                "Spaghetti w/ Meatballs",
                                "Sushi Serving",
                                "Tacos",
                                "Donuts",
                            };

                            for (int i = 0; i < 5; ++i)
                            {
                                if (i == 1 && Day == 2)
                                {
                                    PrimaryOptions.Add(_foodstuffs[_foodIdx["Chocolate Ice Cream"]].GetOption());
                                }
                                else if (i == 3 && Day == 2)
                                {
                                    PrimaryOptions.Add(_foodstuffs[_foodIdx["Meatball Sub"]].GetOption());
                                }
                                else
                                {
                                    int lOpt = r.Next(DinnerOptions.Count);
                                    PrimaryOptions.Add(_foodstuffs[_foodIdx[DinnerOptions[lOpt]]].GetOption());
                                    DinnerOptions.RemoveAt(lOpt);
                                }
                            }

                            PrimaryOptions.SelectionLimit = 3;
                            break;
                    }
                    if (PrimaryOptions.Count == 0)
                    {
                        for (int idx = 0; idx < _foodstuffs.Count && idx < 5; ++idx)
                        {
                            PrimaryOptions.Add(_foodstuffs[idx].GetOption());
                        }
                    }
                    PrimaryOptions.Add(new NoSelectFoodOption("Nothing Prepared") { Enabled = false });
                    hcPage.Message +=
						"Choose Up To " + PrimaryOptions.SelectionLimit.ToString();
					hcPage.PrimaryOptions = PrimaryOptions;
					break;
				case GameState.PageType.Summarize_Meal:
					hcPage.Title = hcPage.SubType.ToString() + " Results";
					//hcPage.Message = "We shouldn't be seeing this.";
					//hcPage.Background = "Start_Page";
					hcPage.PrimaryOptions = new HardCodedOptionSet()
					{
						Finalizer = _emptyOption("Continue")
					};
					break;
				// TODO: Summarize Meal is actually 2 pages.
				case GameState.PageType.Select_Activity:
					hcPage.Title = "Evening";
					hcPage.Message = "Select An Activity";
					hcPage.PrimaryOptions = GetDailyActivities();
					break;
				case GameState.PageType.Summarize_Activity:
                    hcPage.Title = generic.SubType.ToString() + " Summary";
                    switch (generic.SubType)
                    {
                        case GameState.PageSubType.Breakfast:
                            hcPage.Title = "Morning Update";
                            break;
                        case GameState.PageSubType.Lunch:
                            hcPage.Title = "Afternoon Update";
                            break;
                        case GameState.PageSubType.Dinner:
                            hcPage.Title = "Daily Summary";
                            break;
                    }
                    if (generic.SubType == GameState.PageSubType.Daily)
                    {
                        if (generic.AppliedOptions.Count > 0)
                        {
                            foreach (IGameOption igo in generic.AppliedOptions)
                            {
                                Activity _act = igo as Activity;
                                if (_act != null)
                                {
                                    hcPage.Title = _act.OptionName;
                                }
                            }
                        }
                        
                    }
					
					//hcPage.Background = "Start_Page";
					hcPage.PrimaryOptions = new HardCodedOptionSet()
					{
						Finalizer = _emptyOption("Continue")
					};
					break;
				case GameState.PageType.Summarize_Day:
					hcPage.Title = "Activity Results";
					//hcPage.Background = "Start_Page";
					hcPage.PrimaryOptions = new HardCodedOptionSet()
					{
						Finalizer = _emptyOption("Continue")
					};
					break;
				default:
					hcPage.Title = "== Florine ==";
					hcPage.Message = "Unexpected Value " + generic.MainType.ToString();
					break;
			}
           // hcPage.Title = "(" + Day.ToString() + ")" + hcPage.Title;
			return hcPage;
		}
	}
}
