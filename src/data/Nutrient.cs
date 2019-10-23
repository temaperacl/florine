using System;

namespace Florine
{
	public enum NutrientUnit {
		mcg,
		IU,
		mg,
		g,
	}
    public class Nutrient
    {
        public string Name { get; set; }
        public NutrientType Class { get; set; }        
		public NutrientUnit Units { get; set; }
		public NutrientAmount DailyTarget { get; set; }
        public float RatioRDV(NutrientAmount amount)
        {

            if (DailyTarget == null)
            {
                return 1f;
            }
            if (amount == null)
            {
                return 0f;
            }
            return (float)(amount / DailyTarget);
        }
        public enum NutrientType
        {
            Macro,
            Vitamin,
			Mineral
        };
    }
}
