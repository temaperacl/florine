using System;
namespace Florine
{
    public class Food
    {
        public NutrientSet Nutrients { get; set; }

        public IOption GetOption() {
            return new FoodOption() { Parent = this };
        }

        private class FoodOption: IOption {
            public Food Parent;
            public void AdjustNutrients(NutrientSet target) {
                foreach( KeyValuePair<Nutrient, int> kvp in Parent.Nutrients ) {
                    // Switch Dictionary Type to Concurrent?
                    int val = 0;
                    target.Nutrients.TryGetValue(kvp.key, out val);
                    target.Nutrients[kvp.key] = val + kvp.value;
                }
            }
        }
    }
}
