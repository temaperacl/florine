using System;
using System.Collections.Generic;

namespace Florine
{
    public class NutrientSet
    {
        public Dictionary<Nutrient, int> Nutrients { get; private set; }
        public NutrientSet() {
            Nutrients = new Dictionary<Nutrient, int>();
        }
    }
}
