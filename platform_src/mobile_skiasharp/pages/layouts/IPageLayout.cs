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
        Footer
    }

    public abstract class PageLayout
    {
        //Todo: Grid Allocation Size, OptSize Usage
        protected virtual void PlaceOption(
                                 Grid grid, 
                                 SKRect GridAllocation,
                                 SKRect OptSize,
                                 int CurrentOption, 
                                 int OptionCount, 
                                 View Option
        ) {
            int GridLeft = GridAllocation.Left;
            int GridTop = GridAllocation.Top;
             if(OptionCount == 1) {
                 grid.Children.Add( 
                                   Option,  
                                   GridLeft + 2,  
                                   GridLeft + 6,  
                                   GridTop  + 0,  
                                   GridTop  + 2 );
             } else {
                 if(
                    (CurrentOption == OptionCount -1) // Last Option
                    && (0 == CurrentOption % 2) //And Odd (Base 0)
                 ) {
                     //Center
                     grid.Children.Add( Option,
                                        GridLeft + 2,
                                        GridLeft + 6,
                                        GridTop  + 0 + (int)(CurrentOption / 2) * 2,
                                        GridTop  + 2 + (int)(CurrentOption / 2) * 2 );
                 } else {
                     //2-by
                     grid.Children.Add( Option,
                                        GridLeft + 0 + (CurrentOption % 2) * 4,
                                        GridLeft + 4 + (CurrentOption % 2) * 4,
                                        GridTop  + 0 + (int)(CurrentOption / 2) * 2,
                                        GridTop  + 2 + (int)(CurrentOption / 2) * 2 );
                 }
             }
        }

        protected virtual SKRect GetResolution(
            Dictionary<PageComponentType, int> ComponentCounts),
            bool IsTall
        {
            if (IsTall)
            {
                return new SKRect(0,0,8,12);
            } else {
                return new SKRect(0,0,15,7);
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
                            grid
                            p.PCType,
                            p.PCView,
                            CurrentOption, 
                            OptionCount
                     );
                 } else {
                     LayoutComponentWide(
                            grid
                            p.PCType,
                            p.PCView,
                            CurrentOption, 
                            OptionCount
                     );
                 }  // End Wide

        }

        private virtual void LayoutComponentWide(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
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
             switch(p.PCType) {
                 case PageComponentType.Header:
                     grid.Children.Add( p.PCView,  0,  7,  0,  4 );
                     break;
                case PageComponentType.Footer:
                     grid.Children.Add( p.PCView,  2,  5,  6, 7 );
                     break;
                 case PageComponentType.Option:
                     PlaceOption(grid
                                 new SKRect( 8, 1, 14, 6),
                                 new SKRect( 0, 0, 4,  2),
                                 CurrentOption,
                                 OptionCount,
                                 p.PCView
                     );
                     ++CurrentOption;
                     break;
             } 
        }

        private virtual void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
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
                             grid.Children.Add( v,  0,  8,  0,  4 );
                             break;
                        case PageComponentType.Footer:
                             grid.Children.Add( v,  2,  6, 11, 12 );
                             break;
                        case PageComponentType.Message:
                             grid.Children.Add( v,  0,  8,  4,  5 );
                             break;
                         case PageComponentType.Option:
                             PlaceOption(grid
                                         new SKRect(0,5,7,10),
                                         new SKRect(0,0,4,2),
                                         CurrentOption,
                                         OptionCount,
                                         v
                             );
                             ++CurrentOption;
                             break;
                     }
        }
    }    
}
