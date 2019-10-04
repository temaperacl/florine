using System;
using System.Collections.Generic;
using Florine;

namespace FlorineSkiaSharpForms
{
    public class MobileAssetFoundry : FlorineHardCodedData.HardCodedDataFoundry //IPlatformFoundry
    {
        public MobileAssetFoundry()
        {
        }
        /* ================================================== IPlatformFoundry */

        public override GameState LoadGameState() {
            GameState gs = base.LoadGameState();
            gs.Player.Avatar.Picture = new PlayerAvatar(gs.Player.Avatar.Picture);
            return gs;
        }
        //public override bool SaveGameState(GameState _unused) { return base.SaveGameState(); }
        public override IList<Food> LoadFood() { return base.LoadFood(); }
        public override IList<Nutrient> LoadNutrients() { return  base.LoadNutrients(); }
        public override IPage GetPage(IPage GenericPage) { return base.GetPage(GenericPage); }
        public override Activity AutomaticActivity(GameState gs) { return base.AutomaticActivity(gs); }






        /* ================================================== Support */
        /* Data Dumps */
		
// IIMg        private int AssetKey;
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
