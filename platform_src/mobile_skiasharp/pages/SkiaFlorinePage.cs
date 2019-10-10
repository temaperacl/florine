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
            FlorineSkiaOptionSet OptionSet = PrimaryOptions as FlorineSkiaOptionSet;
            if (null != OptionSet)
            {
                SelectedOptions = OptionSet.Selected;
            }
            
            
            IPage Next = _controller.UserOption(SelectedOptions);
            Content = _foundry.RenderPage(_controller.CurrentState);
            
        }

        private IPage SourcePage;
        private IGameOptionSet PrimaryOptions;
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
             _readyIPage(Source, State);
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
        private void _readyOptionSet(
            PageComponentType pcType,
            IGameOptionSet opts,
            EventHandler PressFunc = null
        ) {
            if (null == opts) { return; }
            foreach (IGameOption opt in opts)
            {
                _readyOption(pcType, opt, PressFunc);
            }
        }

        private void _readyOption(
            PageComponentType pcType,
            IGameOption opt,
            EventHandler PressFunc = null
        ) {
            if (null == opt) { return; }
            if (null != opt.SubOptions) {
                _readyOptionSet(
                    pcType,
                    opt.SubOptions,
                    PressFunc
                    );
                return;
            }
            SKCanvasView Img = new SKCanvasView();
            IFlorineSkiaConnectable conn = opt as IFlorineSkiaConnectable;

            if (null != conn)
            {
                conn.ConnectCanvasView(Img);
            }

            IFlorineSkiaEventDriver econn = opt as IFlorineSkiaEventDriver;
            if (null != econn)
            {
                if (null != PressFunc)
                {
                    econn.OnEventTriggered += PressFunc;
                }
            }

            _components.Add(
                new PageComponent(
                    _inc(pcType),
                    Img
                )
            );
        }
        private void _readyIPage(IPage Source, GameState gameState)
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
                         _inc(PageComponentType.Background),
                         Header
                     )
                 );
             }
            if (null != Source.Title)
            {
                SKCanvasView Message = new SKCanvasView();
                ImageText itconn = new ImageText(Source.Title);
                itconn.ConnectCanvasView(Message);
                _components.Add(
                    new PageComponent(
                       _inc(PageComponentType.Title),
                       Message
                     )
                );
            }
            if (null != Source.Message && "" != Source.Message) 
             {
                SKCanvasView Message = new SKCanvasView();
                ImageText itconn = new ImageText(Source.Message) { Overflow = ImageText.WrapType.WordWrap, FontSize = 48.0f };
                itconn.ConnectCanvasView(Message);
                _components.Add(
                    new PageComponent(
                       _inc(PageComponentType.Message),
                       Message
                     )
                 );                
            }
            _readyOptionSet(
                PageComponentType.PickedOption,
                Source.AppliedOptions
            );

            PrimaryOptions = Source.PrimaryOptions;
            _readyOptionSet(
                PageComponentType.Option,
                PrimaryOptions
            );
            
            if (null != Source.PrimaryOptions)
            {
                _readyOption(
                    PageComponentType.Footer,
                    Source.PrimaryOptions.Finalizer,
                    Continue_Handler
                    );
                FlorineSkiaOptionSet FSO = PrimaryOptions as FlorineSkiaOptionSet;
                if (FSO != null) {
                    SKCanvasView Desc = new SKCanvasView();
                    FSO.UpdaterHook.ConnectCanvasView(Desc);
                    _components.Add(
                        new PageComponent(
                            _inc(PageComponentType.Description),
                            Desc
                        )
                    );
                }                
            }
            SourcePage = Source;

            if (null != gameState.Player)
            {
                SKCanvasView avatar = new SKCanvasView();
                IFlorineSkiaConnectable itconn = gameState.Player.Avatar.Picture as IFlorineSkiaConnectable;
                itconn.ConnectCanvasView(avatar);
                _components.Add(
                    new PageComponent(
                       _inc(PageComponentType.Player),
                       avatar
                     )
                );
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
                    break;
                case GameState.PageType.Summarize_Activity:
                    ActiveLayout = new ActivitySummaryLayout();
                        break;
                case GameState.PageType.Char_Creation:
                    ActiveLayout = new AvatarSelectionLayout();
                    break;
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
            
            ActiveLayout.PreLayout(IsTall, grid, _controller, _foundry, SourcePage);
            // ********************************************** * Layout
            LayoutComponents(
                 ActiveLayout,
                 IsTall,
                 _componentCounts,
                 grid
            );
            ActiveLayout.PostLayout(IsTall, grid, _controller, _foundry, SourcePage);

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
