using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florine;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public class MealResultLayout : PageLayout
    {
       private class FlorineSkiaCVWrap : SKCanvasView {
           public IFlorineSkiaConnectable FlorineObj { get; set; }
           public FlorineSkiaCVWrap(IFlorineSkiaConnectable Conn) : base() {
               FlorineObj = Conn;
               FlorineObj.ConnectCanvasView(this);
           }
       }

       private SKCanvasView _addGradientBar(
           double Min,
           double Target,
           double Max,
           double Current,
           bool CanHaveTooMuch,
           bool MaskExcess           
       )
       {
           ImageGradient IG = new ImageGradient();
           float Center = (float)((Target-Min) / (Max - Min));
           float CurPoint = (float)((Current-Min) / (Max - Min));
           if(CanHaveTooMuch) 
           {
               IG.Details[(float)Min] = new SKColor(250,0,0);
               IG.Details[Center] = new SKColor(0,250,0);
               IG.Details[(float)Max] = new SKColor(250,0,0);
           } else {
               IG.Details[(float)Min] = new SKColor(250,0,0);
               IG.Details[Center] = new SKColor(125,125,0);
               IG.Details[(float)Max] = new SKColor(0,250, 0);
           }

           if(MaskExcess) {
               IG.BarWidth = CurPoint;
           } else {
               IG.IndicatorLineLoc = CurPoint;
           }
           IG.BorderSize = 5;

           return new FlorineSkiaCVWrap(IG);
           return BarCanvas;
       }

        // Allow for pre-component rendering layout
        protected virtual void PreLayout(
            bool IsTall,
            Grid grid,
            Controller  GameController,
            IPlatformFoundry GameFoundry
        ) { 
            return;
        }

        //Allow for post-component rendering layout
        protected virtual void PostLayout(
            bool IsTall,
            Grid grid,
            Controller  GameController,
            IPlatformFoundry GameFoundry
        ) {
            IPage CurrentPage = GameController.CurrentState.CurrentPage;
            Player PC = GameController.CurrentState.Player; 

            if(IsTall) {
                // Calories
                view CalorieView = _addGradientBar(
                     0,                    
                     PC.TargetCalories
                     PC.TargetCalories * 2,
                     PC.Calories,
                     true,
                     false
                );
                grid.Children.Add(CalorieView,4,2,8,3);
            }

            return;
        }

        /*
        public virtual SKRectI GetResolution(
            Dictionary<PageComponentType, int> ComponentCounts,
            bool IsTall   
        ){
            if (IsTall)
            {
                return new SKRectI(0,0,8,12);
            } else {
                return new SKRectI(0,0,15,7);
            }
        }
        */

        //TBD: Wide
        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
                     //Tall - 12x7
                     //   01234567
                     //   |      |
                     // 0-HHHHRRRR
                     // 1 HHHHRRRR 1
                     // 2 HHHHRRRR 2
                     // 3-HHHHHHHH-3   1          2 -- 6
                     // 4 |TTTTT| 4  01234567    01234567
                     // 5/OOOOOOO\5    AAAA      AAAABBBB
                     // 6\OOOOOOO/6    AAAA      AAAABBBB
                     // 7/OOOOOOOO\7   BBBB      CCCCDDDD
                     // 8\OOOOOOOO/8   BBBB      CCCCDDDD
                     // 9/OOOOOOOO\9   CCCC        EEEE
                     // A\OOOOOOOO/10  CCCC        EEEE
                     // B | <FF> | 11               
                     //   | |  | | 
                     //   012345678
                     switch(t) {
                         case PageComponentType.Option:
                             PlaceOption(grid,
                                         new SKRectI(0,0,4,6),
                                         new SKRectI(0,0,4,2),
                                         CurrentOption,
                                         OptionCount,
                                         v
                             );
                             ++CurrentOption;
                             return
                     }
             base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
        }        
    }    
}
