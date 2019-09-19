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
		public double DailyTarget { get; set; }
        public enum NutrientType
        {
            Macro,
            Vitamin,
			Mineral
        };
    }
}
