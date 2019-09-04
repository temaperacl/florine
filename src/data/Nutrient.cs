using System;

namespace Florine
{
    public class Nutrient
    {
        public string Name { get; set; }
        public NutrientType Class { get; set; }
        public enum NutrientType
        {
            Macro,
            Vitamin,
        };
    }
}
