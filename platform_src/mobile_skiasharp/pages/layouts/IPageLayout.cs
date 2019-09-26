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
    public class PageComponent {
        public PageComponentType PCType { get; set; }
        public View              PCView { get; set; }
        public PageComponent(PageComponentType pcType, View pcView) 
        {
            PCType = pcType;
            PCView = pcView;
        }
    }
    public enum PageComponentType {
        Header,
        Option,
        Message,
        Footer,
        Player
    }

    public abstract class PageLayout
    {
        // Allow for pre-component rendering layout
        protected virtual void PreLayout(
            bool IsTall,
            Grid grid,
            Controller  GameController,
            IPlatformFoundry GameFoundry
        ) { return; }

        //Allow for post-component rendering layout
        protected virtual void PostLayout(
            bool IsTall,
            Grid grid,
            Controller  GameController,
            IPlatformFoundry GameFoundry
        ) { return; }

        //Todo: Grid Allocation Size, OptSize Usage
        protected virtual void PlaceOption(
                                 Grid grid,
                                 SKRectI GridAllocation,
                                 SKRectI OptSize,
                                 int CurrentOption,
                                 int OptionCount,
                                 View Option
        )
        {
            int GridLeft = GridAllocation.Left;
            int GridTop = GridAllocation.Top;
            int OptCols = GridAllocation.Width / OptSize.Width;
            int OptRows = GridAllocation.Height / OptSize.Height

            if (
            (CurrentOption == OptionCount - 1) // Last Option
            && (0 == CurrentOption % 2) //And Odd (Base 0)
            )
            {
                //Center
                grid.Children.Add(Option,
                                GridLeft + GridAllocation.Width/2 - OptSize.Width /2,
                                GridLeft + GridAllocation.Width/2 + OptSize.Width /2,
                );
            }
            else
            {
                //Todo : Add bottom check.
                grid.Children.Add(Option,
                                GridLeft + (CurrentOption % OptCols) * OptSize.Width,
                                GridTop + (CurrentOption / OptCols) * OptSize.Height
                );
            }
            Grid.SetColumnSpan(Option, OptSize.Width);
            Grid.SetRowSpan(Option, OptSize.Height);                

        }

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

        public virtual void  LayoutComponent(
                                             Grid grid,
                                             PageComponent p,
                                             int CurrentOption,
                                             int OptionCount,
                                             bool isTall
                                             )
        {
                 if(isTall)                  
                 {
                     LayoutComponentTall(
                            grid,
                            p.PCType,
                            p.PCView,
                            CurrentOption, 
                            OptionCount
                     );
                 } else {
                     LayoutComponentWide(
                            grid,
                            p.PCType,
                            p.PCView,
                            CurrentOption, 
                            OptionCount
                     );
                 }  // End Wide

        }

        protected virtual void LayoutComponentWide(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
             // 7x15 - "wide"
             //   0123456789ABCDE
             // 0 HHHHHHH  TTTTT
             // 1 HHHHHHH OOO OOO
             // 2 HHHHHHH OOO OOO
             // 3 HHHHHHH OOO OOO
             // 4 HHHHHHH OOO OOO
             // 5         OOO OOO
             // 6   FFF   OOO OOO
             //   012345678911111
             //             01234
             switch(t) {
                 case PageComponentType.Header:
                     grid.Children.Add( v,  0,  7,  0,  4 );
                     break;
                case PageComponentType.Footer:
                     grid.Children.Add( v,  2,  5,  6, 7 );
                     break;
                 case PageComponentType.Option:
                     PlaceOption(grid,
                                 new SKRectI( 8, 1, 14, 6),
                                 new SKRectI( 0, 0, 4,  2),
                                 CurrentOption,
                                 OptionCount,
                                 v
                     );
                     ++CurrentOption;
                     break;
             } 
        }

        protected virtual void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
                     //Tall - 12x7
                     //   01234567
                     //   |      |
                     // 0-HHHHHHHH-0
                     // 1 HHHHHHHH 1
                     // 2 HHHHHHHH 2
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
                         case PageComponentType.Header:
                             grid.Children.Add( v,  0,  8,  0,  12 );
                             break;
                        case PageComponentType.Footer:
                             grid.Children.Add( v,  2,  6, 11, 12 );
                             break;
                        case PageComponentType.Message:
                             grid.Children.Add( v,  0,  8,  4,  5 );
                             break;
                         case PageComponentType.Option:
                             PlaceOption(grid,
                                         new SKRectI(0,5,7,10),
                                         new SKRectI(0,0,4,2),
                                         CurrentOption,
                                         OptionCount,
                                         v
                             );
                             ++CurrentOption;
                             break;
                        case PageComponentType.Player:
                             grid.Children.Add( v, 7,2, 9, 3);
                             break;
                     }
        }
    }    
}
