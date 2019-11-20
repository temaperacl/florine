using System;
using System.Collections.Generic;

namespace Florine
{
    public class Player
    {
        public Player() {
            Avatar = new Avatar();
            Nutrients = new NutrientSet();
        }
        Player _Yesterday;
        public Player Yesterday
        {
            get
            {
                if (null == _Yesterday) { _Yesterday = new Player(); }
                return _Yesterday;
            }
            set
            {
                _Yesterday = value;
            }
        }
        
        public String Name { get; set; } = "Faerina";
        public Avatar Avatar { get; set; }
        public NutrientSet Nutrients { get; set; }
		public IActivityPath MainPath { get; set; }
        int _money = 0;
        int _happiness = 0;
        public int Happiness
        {
            get { return _happiness; }
            set
            {
                HappinessToDate += (value - _happiness);
                _happiness = value;
            }
        }
        public int Money
        {
            get { return _money; }
            set
            {
                MoneyToDate += (value - _money);
                _money = value;
            }
        }
        public int HappinessToDate { get; set; } = 0;
        public int MoneyToDate { get; set; } = 0;
        public double Energy { get; set; } = 0.0;
        public double Focus { get; set; } = 0.0;
        public double Hunger { get; set; } = 0.0;
		public double Calories { get; set; }
		public double TargetCalories { get; set; }
		public double HoursIntoDay { get; set; }
        private bool _readyToEndDay = false;
		public bool ReadyToEndDay
        {
            get
            {
                return _readyToEndDay
                    || Energy <= 10
                    || HoursIntoDay > 22;
            }
            set
            {
                _readyToEndDay = value;
            }
        }
		public void Tick(int Hours)
		{
			//Raw Calories.
			HoursIntoDay += (double)Hours;
            double target_kcal = 2000.0; // * HoursIntoDay/24;
			double current_kcal = 0
				+ 9 * Nutrients["Fat"]
				+ 4 * Nutrients["Protein"]
				+ 4 * Nutrients["Carbohydrates"];
			Calories = current_kcal;
			TargetCalories = target_kcal;
			
			if(Hours > 0) {
                if (HoursIntoDay > 7)
                {
                    Energy -= Hours * (100 / 16);
                }
			   //Energy = 100 * Math.Min(1.0, Calories/target_kcal);			
			    Hunger = 100 - Energy;
			    Focus = Energy;
			
				//Adjust back up for simple carbs

				//Nutrient Target
				NutrientSet Delta = new NutrientSet();
				foreach(KeyValuePair<Nutrient, NutrientAmount> kvp in Nutrients)
				{				
					//if(kvp.Key.Class != Nutrient.NutrientType.Macro) {
						Delta[kvp.Key] = kvp.Key.DailyTarget / Hours;
					//}
				}
				foreach(KeyValuePair<Nutrient, NutrientAmount> kvp in Delta)
				{				
					//Nutrients[kvp.Key] -= Delta[kvp.Key];
				}
			}
			
		}
		
		public void StartNewDay() 
		{
            //Calculate Next-day effects
            /*
			Nutrients["Fat"] = 0.0;
			Nutrients["Carbohydrates"] = 0.0;
			Nutrients["Protein"] = 0.0;
			Nutrients["Fiber"] = 0.0;		
			*/
            Yesterday = new Player();
            Yesterday.MoneyToDate = MoneyToDate;
            Yesterday.HappinessToDate = HappinessToDate;
            Yesterday.Nutrients = Nutrients;
            Yesterday.Calories = Calories;
            Yesterday.Hunger = Hunger;
            Yesterday.Energy = Energy;
            Yesterday.TargetCalories = 2000;

            MoneyToDate = 0;
            HappinessToDate = 0;
			Nutrients = new NutrientSet();
			Calories = 0;
			Hunger = 0;
			Energy = 100;
			TargetCalories = 0;
			HoursIntoDay = 0;
            ReadyToEndDay = false;

        }
		
        public void ApplyOption(IGameOption option)
        {
            option.ImpactPlayer(this);
			
			NutrientSet Delta = new NutrientSet();
			option.AdjustNutrients(Delta);
			
			// Now Adjust Based on Targets.
			
			
			// Adjust Based on Deltas.
        }
    }
}
