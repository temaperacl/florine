using System;
using System.Collections.Generic;

namespace Florine
{
    public class NutrientSet : IEnumerable<KeyValuePair<Nutrient, int>>
    {
        // Just Derive?
        
        public IEnumerator<KeyValuePair<Nutrient, int>> GetEnumerator()
        {
            foreach (KeyValuePair<Nutrient, int> kvp in Nutrients)
            {
                yield return kvp;
            }            
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(Nutrient n, int amount)
        {
            Nutrients[n] = amount;
        }
        public int this[Nutrient n]
        {
            get { return Nutrients[n]; }
            set { Nutrients[n] = value; }
        }

        // :P

        public Dictionary<Nutrient, int> Nutrients { get; private set; }
        public NutrientSet() {
            Nutrients = new Dictionary<Nutrient, int>();
        }
        public NutrientSet(Dictionary<Nutrient, int> _d)
        {
            Nutrients = _d;
        }
    }
}
