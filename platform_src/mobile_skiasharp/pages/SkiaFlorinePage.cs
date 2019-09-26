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
    public class SkiaFlorinePage : ContentPage
    {
        private SkiaSharpFormsFoundry _foundry;
        private Controller _controller;

        public SkiaFlorinePage(Controller GameController, SkiaSharpFormsFoundry Foundry) : base()
        {
            _controller = GameController;
            _foundry = Foundry;
            Foundry.Page = this;
            Foundry.GameController = GameController;
            Content = Foundry.RenderPage(GameController.CurrentState);
        }

        private void Continue_Handler(object sender, EventArgs e)
        {
            IGameOption SelectedOptions = null;
            FlorineSkiaOptionSet OptionSet = PrimaryOptionContainer as FlorineSkiaOptionSet;
            if(null != OptionSet) { SelectedOptions = OptionSet.Selected; }
            
            IPage Next = _controller.UserOption(SelectedOptions);
            Content = _foundry.RenderPage(_controller.CurrentState);
            
        }

        private IGameOptionSet PrimaryOptionContainer;
        private List<PageComponent> _components = new List<PageComponent>();
        private Dictionary<PageComponentType, int> _componentCounts = new Dictionary<PageComponentType, int>();

        public View UndefinedLayout(
            IPage Source,
            GameState State)
        {
            Button ContinueButton = new Button()
            {
                Text = "Continue",
            };
            ContinueButton.Clicked += Continue_Handler;

            return new StackLayout()
            {
                Children = {
                    new Label() {
                        Text = Source.MainType.ToString()
                        + "("
                        + Source.SubType.ToString()
                        + ")"
                    },
                    ContinueButton
                }
            };
        }

        public View DefaultLayout(
                                  IPage Source,
                                  GameState State
                                  )
        {
	     var grid = new Grid();
             grid.SizeChanged += SizeChanged_Grid;
             _components.Clear();
             _componentCounts.Clear();
             _readyIPage(Source);
            return grid;
        }

        private PageComponentType _inc(PageComponentType ptype)
        {
            if(_componentCounts.ContainsKey(ptype)) {
                _componentCounts[ptype]++;
            } else {
                _componentCounts[ptype] = 1;
            }
            return ptype;
        }

        private void _readyIPage(IPage Source)
        {

             if(null != Source.Background) {
                SKCanvasView Header = new SKCanvasView();
                IFlorineSkiaConnectable iconn = Source.Background as IFlorineSkiaConnectable;
                if (null != iconn)
                {
                    iconn.ConnectCanvasView(Header);
                }
                 _components.Add(
                     new PageComponent(
                         _inc(PageComponentType.Header),
                         Header
                     )
                 );
             }
             if(null != Source.Message && "" != Source.Message) 
             {
                SKCanvasView Message = new SKCanvasView();
                ImageText itconn = new ImageText(Source.Message);
                itconn.ConnectCanvasView(Message);
                _components.Add(
                    new PageComponent(
                       _inc(PageComponentType.Message),
                       Message
                     )
                 );                
            }

            PrimaryOptionContainer = Source.PrimaryOptions;
            IGameOptionSet Opts = Source.PrimaryOptions;
            if(null != Opts) {
                foreach(IGameOption opt in Opts) 
                {
                    SKCanvasView Img = new SKCanvasView();
                    IFlorineSkiaConnectable conn = opt as IFlorineSkiaConnectable;

                    if(null != conn) {
                        conn.ConnectCanvasView(Img);
                    }

                     _components.Add(
                         new PageComponent(
                             _inc(PageComponentType.Option),
                             Img
                         )
                     );                                    
                }
                if(null != Opts.Finalizer) {
                    SKCanvasView Img = new SKCanvasView();
                    IFlorineSkiaConnectable conn = Opts.Finalizer as IFlorineSkiaConnectable;
                    if (null != conn)
                    {
                        conn.ConnectCanvasView(Img);
                    }
                    IFlorineSkiaEventDriver econn = Opts.Finalizer as IFlorineSkiaEventDriver;
                    if(null != econn) 
                    {                        
                        econn.OnEventTriggered += Continue_Handler;
                    }
                    _components.Add(
                        new PageComponent(
                            _inc(PageComponentType.Footer),
                            Img
                        )
                    );
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
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();            
            grid.Padding = new Thickness(0.0, 0.0);

            PageLayout ActiveLayout = null;
            switch (_controller.CurrentState.CurrentPage.MainType)
            {
                case GameState.PageType.Select_Meal:
                    ActiveLayout = new LayoutOptionSelect();
                    break;
                case GameState.PageType.Summarize_Meal:
                    ActiveLayout = new MealResultLayout();
                default:
                    ActiveLayout = new LayoutOptionSelect();
                    break;
            }

            // ************************************************** Prep
            bool IsTall = Height > Width;
            // Todo: Move to layout class.
            // ************************************************** Get Dimensions
            SKRect Dimensions = ActiveLayout.GetResolution(
               _componentCounts,
               IsTall
            );
            int Rows = (int)(Dimensions.Height);
            int Cols = (int)(Dimensions.Width);

            // *********************************************** Setup Grid            
            double CellHeight = Height / Rows;
            double CellWidth = Width / Cols;

            for (int i = 0; i < Rows; ++i)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < Cols; ++i)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            bool IsTall = Height > Width;
            ActiveLayout.PreLayout(IsTall, grid, _controller, _foundry);
            // ********************************************** * Layout
            LayoutComponents(
                 ActiveLayout,
                 IsTall,
                 _componentCounts,
                 grid
            );
            ActiveLayout.PostLayout(IsTall, grid, _controller, _foundry);

            /*
            for (int i = 0; i < Cols; ++i)
            {
                for (int y = 0; y < Rows; ++y)
                {
                    grid.Children.Add(new Label() { Text = i.ToString() + ", " + y.ToString(), FontSize = 8 }, i, y);
                }
             }
             */
        }

        private void LayoutComponents(PageLayout ActiveLayout, bool isTall, Dictionary<PageComponentType,int> OptionCount, Grid grid)
        {
            Dictionary<PageComponentType, int> CurrentOptionIndex = new Dictionary<PageComponentType, int>();
             foreach(PageComponent p in _components) 
             {
                if (CurrentOptionIndex.ContainsKey(p.PCType))
                {
                    CurrentOptionIndex[p.PCType]++;
                }
                else
                {
                    CurrentOptionIndex[p.PCType] = 0;
                } 
                 ActiveLayout.LayoutComponent(
                    grid,
                    p,
                    CurrentOptionIndex[p.PCType],
                    OptionCount[p.PCType],
                    isTall
                 );
             }  // End PageComponentLoop
        }

    }
} 
