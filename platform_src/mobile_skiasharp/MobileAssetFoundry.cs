using System;
using System.Collections.Generic;
using Florine;

namespace FlorineHardCodedData
{
    public class MobileAssetFoundry : HardCodedDataFoundry //IPlatformFoundry
    {
        public MobileAssetFoundry()
        {
        }

        /* ================================================== IPlatformFoundry */
        
        // public virtual GameState LoadGameState() { return new GameState(); }
        // public virtual bool SaveGameState(GameState _unused) { return false; }
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
        //public virtual IList<Nutrient> LoadNutrients() { return _nutrients; }
        //public virtual IPage GetPage(IPage GenericPage) { return HardCodedPageFromIPage(GenericPage); }        


        public virtual Activity AutomaticActivity(GameState gs)
        {
                return _onlyPath.GetActivity(gs);
        }
                
        /* ================================================== Support */
        /* Data Dumps */
        private int AssetKey
		
/*		static Nutrient Carbs = new Nutrient() 
		{ 
			Name = "Carbohydrates", 
			Class = Nutrient.NutrientType.Macro,
			Units = NutrientUnit.g,
			DailyTarget = 130
		};
*/
    }

    public static class MobileFoundryExtensions
    {
        enum AssetClass : int
        {
            Food       = 0x01000000,
            FoodOption = 0x02000000,
            Page       = 0x04000000,
            Nutrient   = 0x08000000,
            Avatar     = 0x10000000
        };

        public static int AssetKey(this Food component)             
        {
            int BaseKey = (int)AssetClass.Food,
        }
        public static int AssetKey(this Food.FoodOption component) 
        {
            int BaseKey = (int)AssetClass.FoodOption,

        }
        public static int AssetKey(this IPage component)
        {
            int BaseKey = (int)AssetClass.Page,

        }
    }
}
