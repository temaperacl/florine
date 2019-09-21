using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Florine;
using SkiaSharp;
using Xamarin.Forms;


namespace FlorineSkiaSharpForms
{
    public class SkiaSharpFormsFoundry : FlorineHardCodedData.HardCodedDataFoundry // : IPlatformFoundry
    {
        public SkiaSharpFormsFoundry() { }
        // Web Overrides
        public override GameState LoadGameState() {
            // PageType, PageSubType
            //

            GameState oldGame = new GameState();
            GameState.PageType mainType;
            GameState.PageSubType subType;

            // Load Vars;
            return base.LoadGameState();
            
           // oldGame.SetPage(mainType, subType);

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

        /* SkiaSharpFormsFoundry */
        public ContentPage RenderPage(IPage Source) {
            return null;
        }
        

        private class _SkiaFlorinePage {
            public _SkiaFlorinePage(IPage Source) {
            }
        }
    }
}
