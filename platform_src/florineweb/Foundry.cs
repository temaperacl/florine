using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Florine;

namespace FlorineWeb
{
    public class WebFoundry : FlorineHardCodedData.HardCodedDataFoundry // : IPlatformFoundry
    {
        // Web Overrides
        public override GameState LoadGameState() {			
            
            if (
                null != _queryparams.GetValues("reset")
                || null == _incookies.Get("florine")
				|| null == _incookies["florine"].Values["game"]
                )
            {
                return base.LoadGameState();
            }			                
            NameValueCollection SourceData = _incookies["florine"].Values;
			_newGame = false;
			return FlorineJSON.JSONUtil.DeserializeGameState(
									  			SourceData["game"]								  		);
        }        

		
        public override bool SaveGameState(GameState gs)
        {
			System.Web.HttpCookie outData = new System.Web.HttpCookie("florine");
			outData["game"] = FlorineJSON.JSONUtil.SerializeGameState(
									  		gs
								  		);
			_outcookies.Set(outData);
			return true;			
        }
		
		
        /* Misc Support */
		private System.Web.Script.Serialization.JavaScriptSerializer _getSerializer()
		{
			System.Web.Script.Serialization.JavaScriptSerializer serializer =
				new System.Web.Script.Serialization.JavaScriptSerializer(
					new System.Web.Script.Serialization.SimpleTypeResolver()
				);
			serializer.RegisterConverters(
				new List<System.Web.Script.Serialization.JavaScriptConverter> 
				{ 
					new NutrientSetConverter()
				}
			);
			return serializer;
		}
		private string _serializeNutrientSet(Florine.NutrientSet target)
		{
			
			return _getSerializer().Serialize(target);
		}
		private Florine.NutrientSet _deserializeNutrientSet(string source)
		{
			if(null == source) { return new Florine.NutrientSet(); }			
			return _getSerializer().Deserialize<Florine.NutrientSet>(source);			
		}		
		private string _serializePlayer(Florine.Player target)
		{
			
			return _getSerializer().Serialize(target);
		}
		private Florine.Player _deserializePlayer(string source)
		{
			if(null == source) { return new Florine.Player(); }			
			return _getSerializer().Deserialize<Florine.Player>(source);			
		}		
		
		/* NutrientSet Support */
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
									  { "n", new Florine.Nutrient() { Name = kvp.Key.Name } },
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
		/*
		private string _serializeNutrientSet(Florine.NutrientSet target)
		{
		}
		private Florine.NutrientSet _deserializeNutrientSet(string source)
		{
		}
		*/
		
        private bool _tryEnum<T>(NameValueCollection SourceData, string Key, out T Destination)
            where T: struct
        {
            Destination = default(T);
            if (null == SourceData[Key])
            {
                warnings.Add("Key [" + Key + "] not found");                
                return false;
            }
            if (!Enum.TryParse(SourceData[Key], out Destination))
            {
                warnings.Add("Value [" + SourceData[Key] + "] can not be parsed.");                
                return false;
            }
            return true;
        }
        /* WebFoundry */
        private bool _newGame = true;		
        private List<string> warnings = new List<string>();
        private System.Web.HttpCookieCollection _incookies;
        private System.Web.HttpCookieCollection _outcookies;
        private NameValueCollection _queryparams;
        private NameValueCollection _params;
        public WebFoundry(System.Web.UI.Page page)
        {
            _params = page.Request.Form;
            _queryparams = page.Request.QueryString;
            _incookies = page.Request.Cookies;
            _outcookies = page.Response.Cookies;
        }
        public void FinalizePage(GameState gamestate)
        {
            SaveGameState(gamestate);
        }        
		
		private Dictionary<string,System.Web.UI.Control> ControlLookup = new Dictionary<string, System.Web.UI.Control>();
        public System.Web.UI.Control RenderPage(GameState Status, IPage Source) 
		{ return RenderPage(Status, Source, false); }
        public System.Web.UI.Control RenderPage(GameState GameStatus, IPage Source, bool debugInfo)
        {
            System.Web.UI.Control Body;
            
            if (debugInfo)
            {
				return RenderDebugInfo(GameStatus, Source);
			}
                
			System.Web.UI.HtmlControls.HtmlForm director = new System.Web.UI.HtmlControls.HtmlForm()
			{
				Action = "."
			};
			director.EnableViewState = false;
			director.ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
			director.Style["border"] = "1px solid black";
			Body = director;
            
            if (Source.Background != null)
            {
                Body.Controls.Add(ImageFromObject(Source.Background, "", false));				
            } else {
				if(null != GameStatus && 
				   null != GameStatus.Player &&
				   null != GameStatus.Player.Avatar &&
				   null != GameStatus.Player.Avatar.Picture) {
                    System.Web.UI.HtmlControls.HtmlTable DisplayTable =
                        new System.Web.UI.HtmlControls.HtmlTable();
                    DisplayTable.Rows.Add(new System.Web.UI.HtmlControls.HtmlTableRow()
                    {
                        Cells = {
                            new System.Web.UI.HtmlControls.HtmlTableCell()
                            {
                                Controls = {
                                    ImageFromObject(
                                        GameStatus.Player.Avatar.Picture,
                                        "Player_Avatar_Picture",
                                        false
                                    )
                                }
                            },
                            new System.Web.UI.HtmlControls.HtmlTableCell()
                            {
                                Controls = {
                                    RenderNutrientBlock(GameStatus.Player.Nutrients,
														"Status", false
													   )
										}
                            },
                            new System.Web.UI.HtmlControls.HtmlTableCell()
                            {
                                Controls = {
                                    RenderNutrientBlock(GameStatus.CurrentDelta,
													   "Current", false
													   )
									}
                            },
                            new System.Web.UI.HtmlControls.HtmlTableCell()
                            {
                                Controls = {
                                    RenderNutrientBlock(GameStatus.DailyDelta,
													   "Change Today", true
													   )
                                }
                            }
                        }
                    });
					DisplayTable.Rows.Add(new System.Web.UI.HtmlControls.HtmlTableRow()
                    {
						Cells = {
							new System.Web.UI.HtmlControls.HtmlTableCell()
                            {
                                Controls = {
                                    new System.Web.UI.WebControls.Label() {
										Text = "<br/>Energy: " 
											+ GameStatus.Player.Energy.ToString()
									},
									new System.Web.UI.WebControls.Label() {
										Text = "<br/>Focus: " 
											+ GameStatus.Player.Focus.ToString()
									},
									new System.Web.UI.WebControls.Label() {
										Text = "<br/>Hunger: " 
											+ GameStatus.Player.Hunger.ToString()
									},
									new System.Web.UI.WebControls.Label() {
										Text = "<br/>kCal/target: " 
											+ GameStatus.Player.Calories.ToString()
											+ " / "
											+ GameStatus.Player.TargetCalories.ToString()
									},
                                }
                            }
						}
					});
                    Body.Controls.Add(DisplayTable);
                    
				}					
			}
            if (Source.Title != null) {
                Body.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("h3")
                {
                    InnerHtml = Source.Title
                });
            }
            if (Source.Message != null)
            {
                System.Web.UI.WebControls.Panel MessagePanel = new System.Web.UI.WebControls.Panel();
                MessagePanel.Controls.Add(new System.Web.UI.WebControls.Label() { Text = Source.Message });
                Body.Controls.Add(MessagePanel);
            }

            System.Web.UI.ControlCollection form = Body.Controls;
						
			ControlLookup["CurrentPage"] = new System.Web.UI.HtmlControls.HtmlInputHidden()
			{
				ID = "stg_curpage",
				Value = Source.MainType.ToString(),
				ViewStateMode = System.Web.UI.ViewStateMode.Disabled
			};
			form.Add(ControlLookup["CurrentPage"]);
			
			/* Special Handling for Character Creation */			
            if(Source.MainType == GameState.PageType.Char_Creation) {			
				form.Add(CharCreateControls(Source));				
			}
						
			if(null != Source.PrimaryOptions) {
			    form.Add(RenderOptions(Source.PrimaryOptions, true));
			}
			if(null != Source.AppliedOptions) {
			    form.Add(RenderOptions(Source.AppliedOptions, false));
			}
			
			return Body;
			
		}
		
		public System.Web.UI.WebControls.Panel RenderOption(IGameOption opt, bool Selectable) {
			System.Web.UI.WebControls.Panel optionPanel =
					new System.Web.UI.WebControls.Panel();
				
				if(Selectable) {
					optionPanel.Controls.Add(
						new System.Web.UI.HtmlControls.HtmlGenericControl("label") {
                            ID = "opt_label_" + opt.OptionName,
                            Controls = {
                                new System.Web.UI.HtmlControls.HtmlInputCheckBox() {
                                    Name = opt.OptionName,
                                    ID = "opt_" + opt.OptionName,
									ViewStateMode = System.Web.UI.ViewStateMode.Disabled
                                },
								ImageFromObject(opt.Picture, "", Selectable)
							}
						}
					);
				} else {
				    optionPanel.Controls.Add(ImageFromObject(opt.Picture, "", Selectable));
				}
			return optionPanel;
		}
		
		private List<IGameOption> GetOptionList(IGameOptionSet options)
		{
			List<IGameOption> renderList = new List<IGameOption>();
			if(null == options) { return renderList; }
			foreach (IGameOption opt in options)
            {				
				// Only one level for now, and just flatten.
				if(null == opt.SubOptions) {
					renderList.Add(opt);
				} else {
					foreach(IGameOption o2 in opt.SubOptions) {
						renderList.Add(o2);
					}
				}
			}
			return renderList;
		}
		public System.Web.UI.Control RenderOptions(IGameOptionSet options, bool Selectable)
        {	
            int OptionCount = 0;
			int OptionColumn = 0;
			System.Web.UI.HtmlControls.HtmlTable OptTab = 
					new System.Web.UI.HtmlControls.HtmlTable();
			List<IGameOption> renderList = GetOptionList(options);
            
			foreach (IGameOption opt in renderList) 
			{
				++OptionCount;				
                System.Web.UI.WebControls.Panel optionPanel =
					RenderOption(opt, Selectable);
											
				if(OptionColumn == 0) {
					OptTab.Rows.Add(new System.Web.UI.HtmlControls.HtmlTableRow());
					OptionColumn++;			
				} else {
				    OptionColumn = 0;
				}
			
				OptTab.Rows[OptTab.Rows.Count-1].Cells.Add(
					new System.Web.UI.HtmlControls.HtmlTableCell()
					{
						Controls = { optionPanel }
					}					
				);
            }
		
			System.Web.UI.WebControls.Panel panel = new System.Web.UI.WebControls.Panel();
			if(OptionCount > 0) {
				panel.Controls.Add(OptTab);
			}
			if(Selectable) {
				if (null != options.SelectionLimit)
				{
					ControlLookup["SelectLimit"] = new System.Web.UI.HtmlControls.HtmlInputHidden()
					{
						ID = "SelectLimit",
						Value = options.SelectionLimit.ToString(),
						ViewStateMode = System.Web.UI.ViewStateMode.Disabled
					};
					panel.Controls.Add(ControlLookup["SelectLimit"]);
				}		

				if (null != options.Finalizer)
				{
					panel.Controls.Add(new System.Web.UI.HtmlControls.HtmlInputSubmit()
							 {
								 ID = "Finalizer",
								 Value = options.Finalizer.OptionName,
								 ViewStateMode = System.Web.UI.ViewStateMode.Disabled
							 });
				}
			}
            return panel;
        }
		
		protected System.Web.UI.Control CharCreateControls(IPage Source) {
			System.Web.UI.WebControls.Panel p = new System.Web.UI.WebControls.Panel();
			
			p.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
			
			/* Name */
			 System.Web.UI.WebControls.TextBox NameBox = new System.Web.UI.WebControls.TextBox()
				{
					ID = "Avatar_Name",
					Text = "Faerina",
					BorderStyle = System.Web.UI.WebControls.BorderStyle.None,
					Width = System.Web.UI.WebControls.Unit.Percentage(100)
				};
			NameBox.Font.Size = System.Web.UI.WebControls.FontUnit.Larger;
			p.Controls.Add(NameBox);
			
			List<string> AvatarParts = new List<string>() {
				"skin", "wings","hair","pants","shirt","shoes"
			};
			
			System.Web.UI.WebControls.Panel combined = new System.Web.UI.WebControls.Panel()
			{
				CssClass = "OverlapContainer",
				Width = System.Web.UI.WebControls.Unit.Pixel(262),
				Height = System.Web.UI.WebControls.Unit.Pixel(332),
			};
			
			foreach(string part in AvatarParts) {
				combined.Controls.Add(new System.Web.UI.WebControls.Image() 
				{
					ImageUrl = "Images/Avatar/char_" + part + ".png",
					CssClass = "OverlapImage img_" + part				
				});
			}
			System.Web.UI.WebControls.Panel scom = new System.Web.UI.WebControls.Panel() {
				HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center
			};
			scom.Controls.Add(combined);
			p.Controls.Add(scom);
			Dictionary<string, int> BaseColor = new Dictionary<string, int>
			{
				{ "skin", 195},
				{ "wings", 193 },
				{ "hair", 61 },
				{ "pants", 82},
				{ "shirt", 154 },
				{ "shoes", 45 }
			};
			Dictionary<string, double> PartBrightBase = new Dictionary<string, double>
			{
				{ "skin", .3},
				{ "wings", .3 },
				{ "hair", .2 },
				{ "pants", .4},
				{ "shirt", .3 },
				{ "shoes", .2 }
			};
			Dictionary<string, double> PartBright = new Dictionary<string, double>
			{
				{ "skin", .2},
				{ "wings", .2 },
				{ "hair", .5 },
				{ "pants", .4},
				{ "shirt", .2 },
				{ "shoes", .5 }
			};
			foreach(string part in AvatarParts) {
				//inline block
				System.Web.UI.WebControls.Panel icom = new System.Web.UI.WebControls.Panel()
				{
					CssClass = "TileImage"
				};
				System.Web.UI.HtmlControls.HtmlTable Tabular = 
					new System.Web.UI.HtmlControls.HtmlTable();				
				int HueCount = 5;
				int BrightCount = 5;
				int HueSpacing = 360 / HueCount;
				int HueBase = 45;
				if(part == "skin") { HueBase = -HueBase; }
				for(int row_idx = 0; row_idx < HueCount+1; ++row_idx) 
				{
					System.Web.UI.HtmlControls.HtmlTableRow row = 
						new System.Web.UI.HtmlControls.HtmlTableRow();
					if( row_idx < HueCount )
					{
						//Hue
						row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell()
							{
								Controls = {
									Stylize(new System.Web.UI.WebControls.Button() {
											UseSubmitBehavior = false,
											Text = " ",
										    BorderStyle =
												System.Web.UI.WebControls.BorderStyle.None,
											OnClientClick="changeHue('" 
												+ part 
												+ "'," 
												+ ((HueSpacing*(row_idx-2)) + HueBase) 
												+ ");//",
											BackColor = System.Drawing.Color.FromArgb(
												BaseColor[part],BaseColor[part],BaseColor[part]
											)
									},
										"filter_brightness_" + part,
										"sepia(1)"
										    +" hue-rotate(" 
											+ ((HueSpacing*(row_idx-2)) + HueBase) 
											+ "deg)"
									)
								}
							});
						if(row_idx == 0) {							
							System.Web.UI.HtmlControls.HtmlInputHidden partHue = 
								new System.Web.UI.HtmlControls.HtmlInputHidden()
							{
								ID = "val_h_" + part,
								Value = HueBase.ToString(),
								ViewStateMode = System.Web.UI.ViewStateMode.Disabled
							};
							System.Web.UI.HtmlControls.HtmlInputHidden partBri = 
								new System.Web.UI.HtmlControls.HtmlInputHidden()
							{
								ID = "val_b_" + part,
								Value = ( PartBrightBase[part] + PartBright[part] * 2 ).ToString(),
								ViewStateMode = System.Web.UI.ViewStateMode.Disabled
							};
							System.Web.UI.WebControls.Image img =
								new System.Web.UI.WebControls.Image() 
								{
									ImageUrl = "Images/Avatar/Zoom/char_" + part + ".png",
									CssClass = "img_" + part									
								};
									
							
							row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell()
							{
								ColSpan = HueCount,
								RowSpan = BrightCount,
								Controls = { img, partHue, partBri }
							});
						}
					} else {
						row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell());
						for(int col_idx = 0; col_idx < BrightCount; ++col_idx)
						{
							row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell()
							{
								Controls = {
									Stylize(
										new System.Web.UI.WebControls.Button() {
											UseSubmitBehavior = false,
											Text = " ",
											BorderStyle =
												System.Web.UI.WebControls.BorderStyle.None,
											OnClientClick="changeBrightness('" 
												+ part 
												+ "'," 
												+ ( PartBrightBase[part] + PartBright[part] * col_idx ) 
												+ ");//",
											BackColor = System.Drawing.Color.FromArgb(
												BaseColor[part],BaseColor[part],BaseColor[part]
												)												
										},
										"filter_hue_" + part,
										"sepia(1) brightness("
										+ ( PartBrightBase[part] + PartBright[part] * col_idx )
										+ ");"
									)										
								}
							});
						}
					}
					Tabular.Rows.Add(row);					
				}
				icom.Controls.Add(Tabular);				
				p.Controls.Add(icom);
			}
			
			
			return p;
		}
		
		private System.Web.UI.WebControls.WebControl Stylize(System.Web.UI.WebControls.WebControl c, string style, string val)
		{
			
			c.Style["filter"] = val;
			return new System.Web.UI.WebControls.Panel()
			{
				CssClass = style,
				Controls = { c }
			};
		}

		/* Update Page */
        public void UpdatePage(IPage Source)
        {
			
			/* Current Page */
			System.Web.UI.HtmlControls.HtmlInputHidden cpage = 
				(System.Web.UI.HtmlControls.HtmlInputHidden)(ControlLookup["CurrentPage"]);
			cpage.Value = Source.MainType.ToString();
					
			/* Select Limit */
            IGameOptionSet options = Source.PrimaryOptions;            
            if (null != options.SelectionLimit)
            {
				System.Web.UI.HtmlControls.HtmlInputHidden sellim = 
				(System.Web.UI.HtmlControls.HtmlInputHidden)(ControlLookup["SelectLimit"]);
                sellim.Value = options.SelectionLimit.ToString();					
            }
        }
		
		
		/* End Update */
        public IGameOption GetChosenOption(Controller ctrl)
        {
            if (_newGame) {                
                return null;
            }
			
			/* Resubmitted Page? */
			if (
				null == _params["stg_curpage"]
				|| _params["stg_curpage"] !=  ctrl.GetCurrentPage().MainType.ToString()
			)
			{
				return null;
			}
			
			if(null == _params["Finalizer"]) {
				return null;
			}
			
			if(_params["stg_curpage"] == "Char_Creation") {
				return new CharacterCreationOption(_params);
			}
            Florine.IGameOptionSet options = ctrl.GetCurrentPage().PrimaryOptions;
            _WebOptionGroupSelect ResultOption = new _WebOptionGroupSelect();
            foreach (IGameOption opt in options)
            {
                if (null != _params["opt_" + opt.OptionName]
                    && "on" == _params["opt_" + opt.OptionName]
                    )
                {
                    ResultOption.ChosenOptions.Add(opt);
                }
            }
            return ResultOption;                
        }

        private class _WebOptionGroupSelect : List<IGameOption>, IGameOption, IGameOptionSet
        {
            public string OptionName { get { return "" + String.Join(" & ", ChosenOptions); } }
            public IImage Picture { get { return null; } }
			public int SelectionLimit { get { return 0; } }
			public IGameOption Finalizer { get { return null; } }
            public List<Florine.IGameOption> ChosenOptions {
				get {
					return this;
				}
			}            
			
			public IGameOptionSet SubOptions {
				get 
				{
					return this;
				}
			}

            public void ImpactPlayer(Florine.Player target)
            {
                foreach (Florine.IGameOption opt in ChosenOptions)
                {
                    opt.ImpactPlayer(target);
                }
            }
			
			public void AdjustNutrients(Florine.NutrientSet target)
            {
                foreach (Florine.IGameOption opt in ChosenOptions)
                {
                    opt.AdjustNutrients(target);
                }
            }

        }

        private class _WebImage : IImage
        {
            public int ImageKey { get; set; }
            public string URI;
        }
        public override IImage LoadImageFromFood(Food Parent)
        {
            if (null != Parent.OptionPicture) {
                return Parent.OptionPicture;
            }
            string PreMapUrl = "Images/Food/" + Parent.Name.Replace(' ', '_') + ".png";
            //if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(PreMapUrl))) {
                return new _WebImage()
                {
                    ImageKey = 12345,
                    URI = PreMapUrl
                };
            //}
            return null;
        }

        public System.Web.UI.Control RenderNutrientBlock(
			Florine.NutrientSet Nutrients, 
			string Title,
			bool IncludeNutrientDetails
		)
        {			
			System.Web.UI.WebControls.Panel pPanel = new System.Web.UI.WebControls.Panel();
			pPanel.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
			pPanel.BorderWidth = new System.Web.UI.WebControls.Unit("1 px");
            System.Web.UI.HtmlControls.HtmlTable tab = new System.Web.UI.HtmlControls.HtmlTable();			
			if(null == Nutrients) { return tab; }
			if(Nutrients.Count == 0) { return tab; }
			if(null != Title) {
				tab.Rows.Add(new System.Web.UI.HtmlControls.HtmlTableRow()
				{
					Cells = {
						new System.Web.UI.HtmlControls.HtmlTableCell()
						{
							ColSpan = 2,
							Controls = 
							{
								new System.Web.UI.HtmlControls.HtmlGenericControl("b")
									{ InnerHtml = Title }
							}
						}
					}
				});
			}
            foreach (KeyValuePair<Florine.Nutrient, Florine.NutrientAmount> kvp in Nutrients)
            {
                tab.Rows.Add(new System.Web.UI.HtmlControls.HtmlTableRow()
                {
                    Cells = {
                        new System.Web.UI.HtmlControls.HtmlTableCell()
                        {
                            Controls =
                            {
                                new System.Web.UI.WebControls.Literal() { Text = kvp.Key.Name.ToString() }
                            }
                        },
                        new System.Web.UI.HtmlControls.HtmlTableCell()
                        {
                            Controls =
                            {
                                new System.Web.UI.WebControls.Literal() { Text = kvp.Value.ToString("N2") }
                            }
                        }
                    }
                });
				if(IncludeNutrientDetails 
				  && null != kvp.Key.Units
				  ) {
					tab.Rows[tab.Rows.Count-1].Cells.Add(
						new System.Web.UI.HtmlControls.HtmlTableCell()
                        {
                            Controls =
                            {
                                new System.Web.UI.WebControls.Literal() {
									Text = "/" 
										+ ((null == kvp.Key.DailyTarget)?
										   "??"
										   :kvp.Key.DailyTarget.ToString())
										+kvp.Key.Units.ToString()
								}
                            }
                        }
					);
				}
            }
			pPanel.Controls.Add(tab);
			return pPanel;
        }

        public System.Web.UI.Control ImageFromObject(IImage Source, string ID, bool AsButton) {
            if (null == Source)
            {
                return new System.Web.UI.WebControls.Literal() { Text = "" };
            }
            if (Source is _WebImage)
            {
                _WebImage PlatformImage = (_WebImage)Source;
                if (AsButton)
                {
                    //return new System.Web.UI.WebControls.ImageButton() { ImageUrl = PlatformImage.URI, ID = ID };
                }
                return new System.Web.UI.WebControls.Image() { ImageUrl = PlatformImage.URI };
            }
			if(Source is _LayeredWebImage) {
				return ((_LayeredWebImage)Source).GetControl();
			}
            return new System.Web.UI.WebControls.Literal() { Text = Source.ToString() };
            
        }
        private class _LayeredWebImage : IImage
        {
            public int ImageKey { get; set; }
            public string URI = "";
			public string hue = "0";
			public string brightness = "1";
			public string contrast = "1";
			public int height = 100;
			public int width = 100;
			public _LayeredWebImage Next;
			
			public System.Web.UI.Control GetControl() {
				System.Web.UI.WebControls.Panel p = new System.Web.UI.WebControls.Panel()
				{
					CssClass = "OverlapContainer",
					Width = System.Web.UI.WebControls.Unit.Pixel(width),
					Height = System.Web.UI.WebControls.Unit.Pixel(height),
				};
				return AddNext(p);
			}
			private System.Web.UI.Control AddNext(System.Web.UI.Control p) {
				System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image()
				{
					ImageUrl = URI,
					CssClass = "OverlapImage",
				};
				img.Style["filter"] = "sepia(1) "
					+ "hue-rotate(" + hue + "deg) "
					+ "brightness(" + brightness + ") "
					+ "contrast(" + contrast + ")";
				p.Controls.Add(img);
				if(null != Next) {
					Next.AddNext(p);
				}
				return p;
			}
        }
		public System.Web.UI.Control RenderDebugInfo(GameState GameStatus, IPage Source)
        {
            System.Web.UI.Control Body;
                        
			System.Web.UI.WebControls.Panel pPanel = new System.Web.UI.WebControls.Panel();
			Body = pPanel;
			Body.Controls.Add(new System.Web.UI.WebControls.Label()
							  {
								  Text = "<pre>" 
									  + FlorineJSON.JsonHelper.FormatJson(
									  		FlorineJSON.JSONUtil.SerializeGameState(
									  			GameStatus,
									  			false
								  			)
									  	)
									  + "</pre>"
							  });
			pPanel.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
			pPanel.BorderWidth = new System.Web.UI.WebControls.Unit("1 px");
			foreach (string w in warnings)
			{
				Body.Controls.Add(new System.Web.UI.WebControls.Label()
								  {
									  Text = "Warning: " + w,
									  BackColor = System.Drawing.Color.MistyRose
								  });
			}
			Body.Controls.Add(new System.Web.UI.WebControls.Label()
							  {
								  Text = Source.MainType.ToString() + "(" + Source.SubType.ToString() + ")"
									  + "<pre>" + Source.ToString() + "</pre>"

							  });
			Body.Controls.Add(new System.Web.UI.HtmlControls.HtmlAnchor()
							  {
								  HRef = ".?reset=1",
								  InnerText = "DeForm"
							  });				
			Body.Controls.Add(new System.Web.UI.WebControls.Label()
							  {
								  Text = "[" 
									  + _params["stg_curpage"] 
									  + " -> " 
									  + Source.MainType.ToString() 
									  + "]"
							  });			
			Body.Controls.Add(new System.Web.UI.WebControls.Label()
							  {
								  Text = "{<ul><li>" 
									  + String.Join("<li>",
													GetOptionList(Source.PrimaryOptions))
									  + " </ul>-><ul><li> " 
									  + String.Join("<li>",
													GetOptionList(Source.AppliedOptions))
									  + "</ul>}"
							  });            
            System.Web.UI.HtmlControls.HtmlGenericControl dbgList = new System.Web.UI.HtmlControls.HtmlGenericControl("ul");
			for (int i = 0; i < _params.Count; ++i)
			{
				System.Web.UI.HtmlControls.HtmlGenericControl li = new System.Web.UI.HtmlControls.HtmlGenericControl("li");
				li.Controls.Add(new System.Web.UI.WebControls.Label() { Text = _params.GetKey(i) });
				string[] pval = _params.GetValues(i);

				System.Web.UI.HtmlControls.HtmlGenericControl subList = new System.Web.UI.HtmlControls.HtmlGenericControl("ul");
				foreach (string s in pval)
				{
					subList.Controls.Add(new System.Web.UI.WebControls.Label() { Text = s });
				}
				li.Controls.Add(subList);

				dbgList.Controls.Add(li);
			}                
			Body.Controls.Add(dbgList);
			return Body;

		}
		private class CharacterCreationOption : Florine.IGameOption
		{
			public string OptionName { get { return "Character Creation Settings"; } }
			public IImage Picture { get { return null; } }
			// Actually return colors here.
			public IGameOptionSet SubOptions { get { return null; } }
			private string _name = "";
			private _LayeredWebImage _pic = null;
			
			public CharacterCreationOption() { }
			public CharacterCreationOption(NameValueCollection dataSet)
			{
				_name = dataSet["Avatar_Name"];
				List<string> AvatarParts = new List<string>() {
					"skin", "wings","hair","pants","shirt","shoes"
				};
					
				_LayeredWebImage next_img = null;
				foreach(string part in AvatarParts) 
				{
					_LayeredWebImage partimg = new _LayeredWebImage() 
					{
						URI = "Images/Avatar/char_" + part + ".png",
						height = 332,
						width = 262,
						hue = dataSet["val_h_" + part],
						brightness = dataSet["val_b_" + part]							
					};
					
					if(null == _pic) {
						_pic = partimg;
						next_img = partimg;
					} else {
						next_img.Next = partimg;
						next_img = next_img.Next;
					}					
				}				
			}
									  
									  
			public void ImpactPlayer(Florine.Player target) 
			{
				target.Name = _name;
				target.Avatar.Picture = _pic;
			}
			public void AdjustNutrients(Florine.NutrientSet n) { }
		}

    }
}
