using System;
using System.Collections.Generic;

namespace Florine
{
    public class Food
    {
        public String Name { get; set; }
        public NutrientSet Nutrients { get; set; }
        public IImage OptionPicture { get; set; }
        public IGameOption GetOption() {
            return new FoodOption() { Parent = this };
        }

        private class FoodOption: IGameOption {
            public Food Parent;
            public String OptionName { get { return Parent.Name; } }
            public IImage Picture { get { return Parent.OptionPicture; } }
			public IGameOptionSet SubOptions { get { return null; } }
			public void ImpactPlayer(Player target) { AdjustNutrients(target.Nutrients); }
            public void AdjustNutrients(NutrientSet target) {
                // Probably should change NutrientSet type to inherit directly or implement IEnumX
                foreach( KeyValuePair<Nutrient, NutrientAmount> kvp in Parent.Nutrients ) {
                    // Switch Dictionary Type to Concurrent?
                    NutrientAmount val = 0;
                    target.TryGetValue(kvp.Key, out val);
                    target[kvp.Key] = val + kvp.Value;
                }
            }
        }
    }
}
