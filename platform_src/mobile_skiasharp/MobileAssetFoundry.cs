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
        //public virtual IList<Food> LoadFood() 
        //public virtual IList<Nutrient> LoadNutrients() { return _nutrients; }
        //public virtual IPage GetPage(IPage GenericPage) { return HardCodedPageFromIPage(GenericPage); }        


        //public virtual Activity AutomaticActivity(GameState gs)        
        /* ================================================== Support */
        /* Data Dumps */
        private int AssetKey;
		
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
            int BaseKey = (int)AssetClass.Food;
                return BaseKey;
        }
        public static int AssetKey(this Food.FoodOption component) 
        {
            int BaseKey = (int)AssetClass.FoodOption;
            return BaseKey;
        }
        public static int AssetKey(this IPage component)
        {
            int BaseKey = (int)AssetClass.Page;
            return BaseKey;

        }
    }
}
