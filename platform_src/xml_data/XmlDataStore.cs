using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Florine;

namespace FlorineXml
{
    public class DataStore
    {
        public DataStore()
        {
        }
        /* Food */
        public static List<IndexedFood> GetAllFoods()
        {
            return IndexedFood.From(LoadFromFile<BufferFood>("Food"));
        }
		public static void ClearFoods()
		{
			Store<BufferFood> all = LoadFromFile<BufferFood>("Food");
			all.Clear();
			SavetoFile("Food", all);
		}
		public static void InsertFoods(List<Florine.Food> Foods)
		{
			Store<BufferFood> all = LoadFromFile<BufferFood>("Food");
			foreach(Florine.Food f in Foods) {
            	all.Add(new BufferFood(f));
			}
            SavetoFile("Food", all);
		}
		public static void UpdateFoodsForNutrientNameChange(string oldName, string newName)
		{
			Store<BufferFood> all = LoadFromFile<BufferFood>("Food");			
			foreach(BufferFood f in all) {				
            	for(int idx = 0; idx < f.NutNames.Count; ++idx) 
				{
					if(f.NutNames[idx] == oldName)
					{
						f.NutNames[idx] = newName;
					}
				}
			}
            SavetoFile("Food", all);
		}
        public static void InsertFood(string Name)
        {
            Store<BufferFood> all = LoadFromFile<BufferFood>("Food");
            all.Add(new BufferFood()
            {
                Name = Name,
            });
            SavetoFile("Food", all);
        }
        public static void UpdateNutrient(
            int idx,
            string Name
            )
        {
            Store<BufferFood> all = LoadFromFile<BufferFood>("Food");
            all[idx].Name = Name;
            SavetoFile("Food", all);
        }
        public static void DeleteFood(
            int idx
            )
        {
            Store<BufferFood> all = LoadFromFile<BufferFood>("Food");
            all.RemoveAt(idx);
            SavetoFile("Food", all);
        }

        public class IndexedFood : BufferFood
        {
            public int Idx { get; set; }

            public IndexedFood(BufferFood n, int idx)
            {
                this.Name = n.Name;
				this.NutNames = n.NutNames;
				this.NutVals = n.NutVals;
                this.Idx = idx;
            }
            public static Store<IndexedFood> From(List<BufferFood> l)
            {
                Store<IndexedFood> nl = new Store<IndexedFood>();
                for (int i = 0; i < l.Count; ++i)
                {
                    nl.Add(new IndexedFood(l[i], i));
                }
                return nl;
            }
        }
        public class BufferFood {
            public string Name { get; set; }			
            public List<string> NutNames { get; set; }			
			public List<Florine.NutrientAmount> NutVals { get; set; }

			private void _init_em() {
				NutNames = new List<string>();
				NutVals = new List<Florine.NutrientAmount>();
			}
            public Dictionary<string, Florine.NutrientAmount> NutrientDict()
            {				
                //TODO: Do actual nutrient retrieval.
                Dictionary<string, Florine.NutrientAmount> nuts = new Dictionary<string, Florine.NutrientAmount>();
				for(int i = 0; i < NutNames.Count; ++i) {
					nuts[NutNames[i]] = NutVals[i];
				}                
                return nuts;
            }
			public BufferFood() { 
				Name = "";				
				//_init_em();
			}
			public BufferFood(Florine.Food f) 
			{
				Name = f.Name;				
				_init_em();
				if(null != f.Nutrients) 
				{
					foreach(KeyValuePair<Florine.Nutrient, Florine.NutrientAmount> kvp in f.Nutrients)
					{
						NutNames.Add(kvp.Key.Name);
						NutVals.Add(kvp.Value);
					}
				}
			}
        };
        /* Nutrients */
        static List<Florine.Nutrient> _allNutrients;
        public static List<IndexedNutrient> GetAllNutrients(String TargetClass){
            if (null == _allNutrients)
            {
                _allNutrients = LoadFromFile<Florine.Nutrient>("Nutrient");
            }
            if ("" == TargetClass)
            {
                IndexedNutrient.From(_allNutrients);
            }
            return IndexedNutrient.From(_allNutrients).FindAll(n => n.Class.ToString() == TargetClass);
        }
		
        public static void InsertNutrient(string Name, string Class)
        {
            Store<Florine.Nutrient> all = LoadFromFile<Florine.Nutrient>("Nutrient");
            all.Add(new Florine.Nutrient()
            {
                Name = Name,
                Class = (Florine.Nutrient.NutrientType)Enum.Parse(typeof(Florine.Nutrient.NutrientType), Class, true)
        });
            SavetoFile("Nutrient", all);
        }
        public static void UpdateNutrient(
            int idx,
            string Name,
            string Class
            )
        {
            Store<Florine.Nutrient> all = LoadFromFile<Florine.Nutrient>("Nutrient");
			
			UpdateFoodsForNutrientNameChange(all[idx].Name, Name);			
            all[idx].Name = Name;		
			
            try
            {
                all[idx].Class = (Florine.Nutrient.NutrientType)Enum.Parse(typeof(Florine.Nutrient.NutrientType), Class, true);
            }
            catch
            {
                return;
            }
            SavetoFile("Nutrient", all);
        }
        public static void DeleteNutrient(
            int idx
            )
        {
            Store<Florine.Nutrient> all = LoadFromFile<Florine.Nutrient>("Nutrient");
            all.RemoveAt(idx);
            SavetoFile("Nutrient", all);
        }

        public class IndexedNutrient : Florine.Nutrient
        {
            public int Idx { get; set; }
            
            public IndexedNutrient(Florine.Nutrient n, int idx) {
                this.Name = n.Name;
                this.Class = n.Class;
                this.Idx = idx;
            }
            public static Store<IndexedNutrient> From(List<Florine.Nutrient> l)
            {
                Store<IndexedNutrient> nl = new Store<IndexedNutrient>();
                for (int i = 0; i < l.Count; ++i)
                {
                    nl.Add(new IndexedNutrient(l[i], i));
                }
                return nl;
            }
        }
        public class Store<T> : List<T> { }

        public static string BaseFilePath { get; set; }
        public static Store<T> LoadFromFile<T>(string DataType)
        {
            string fileName = BaseFilePath + "/" + DataType;
            if (System.IO.File.Exists(fileName))
            {
                return LoadDataFromString<T>(System.IO.File.ReadAllText(fileName));
            }
            return new Store<T>();

        }
        public static void SavetoFile<T>(string DataType, Store<T> data)
        {
            string fileName = BaseFilePath + "/" + DataType;
            System.IO.File.WriteAllText(fileName, DataAsString<T>(data));

        }

        public static Store<T> LoadDataFromString<T>(string input)
        { 
            using (var stream = new StringReader(input))
            {
                var formatter = new XmlSerializer(typeof(Store<T>));
                return (Store<T>)(formatter.Deserialize(stream));                
            }
        }
        public static string DataAsString<T>(Store<T> data)
        {            
            using (var stream = new StringWriter())
            {
                var formatter = new XmlSerializer(typeof(Store<T>));
                formatter.Serialize(stream,data);
                return stream.ToString();
            }
        }
    }
}
