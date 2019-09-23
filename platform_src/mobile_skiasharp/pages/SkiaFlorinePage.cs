using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florine;

using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public class SkiaFlorinePage : ContentPage
    {
        public SkiaFlorinePage(Controller GameController, SkiaSharpFormsFoundry Foundry) : base()
        {
            Foundry.Page = this;
            Foundry.GameController = GameController;
            Content = Foundry.RenderPage(GameController.CurrentState);
        }

        private enum PageComponentType {
            Header,
            Option,            
            Footer
        }
        private class PageComponent {
            public PageComponentType PCType { get; set; }
            public View              PCView { get; set; }
            public PageComponent(PageComponentType pcType, View pcView) 
            {
                PCType = pcType;
                PCView = pcView;
            }
        }
        private List<PageComponent> _components = new List<PageComponent>();

        public void DefaultOptionLayout(
                                  IPage Source,
                                  GameState State,
                                  )
        {
	     var grid = new Grid();
             grid.SizeChanged += SizeChanged_Grid;
             _components.Clear();
             _readyIPage(Source);
             this.Content = grid;
        }

        private void _readyIPage(IPage Source)
        {

             if(null != Source.Background) {
                 SKCanvasView Header = new SKCanvasView();
                 _components.Add(
                     new PageComponent(
                         PageComponentType.Header,
                         Header
                     )
                 );
             }

            IGameOptionSet Opts = Source.PrimaryOptions;
            if(null != Opts) {
                foreach(IGameOption opt in Opts) 
                {
                    SKCanvasView Img = new SKCanvasView();
                    if(null != opt.Picture
                       && opt.Picture is AspectImage) {
                        ((AspectImage)(opt.Picture)).ConnectCanvasView(Img);
                    }
                     _components.Add(
                         new PageComponent(
                             PageComponentType.Option,
                             Img
                         )
                     );                                    
                }
                if(null != Opts.Finalizer) {
                    SKCanvasView Img = new SKCanvasView();
                    if(null != Opts.Finalizer.Picture
                       && Opts.Finalizer.Picture is AspectImage) {
                        ((AspectImage)(opt.Picture)).ConnectCanvasView(Img);
                    } else {
                        //?
                    }
                     _components.Add(
                         new PageComponent(
                             PageComponentType.Footer,
                             Img
                         )
                }
            }            
        }

        private void SizeChanged_Grid(object sender, EventArgs e)
        {        
            // Conceptually:
            //  [ Image= ]
            //  [--------]
            //  [ O ][ O ]
            //  [ O ][ O ]
            //  [ O ][ O ]
            //    [ DN ]            
            Grid grid = Content as Grid;
            if (grid == null) return;

	     float Height = Dim.Height;
	     float Width = Dim.Width;

             int Rows = 0;
             int Cols = 0;

	     if (Height > Width)
	     {
                 //Tall
                 Rows = 12;
                 Cols = 7;
	     } else {
                 // Wide/Square
                 Rows = 7;
                 Cols = 15;
             }

             double CellHeight = Height / Rows;
             double CellWidth = Width / Cols;

             for (int i = 0; i > Rows; ++i) 
             {
		 grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(CellHeight, GridUnitType.Absolute) });
             }
             for (int i = 0; i > Cols; ++i) 
             {
		 grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(CellWidth, GridUnitType.Absolute) });
             }

             // ************************************************** Prep
             int OptionCount = 0;
             foreach(PageComponent p in _components) 
             {
                 if (p.PCType == PageComponentType.Option) 
                 {
                     OptionCount++;
                 }
             }

             // ********************************************** * Layout
             int CurrentOption = 0;
             foreach(PageComponent p in _components) 
             {
                 if(Height > Width)                  
                 {
                     //Tall - 12x7
                     //   0123456
                     //   |     |
                     // 0-HHHHHHH-0
                     // 1 HHHHHHH 1
                     // 2 HHHHHHH 2
                     // 3-HHHHHHH-3   1 - 3   4 - 6
                     // 4 |TTTTT| 4  0123456 0123456
                     // 5/OOO OOO\5    AAA   AAA BBB
                     // 6\OOO OOO/6    AAA   AAA BBB
                     // 7/OOO OOO\7    BBB   CCC DDD
                     // 8\OOO OOO/8    BBB   CCC DDD
                     // 9/OOO OOO\9    CCC     EEE
                     // A\OOO OOO/10   CCC     EEE
                     // B +<FFF>+ 11               
                     //   | | | |
                     //   0123456
                     switch(p.PCType) {
                         case PageComponentType.Header:
                             grid.Children.Add( p.PCView,  0,  7,  0,  4 );
                             break;
                         case PageComponentType.Footer
                             grid.Children.Add( p.PCView,  2,  5, 11, 12 );
                             break;
                         case PageComponentType.Option:
                             if(OptionCount < 4) {
                                 grid.Children.Add( p.PCView,  2,  5,  5+CurrentOption*2,  7+CurrentOption*2 );
                             } else {
                                 if(CurrentOption > 3) {
                                     grid.Children.Add( p.PCView,  2,  5,  9,  11 );
                                 } else {
                                     grid.Children.Add( p.PCView,
                                                        0 + (currentOption % 2) * 4,
                                                        3 + (currentOption % 2) * 4,
                                                        5 + int(CurrentOption / 2) * 2,
                                                        7 + int(CurrentOption / 2) * 2 );
                                 }
                             }
                             ++CurrentOption;
                             break;
                         }
                     }
                 } else {
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
                         case PageComponentType.Footer
                             grid.Children.Add( p.PCView,  2,  5,  6, 7 );
                             break;
                         case PageComponentType.Option:
                             if(OptionCount < 4) {
                                 grid.Children.Add( p.PCView,  10,  13,  1+CurrentOption*2,  1+CurrentOption*2 );
                             } else {
                                 if(CurrentOption > 3) {
                                     grid.Children.Add( p.PCView,  5,  7,  10,  13 );
                                 } else {
                                     grid.Children.Add( p.PCView,
                                                        8  + (currentOption % 2) * 4,
                                                        11 + (currentOption % 2) * 4,
                                                        1  + int(CurrentOption / 2) * 2,
                                                        3  + int(CurrentOption / 2) * 2 );
                                 }
                             }
                             ++CurrentOption;
                             break;
                     } 
                 }  // End Wide
             }  // End PageComponentLoop
                     //grid.Children.Add(_displays["hairB"], 1 * col, 1 * row+1);
                 //grid.Children.Add(_displays["hairR"], 1 * col, 1 * row);

            return Container;
        }
    }    
}
