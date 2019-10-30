using System;
using System.Collections.Generic;

namespace Florine
{
	public class NutrientAmount {
		private double _amount;
		public NutrientAmount(double amount) { _amount = amount; }		
		public static NutrientAmount operator +(NutrientAmount a) { return a; }
		public static NutrientAmount operator +(NutrientAmount a, NutrientAmount b)
		{ 
			if(null == a) { return b; }
			if(null == b) { return a; }
			return new NutrientAmount(a._amount + b._amount); 
		}
		public static NutrientAmount operator -(NutrientAmount a)
		{ return new NutrientAmount(-a._amount); }
		public static NutrientAmount operator -(NutrientAmount a, NutrientAmount b)
		{ return new NutrientAmount(a._amount - b._amount); }
		public static NutrientAmount operator *(NutrientAmount a, NutrientAmount b)
		{ return new NutrientAmount(a._amount * b._amount); }
		public static NutrientAmount operator /(NutrientAmount a, NutrientAmount b)
		{ return new NutrientAmount(a._amount / b._amount); }
		
		public static implicit operator double(NutrientAmount n)
		{ return n._amount; }
		public static implicit operator NutrientAmount(double d)
		{ return new NutrientAmount(d); }
        public override string ToString() 
		{ return _amount.ToString(); }
		public string ToString(string Spec) 
		{ return _amount.ToString(Spec); }
	}
	
    public class NutrientSet : IEnumerable<KeyValuePair<Nutrient, NutrientAmount>>
    {
        // Just Derive?        
        public IEnumerator<KeyValuePair<Nutrient, NutrientAmount>> GetEnumerator()
        {
            List<string> NutrientRainbowOrder = new List<string>()
            {
                "Calcium",
                "Potassium",
                "Vitamin D",
                "Folic Acid",
                "Vitamin B12",
                "Vitamin A",
                "Iron",
                "Protein",
                "Carbohydrates",
                "Fat",
                "Fiber"
            };
            foreach (string s in NutrientRainbowOrder)
            {
                if (Nutrients.ContainsKey(s))
                {
                    yield return new KeyValuePair<Nutrient, NutrientAmount>(_LastNutrients[s], Nutrients[s]);
                }
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public Florine.NutrientAmount AsCalories()
        {
            return 9 * Nutrients["Fat"]
                + 4 * Nutrients["Protein"]
                + 4 * Nutrients["Carbohydrates"];
        }
        public void Add(object o)
        {
        }
        public bool TryGetValue(Florine.Nutrient n, out NutrientAmount amount)
        {
            return Nutrients.TryGetValue(n.Name, out amount);
        }
		public int Count { get { return Nutrients.Count; } }
        public void Add(Nutrient n, NutrientAmount amount)
        {

            Nutrients[n.Name] = amount;
            _LastNutrients[n.Name] = n;
        }
        public NutrientAmount this[Nutrient n]
        {
            get { return Nutrients[n.Name]; }
            set { Nutrients[n.Name] = value; _LastNutrients[n.Name] = n; }
        }
		public NutrientAmount this[string n]
        {
            get 
			{ 
				if(Nutrients.ContainsKey(n)){ return Nutrients[n]; }
				return 0.0; 
			}
			set
			{
				if(Nutrients.ContainsKey(n)){ Nutrients[n] = value; }
			}
        }
		public void ApplyOption(IGameOption option) {
			option.AdjustNutrients(this);
		}
        // :P        
        private Dictionary<string, Nutrient> _LastNutrients = new Dictionary<string, Nutrient>();
        private Dictionary<string, NutrientAmount> Nutrients { get; set; }
        public NutrientSet() {
            Nutrients = new Dictionary<string, NutrientAmount>();
        }
        public NutrientSet(Dictionary<Nutrient, NutrientAmount> _d)
        {
            foreach (KeyValuePair<Nutrient, NutrientAmount> kvp in _d)
            {
                _LastNutrients[kvp.Key.Name] = kvp.Key;
                Nutrients[kvp.Key.Name] = kvp.Value;
            }
            
        }
    }
}
