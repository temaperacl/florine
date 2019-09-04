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
                )
            {
                return base.LoadGameState();
            }
            NameValueCollection SourceData = _incookies["florine"].Values;

            GameState oldGame = new GameState();
            GameState.PageType mainType;
            GameState.PageSubType subType;

            // Main Page Point
            if (!(
                   _tryEnum(SourceData, "Step", out mainType)
                && _tryEnum(SourceData, "SubStep", out subType)
                ) )
            {
                return base.LoadGameState();
            }
            
            oldGame.SetPage(mainType, subType);

            // Player      
            _newGame = false;
            return oldGame;
        }        

        public override bool SaveGameState(GameState gs)
        {
            System.Web.HttpCookie outData = new System.Web.HttpCookie("florine");
            outData.Values["Step"] = gs.CurrentPage.MainType.ToString();
            outData.Values["SubStep"] = gs.CurrentPage.SubType.ToString();
            _outcookies.Set(outData);
            return true; ;
        }
        /* Misc Support */
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
        public System.Web.UI.Control RenderPage(IPage Source) { return RenderPage(Source, false); }
        public System.Web.UI.Control RenderPage(IPage Source, bool debugInfo)
        {
            System.Web.UI.Control Body;
            
            if (debugInfo)
            {
                System.Web.UI.WebControls.Panel pPanel = new System.Web.UI.WebControls.Panel();
                Body = pPanel;
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
            }
            else
            {
                System.Web.UI.HtmlControls.HtmlForm director = new System.Web.UI.HtmlControls.HtmlForm()
                {
                    Action = "."
                };
                director.Style["border"] = "1px solid black";
                Body = director;
                
            }
            
            if (Source.Background != null)
            {
                Body.Controls.Add(ImageFromObject(Source.Background));
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

            // ToDo: NutrientState/NutrientDelta;

            IGameOptionSet options = Source.PrimaryOptions;
            
            if (null != options.SelectionLimit)
            {
                form.Add(new System.Web.UI.HtmlControls.HtmlInputHidden()
                {
                    ID = (debugInfo?"dbg_":"") + "SelectLimit",
                    Value = options.SelectionLimit.ToString()
                });
            }
            //int OptionCount = options.Count;            
            foreach (IGameOption opt in options)
            {                
                System.Web.UI.WebControls.Panel optionPanel = new System.Web.UI.WebControls.Panel()
                {
                    Controls = {
                        new System.Web.UI.HtmlControls.HtmlInputCheckBox() {
                            Name = opt.OptionName,
                            ID = (debugInfo?"dbg_":"") + opt.OptionName
                        },
                        new System.Web.UI.WebControls.Label() { Text = opt.OptionName }
                    }
                };
                form.Add(optionPanel);

            }

            if (null != options.Finalizer)
            {
                form.Add(new System.Web.UI.HtmlControls.HtmlInputSubmit()
                {
                    Value = options.Finalizer.OptionName
                });
            }

            // ToDo: PrimaryOptions
            if (debugInfo)
            {
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
            }
            return Body;
        }
        public IGameOption GetChosenOption()
        {
            if (_newGame) {
                
                return null;
            }
            
            return new HardcodedEmptyOption("---");
        }

        public System.Web.UI.WebControls.Image ImageFromObject(IImage Source) {
            return new System.Web.UI.WebControls.Image();
        }


    }
}
