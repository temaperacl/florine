using System;

namespace Florine
{
    public class NutrientSet
    {
        public Dictionary<Nutrient, int> Nutrients { get; }
        public NutrientSet {
            Nutrients = new Dictionary<Nutrient, int>();
        }
    }
}
