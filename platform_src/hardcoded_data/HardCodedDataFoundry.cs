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
				_foodstuffs.Add(new Food() {
					Name = kvp.Key,
					Nutrients = new NutrientSet() {
						{Proteins,   l[0]},
						{Carbs,      l[1]},
						{Fats,       l[2]},
						{Fiber,      l[3]},
						{Folic_Acid, l[4]},
						{Vitamin_D,  l[5]},
						{Calcium,    l[6]},
						{Iron,       l[7]},
						{Potassium,  l[8]},
						{Vitamin_B12,l[9]},
						{Vitamin_A,  l[10]}
					}
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


		public class HardCodedActivityPath : BasicActivityPath
		{
			public override Florine.Activity ResolveActivityForGameState(GameState gs) {
				switch(gs.CurrentPage.MainType)
                                {
                                    case GameState.PageType.Summarize_Activity:
					if(gs.CurrentPage.SubType == GameState.PageSubType.Breakfast) {
						return new Activity() {
							Impact = new NutrientSet(),
							OptionName = "At Work",
							Description = gs.Player.Name + " is doing well at work this morning. She completed 27 measurements!",
						};
					} else if(gs.CurrentPage.SubType == GameState.PageSubType.Lunch) {
						return new Activity() {
							Impact = new NutrientSet(),
							OptionName = "At Work",
							Description = gs.Player.Name + " had a productive day. She wrote a blog post about her latest discovery!",
						};
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
                Finalizer = _emptyOption("Choose")
            };

            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Cooking",
                    Description = "Cook Tasty Stuff",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Dancing",
                    Description = "Cook Tasty Stuff",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Gym",
                    Description = "Cook Tasty Stuff",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Home",
                    Description = "Cook Tasty Stuff",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Social",
                    Description = "Cook Tasty Stuff",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Studying",
                    Description = "Cook Tasty Stuff",
                }
            );
            activities.Add(
                new Activity()
                {
                    Impact = new NutrientSet(),
                    OptionName = "Shopping",
                    Description = "Cook Tasty Stuff",
                }
            );

            return activities;
        }

		static HardCodedActivityPath _onlyPath = new HardCodedActivityPath();
		public bool GetNextGameState(GameState CurrentState,
						  out GameState.PageType nextType,
						  out GameState.PageSubType nextSubType
						  ) {
			nextType = CurrentState.CurrentPage.MainType;
			nextSubType = CurrentState.CurrentPage.SubType;

			switch(CurrentState.CurrentPage.MainType) {
				case GameState.PageType.Start:
					// TODO: Actual Switch
					nextType = GameState.PageType.Char_Creation;
					nextSubType = GameState.PageSubType.Setup;
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
					break;
				case GameState.PageType.Summarize_Meal:
					nextType = GameState.PageType.Summarize_Activity;
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
							nextType = GameState.PageType.Select_Activity;
							nextSubType = GameState.PageSubType.Daily;
							return true;
						case GameState.PageSubType.Lunch:
							nextType = GameState.PageType.Select_Meal;
							nextSubType = GameState.PageSubType.Dinner;
							return true;
						case GameState.PageSubType.Daily:
							nextType = GameState.PageType.Summarize_Day;
							nextSubType = GameState.PageSubType.Daily;
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
			{"toast",
			 	new List<NutrientAmount>() {2.25,13.6,1,0.725,72.4,26,0,29.8,0.832,32.8,0.005,0} },
			{"grilledcheese",
			 	new List<NutrientAmount>() {5,71,9,2,385,0,0,0,1.8,0,0,0} },
			{"pancakes",
			 	new List<NutrientAmount>() {2.09,68,2.796,.4,305.524,25.6,0,32.66,2.336,38.19,1.12,181} },
			{"fruit",
			 	new List<NutrientAmount>() {1.03,19.4,0.3,2.34,84.33,22.19,0,23.73,0.23,278.3,0,7.51} },
			{"eggs",
			 	new List<NutrientAmount>() {22,3.54,24.2,0,319.96,79.2,3.96,145,2.88,290,1.67,354} },
			{"cereal",
			 	new List<NutrientAmount>() {3.99,24.2,2.22,3.1,132.74,236,1.12,132,10.9,212,2.23,327} }
		};

		List<Food> _foodstuffs = new List<Food>();

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
					hcPage.Title = "Activity summary";
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
			return hcPage;
		}
	}
}
