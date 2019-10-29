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
			foreach(KeyValuePair<string, List<NutrientAmount>> kvp in FoodTable) {
				List<NutrientAmount> l = kvp.Value;
                _foodIdx[kvp.Key] = _foodIdx.Count;
                NutrientAmount n = l[11];
                _foodstuffs.Add(new Food() {
                    Name = kvp.Key,
                    Nutrients = new NutrientSet() {
                        {Proteins,   l[0] * n},
                        {Carbs,      l[1] * n},
                        {Fats,       l[2] * n},
                        {Fiber,      l[3] * n},
                        {Folic_Acid, l[4] * n},
                        {Vitamin_D,  l[5] * n},
                        {Calcium,    l[6] * n},
                        {Iron,       l[7] * n},
                        {Potassium,  l[8] * n},
                        {Vitamin_B12,l[9] * n},
                        {Vitamin_A,  l[10] * n}
                    },
                    Description = kvp.Key // Food Description
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
			DailyTarget = 0
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
		static Nutrient Vitamin_B12 =new Nutrient()
		{
			Name = "Vitamin B12",
			Class = Nutrient.NutrientType.Vitamin ,
			Units = NutrientUnit.mcg,
			DailyTarget = 2.4
		};
		static Nutrient Potassium = new Nutrient()
		{
			Name = "Potassium",
			Class = Nutrient.NutrientType.Vitamin ,
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
			Units = NutrientUnit.IU,
			DailyTarget = 600
		};
		static Nutrient Folic_Acid = new Nutrient() {
			Name = "Folic Acid",
			Class = Nutrient.NutrientType.Vitamin,
			Units = NutrientUnit.mcg,
			DailyTarget = 400
		};


		List<Nutrient> _nutrients = new List<Nutrient>()
		{
			Carbs,
			Proteins,
			Fats,
			Vitamin_A,
			Vitamin_B12,
			Calcium,
			Potassium,
			Iron,
			Vitamin_D,
			Folic_Acid

		};

        public class EndDayActivity : Activity
        {
            public override void ImpactPlayer(Player target)
            {
                target.ReadyToEndDay = true;
                base.ImpactPlayer(target);
            }
        }

        public class HardCodedActivityPath : BasicActivityPath
        {
            public override Florine.Activity ResolveActivityForGameState(GameState gs)
            {
                switch (gs.CurrentPage.MainType)
                {
                    case GameState.PageType.Summarize_Activity:
                        if (gs.CurrentPage.SubType == GameState.PageSubType.Breakfast)
                        {
                            return new Activity()
                            {
                                Impact = new NutrientSet(),
                                OptionName = "At Work",
                                Description = gs.Player.Name + " is doing well at work this morning. She completed 27 measurements!",
                            };
                        }
                        else if (gs.CurrentPage.SubType == GameState.PageSubType.Lunch)
                        {
                            return new Activity()
                            {
                                Impact = new NutrientSet(),
                                OptionName = "At Work",
                                Description = gs.Player.Name + " had a productive day. She wrote a blog post about her latest discovery!",
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
                                    Description = gs.Player.Name + " goes to bed at " + (gs.Player.HoursIntoDay - 12) + "pm"
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
                                        Description = "After a nice evening at home, " + gs.Player.Name + " feels renewed " 
                                        + "and goes to bed at " + (gs.Player.HoursIntoDay - 12) + "pm",
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
                    Description = "Cook Tasty Stuff",
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
                    Description = "Working out at the gym",
                }
            );
            activities.Add(
                new EndActivity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Home",
                    Description = "A quiet night at home",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Social",
                    Description = "Talking and meeting up with friends",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Studying",
                    Description = "Learn and grow!",
                }
            );            
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Shopping",
                    Description = "Find new Items to shop",
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
					nextType = GameState.PageType.Summarize_Activity;
					nextSubType = GameState.PageSubType.Daily;
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
                {"Fruit",             new List<NutrientAmount>() { 1.03, 19.4, 0.3,   2.34,84.33,22.19,0,23.73,0.23,278.3,0,7.51,1} },
                {"Eggs",              new List<NutrientAmount>() { 22  , 3.54, 24.2,  0,319.96,79.2,3.96,145,2.88,290,1.67,354,1} },
                {"Cereal",            new List<NutrientAmount>() { 3.99, 24.2, 2.22,  3.1,132.74,236,1.12,132,10.9,212,2.23,327,1} },
                {"Toaster Pastry",    new List<NutrientAmount>() { 2.09, 15.1, 2.73,  0.4,93.3,25.6,0,31.2,2.27,36,1.12,181,1 } },
                {"Wheat Toast",       new List<NutrientAmount>() { 3.11, 13.4, 1.02,  1.13,75.22,20.6,0,39.6,0.982,53.5,0,0,2 } },
                {"Multigrain Toast",  new List<NutrientAmount>() { 4.79, 15.5, 1.52,  2.67,94.84,23.1,0,36.6,0.898,82.5,0,0,2 } },
                {"Strawberry Yogurt", new List<NutrientAmount>() { 8.38, 23.5, 2.12,  0,146.6,18.7,1.7,291,.119,372,0.901,73.1,1 } },
                {"Eggs, Bacon, and Toast",
                    new List<NutrientAmount>() { 29.96, 17.276,28.01,0.725,429.034,105.2,3.992,175.68,3.788,362.7,1.762,354.88,1 } },
                {"Hamburger",
                    new List<NutrientAmount>() { 13.3, 29.6, 10.2, 1.8, 263.4, 46, .1, 116, 2.87, 197, 1.2, 3, 1 } },
                {"Chocolate Ice Cream",
                    new List<NutrientAmount>() { 2.51, 18.6, 7.26, .792, 149.78, 10.6, 5.28, 71.9, .614, 164, .191, 275, 1 } },
                {"Instant Noodles",
                    new List<NutrientAmount>() { 3.61, 21.4, 6.24, .932, 156.2, 28, 0, 14, 1.44, 65.2, .093, 0, 1 } },
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
                    new List<NutrientAmount>() { 8.93, 47.5, 7.14, 4.46, 289.98, 107, 0, 39.7, 2.48, 434, 0, 37.2, 1 } },
                {"Spaghetti w/ Meatballs",
                    new List<NutrientAmount>() { 14.3, 42.7, 11.1, 3.97, 327.9, 96.7, 0, 42.2, 2.7, 471, 2.7, 471, .372, 34.7, 1 } },
                {"Sushi Roll, Tuna",
                    new List<NutrientAmount>() { 2.22, 4.59, 0.075, .21, 27.915, .9, .12, 1.2, .093, 35.7, .153, 2.4, 6 } },
                {"Tacos",
                    new List<NutrientAmount>() { 12, 22.2, 16.5, 3.1, 285.3, 28.8, 0, 117, 1.63, 316, 0.8282, 52.9, 2 } },
                {"Donuts",
                    new List<NutrientAmount>() { 7.78, 48.4, 31.1, 1.355, 504.62, 80.2, 0, 48.2, 2, 113.9, 0.227, 38.5, 1 } },                

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

		protected class HardcodedEmptyOption : IGameOption
		{
			public HardcodedEmptyOption(string name) { OptionName = name; }
			public IImage Picture { get { return null; } }
			public String OptionName { get; set; }
			public void ImpactPlayer(Player target) { }
			public void AdjustNutrients(NutrientSet nutrients) { }
			public IGameOptionSet SubOptions { get; set; }
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
                    hcPage.Message = "Welcome to a new day. Let's see what today holds!";
                    if (Day == 2)
                    {
                        hcPage.Message = "You don’t need to eat a lot to feel full - try eating more fiber(with fiber in the same green as on the nutrient bars).";
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
                    Random r = new Random();
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
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Cereal"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["White Toast"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Strawberry Yogurt"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Fruit"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Pancakes"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Toaster Pastry"]].GetOption());
                            }
                            else if (Day == 2)
                            {
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Eggs, Bacon, and Toast"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Fruit"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Wheat Toast"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Pancakes"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Eggs"]].GetOption());
                                PrimaryOptions.Add(_foodstuffs[_foodIdx["Toaster Pastry"]].GetOption());
                            }
							break;
						case GameState.PageSubType.Lunch:
							PrimaryOptions.SelectionLimit = 3;
                            List<string> LunchOptions = new List<string>() {
                                "Hamburger",                              
                                "Instant Noodles",
                                "Meatball Sub",
                                "Peanut Butter & Jelly",
                                "Pepperoni Pizza Slice",
                                "Vegetable Soup",
                                "Spaghetti",
                                "Spaghetti w/ Meatballs",
                                "Sushi Roll, Tuna",
                                "Tacos",
                                "Donuts",
                            };
                            for (int i = 0; i < 6; ++i)
                            {
                                int lOpt = r.Next(LunchOptions.Count);
                                PrimaryOptions.Add(_foodstuffs[_foodIdx[LunchOptions[lOpt]]].GetOption());
                                LunchOptions.RemoveAt(lOpt);
                            }
							break;
						case GameState.PageSubType.Dinner:
                            List<string> DinnerOptions = new List<string>() {
                                "Hamburger",
                                "Chocolate Ice Cream",
                                "Instant Noodles",
                                "Lamb Chops",
                                "Meatball Sub",                                
                                "Pepperoni Pizza Slice",
                                "Vegetable Soup",
                                "Spaghetti",
                                "Spaghetti w/ Meatballs",
                                "Sushi Roll, Tuna",
                                "Tacos",
                                "Donuts",
                            };

                            for (int i = 0; i < 6; ++i)
                            {
                                int lOpt = r.Next(DinnerOptions.Count);
                                PrimaryOptions.Add(_foodstuffs[_foodIdx[DinnerOptions[lOpt]]].GetOption());
                                DinnerOptions.RemoveAt(lOpt);
                            }
                            PrimaryOptions.SelectionLimit = 3;
							break;
					}
                    if (PrimaryOptions.Count == 0)
                    {
                        for (int idx = 0; idx < _foodstuffs.Count && idx < 6; ++idx)
                        {
                            PrimaryOptions.Add(_foodstuffs[idx].GetOption());
                        }
                    }
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
					hcPage.Title = "Select Activity";
					hcPage.Message = "TBD";
					hcPage.PrimaryOptions = GetDailyActivities();
					break;
				case GameState.PageType.Summarize_Activity:
                    hcPage.Title = generic.SubType.ToString() + "Summary";
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
