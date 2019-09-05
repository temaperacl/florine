using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Florine;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    public class SkiaSharpFormsFoundry : FlorineHardCodedData.HardCodedDataFoundry // : IPlatformFoundry
    {
        // Web Overrides
        public override GameState LoadGameState() {
            // PageType, PageSubType
            //

            GameState oldGame = new GameState();
            GameState.PageType mainType;
            GameState.PageSubType subType;

            // Load Vars;
            if(1)
            {
                return base.LoadGameState();
            }
            
            oldGame.SetPage(mainType, subType);

            // Player      
            return oldGame;
        }        

        public override bool SaveGameState(GameState gs)
        {
            //System.Web.HttpCookie outData = new System.Web.HttpCookie("florine");
            //outData.Values["Step"] = gs.CurrentPage.MainType.ToString();
            //outData.Values["SubStep"] = gs.CurrentPage.SubType.ToString();
            //_outcookies.Set(outData);
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

        /* SkiaSharpFormsFoundry */
        public ContentPage RenderPage(IPage Source) {
        }
        

        private class _SkiaFlorinePage {
            public _SkiaFlorinePage(IPage Source) {
            }
        }
    }
}
