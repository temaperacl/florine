using System;

namespace Florine
{
    public class Nutrient
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public enum NutrientType
        {
            Macro,
            Vitamin,
        };
    }
}
