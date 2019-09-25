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
        public SkiaSharpFormsFoundry() {            
        }
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
        public Controller GameController { get; set; }
        public SkiaFlorinePage Page { get; set; }
        public IPage PageFromIPage(IPage generic)
        {
            return new Florine_SkiaPage(generic, this, GameController);
        }
        /* SkiaSharpFormsFoundry */
        public View RenderPage(GameState CurrentState) {

            IPage Source = PageFromIPage(
                base.GetPage(
                    CurrentState.CurrentPage
                )
            );

            
            switch (CurrentState.CurrentPage.MainType)
            {
             //   case GameState.PageType.Start:
                    // TODO: Actual Switch                    
//                case GameState.PageType.Char_Creation:                    
//                case GameState.PageType.Day_Intro:                    
                case GameState.PageType.Select_Meal:
                    return Page.DefaultLayout(Source, CurrentState);
//                case GameState.PageType.Summarize_Meal:
//                case GameState.PageType.Select_Activity:
//                case GameState.PageType.Summarize_Activity:
//                case GameState.PageType.Summarize_Day:
                   
            }
            return Page.UndefinedLayout(Source, CurrentState);
        }
    }
}
