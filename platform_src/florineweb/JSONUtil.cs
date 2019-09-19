using System;
using System.Text; //helper
using System.Linq;  //helper
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.IO;
using System.IO.Compression;
using Florine;

namespace FlorineJSON
{
	
    public class JSONUtil
    {
        // Web Overrides
		 public static GameState DeserializeGameState(string source) {            
			return _ComposeGameState(
				_getSerializer().Deserialize<DecomposedGameState>(
					_uncompress(source)
				)
			);
        }        
		
        public static string SerializeGameState(GameState gs, bool Compress = true)
        {
			string _base = 	_getSerializer().Serialize(
				_DeComposeGameState(gs)
			);
			if(Compress) { _base = _compress(_base); }
			return _base;
        }
		/* Composition */
		private static GameState _ComposeGameState(DecomposedGameState dgs) 
		{
			return dgs.Parent;
		}
		private static DecomposedGameState _DeComposeGameState(GameState gs)
		{
			return new DecomposedGameState() { Parent = gs };
		}
		private class DecomposedGameState
		{
			public GameState Parent { get; set; }
		}
		
		/* Compress */
		private static string _compress(string s) 
		{
			byte[] data = Encoding.UTF8.GetBytes(s);

			using (MemoryStream instream = new MemoryStream(data)) {
				using (MemoryStream outstream = new MemoryStream()) {
					using ( GZipStream compressor = new GZipStream(
						outstream,
						CompressionMode.Compress) 
					) {
						instream.CopyTo(compressor);						
					}					
					return Convert.ToBase64String(outstream.ToArray());
				}
			}
		}
		private static string _uncompress(string s) 
		{
			byte[] data = Convert.FromBase64String(s);

			using (MemoryStream instream = new MemoryStream(data)) {
				using (MemoryStream outstream = new MemoryStream()) {
					using ( GZipStream compressor = new GZipStream(
						instream,
						CompressionMode.Decompress) 
					) {
						compressor.CopyTo(outstream);						
					}					
					
					return Encoding.UTF8.GetString(
						outstream.ToArray()
					);
				}
			}			
		}
        /* Misc Support */
		private static JavaScriptSerializer _getSerializer()
		{
			System.Web.Script.Serialization.JavaScriptSerializer serializer =
				new System.Web.Script.Serialization.JavaScriptSerializer(
					new FlorineTypeResolver()
				);
			serializer.RegisterConverters(
				new List<System.Web.Script.Serialization.JavaScriptConverter> 
				{ 
					new NutrientSetConverter(),
					new IPageConverter()
				}
			);
			return serializer;
		}		
	}

	public class FlorineTypeResolver : SimpleTypeResolver 
	{
		public override Type ResolveType(string id) {
			if(id == typeof(JSONIPage).ToString()) {
				return typeof(JSONIPage);
			}
			return base.ResolveType(id);
		}
		public override string ResolveTypeId(Type type) {
			if(typeof(IPage).IsAssignableFrom(type) )
			{
				return typeof(JSONIPage).ToString();
			}
			return base.ResolveTypeId(type);
		}
	}
	/* ==========================================================   NutrientSet Support */
	public class NutrientSetConverter : System.Web.Script.Serialization.JavaScriptConverter
	{
		public override IEnumerable<Type> SupportedTypes
		{			
			get { return new List<Type>() { typeof(Florine.NutrientSet) }; }
		}

		public override IDictionary<string, object> Serialize(
			object obj, 
			System.Web.Script.Serialization.JavaScriptSerializer serializer
		) {
			Florine.NutrientSet ns = obj as Florine.NutrientSet;
			Dictionary<string, object> result = new Dictionary<string, object>();

			if (null != ns)
			{
				// Create the representation.            
				System.Collections.ArrayList itemsList = new System.Collections.ArrayList();

				foreach (KeyValuePair<Florine.Nutrient, NutrientAmount> kvp in ns)
				{
					//Add each entry to the dictionary.
					itemsList.Add(new Dictionary<string, object>()
								  {
									  { "n", kvp.Key },
									  { "v", (double)(kvp.Value) }
								  }
								 );     
				}
				result["Nutrients"] = itemsList;                
			}
			return result;
		}

		public override object Deserialize(
			IDictionary<string, object> dictionary,
			Type type,
			System.Web.Script.Serialization.JavaScriptSerializer serializer)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			if (type == typeof(Florine.NutrientSet))
			{
				// Create the instance to deserialize into.
				Florine.NutrientSet ns = new Florine.NutrientSet();                

				// Deserialize the ListItemCollection's items.
				System.Collections.ArrayList itemsList =
					(System.Collections.ArrayList)dictionary["Nutrients"];
				foreach(object o in itemsList)
				{
					Dictionary<string, object> itm =
						serializer.ConvertToType<Dictionary<String,object>>(o);

					ns[serializer.ConvertToType<Florine.Nutrient>(itm["n"])] = 
						serializer.ConvertToType<double>(itm["v"]);
					//list.Add(serializer.ConvertToType<ListItem>(itemsList[i]));
				}
				return ns;
			}
			return null;
		}
    }	
	
	class JSONIOptionSet : List<IGameOption>, IGameOptionSet
	{
		public int SelectionLimit { get; set; }
		public IGameOption Finalizer { get; set; }
	}
	class JSONIPage : IPage {
		public GameState.PageType MainType { get; set; }
		public GameState.PageSubType SubType { get; set; }

		public IGameOptionSet AppliedOptions { get; set; }

		public IImage Background { get { return null; } }
		public IGameOptionSet PrimaryOptions { get { return null; } }
		public String Title { get { return MainType.ToString(); } }
		public String Message { get { return null; } }
		public NutrientSet NutrientState { get { return null; } }
		public NutrientSet NutrientDelta { get { return null; } }
	}
	/* ========================================================== GameOption Support */
	public class IPageConverter : System.Web.Script.Serialization.JavaScriptConverter
	{
		public override IEnumerable<Type> SupportedTypes
		{			
			get { return new List<Type>() {
				typeof(JSONIPage)				
			}; }
		}		

		public override IDictionary<string, object> Serialize(
			object obj, 
			System.Web.Script.Serialization.JavaScriptSerializer serializer
		) {					
			return null;
			/*
			IPage page = obj as IPage;
			Dictionary<string, object> result = new Dictionary<string, object>();			
			
			if (null != page)
			{
				result["MainTypeX"] = page.MainType;
				result["SubType"] = page.SubType;
				if(null != page.AppliedOptions) 
				{
					result["AO_SL"] = page.AppliedOptions.SelectionLimit;
					result["AO_F"] = page.AppliedOptions.Finalizer;
					System.Collections.ArrayList optionList =
						new System.Collections.ArrayList();	
					foreach(IGameOption  opt in page.AppliedOptions) 
					{
						optionList.Add(opt);
					}
					result["AO_O"] = optionList;
				}				
			}
			return result;			
			*/
		}

		public override object Deserialize(
			IDictionary<string, object> dictionary,
			Type type,
			System.Web.Script.Serialization.JavaScriptSerializer serializer)
		{			
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			if (type == typeof(JSONIPage))
			{
				JSONIPage np = new JSONIPage();
			    np.MainType = serializer.ConvertToType<GameState.PageType>(
					dictionary["MainType"]
				);
				np.SubType = serializer.ConvertToType<GameState.PageSubType>(
					dictionary["SubType"]
				);
				if(dictionary.Keys.Contains("AppliedOptions"))
				{
					np.AppliedOptions = new JSONIOptionSet()
					{						
					};
					System.Collections.ArrayList lst = 
						serializer.ConvertToType<System.Collections.ArrayList>(
							dictionary["AppliedOptions"]
						);
					if(lst != null) {
						foreach(object i in lst) {		
							if(i is IGameOption) {
								np.AppliedOptions.Add(serializer.ConvertToType<IGameOption>(i));
							} else {
								foreach(object o in serializer.ConvertToType<ArrayList>(i))
								{
									//List<IGameOption>
									np.AppliedOptions.Add(serializer.ConvertToType<IGameOption>(o));
								}
							}
						}
					}
				}
				

				return np;
			}
			return null;
		}		
    }	
	/* ===================================== */
	public class JsonHelper
{
    private const string INDENT_STRING = "    ";
    public static string FormatJson(string str)
    {
        var indent = 0;
        var quoted = false;
        var sb = new StringBuilder();
        for (var i = 0; i < str.Length; i++)
        {
            var ch = str[i];
            switch (ch)
            {
                case '{':
                case '[':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    break;
                case '}':
                case ']':
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    sb.Append(ch);
                    break;
                case '"':
                    sb.Append(ch);
                    bool escaped = false;
                    var index = i;
                    while (index > 0 && str[--index] == '\\')
                        escaped = !escaped;
                    if (!escaped)
                        quoted = !quoted;
                    break;
                case ',':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    break;
                case ':':
                    sb.Append(ch);
                    if (!quoted)
                        sb.Append(" ");
                    break;
                default:
                    sb.Append(ch);
                    break;
            }
        }
        return sb.ToString();
    }
}

static class Extensions
{
    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
    {
        foreach (var i in ie)
        {
            action(i);
        }
    }
}
}