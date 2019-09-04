using System;
using System.Collections.Generic;

namespace Florine
{
    public class Food
    {
        public String Name { get; set; }
        public NutrientSet Nutrients { get; set; }

        public IGameOption GetOption() {
            return new FoodOption() { Parent = this };
        }

        private class FoodOption: IGameOption {
            public Food Parent;
            public String OptionName { get { return Parent.Name; } }
            public void AdjustNutrients(NutrientSet target) {
                // Probably should change NutrientSet type to inherit directly or implement IEnumX
                foreach( KeyValuePair<Nutrient, int> kvp in Parent.Nutrients ) {
                    // Switch Dictionary Type to Concurrent?
                    int val = 0;
                    target.Nutrients.TryGetValue(kvp.Key, out val);
                    target.Nutrients[kvp.Key] = val + kvp.Value;
                }
            }
        }
    }
}
