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

    public class AvatarSelectionLayout: PageLayout
    {
        public override SKRectI GetResolution(Dictionary<PageComponentType, int> ComponentCounts, bool IsTall)
        {
            return base.GetResolution(ComponentCounts, IsTall);
        }

        PlayerAvatar ActualPlayer;
        SKCanvasView PlayerView;
        Dictionary<SKCanvasView, AspectImage> _rmI = new Dictionary<SKCanvasView, AspectImage>();
        private void SetClothesImage(object sender, FlorineSkiaTapWrap.TapEventArgs e)
        {
            FlorineSkiaTapWrap fstw = sender as FlorineSkiaTapWrap;            
            if (
                null != fstw
                && null != PlayerView
                && null != ActualPlayer
                )
            {
                SKCanvasView scv = fstw.Tie;
                AspectImage ai;
                if (_rmI.TryGetValue(scv, out ai) )
                {
                    ActualPlayer.Clothes = ai.baseImage;
                    PlayerView.InvalidateSurface();
                }
            }
            //_MainCanvas.IsVisible = false;
        }

        public override void PostLayout(bool IsTall, Grid grid, Controller GameController, IPlatformFoundry GameFoundry, IPage SourcePage)
        {
            ActualPlayer = GameController.CurrentState.Player.Avatar.Picture as PlayerAvatar;
            Dictionary<string, SKImage> clothes = ResourceLoader.ImageList("customization/03_clothes/Box/50%");
            int i = 0;
            foreach (KeyValuePair<string, SKImage> kvp in clothes)
            {
                SKCanvasView canvas = new SKCanvasView();
                AspectImage ai = new AspectImage() { baseImage = kvp.Value };                                        
                ai.ConnectCanvasView(canvas);
                _rmI[canvas] = ai;

                FlorineSkiaTapWrap.Associate(canvas, SetClothesImage);


                int X = i % 8;
                int Y = i / 8 + 6;
                grid.Children.Add(canvas, X,X+1,Y,Y+1);
                i++;
            }
            base.PostLayout(IsTall, grid, GameController, GameFoundry, SourcePage);
        }

        protected override void LayoutComponentWide(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            base.LayoutComponentWide(grid, t, v, CurrentOption, OptionCount);
        }

        
        protected override void LayoutComponentTall(Grid grid, PageComponentType t, View v, int CurrentOption, int OptionCount)
        {
            switch (t)
            {
                case PageComponentType.Message:
                    grid.Children.Add(v, 0, 8, 1, 10);
                    break;
                case PageComponentType.Player:
                    PlayerView = v as SKCanvasView;
                    grid.Children.Add(v, 0, 8, 1, 5);
                    break;
                default:
                    base.LayoutComponentTall(grid, t, v, CurrentOption, OptionCount);
                    break;
            }
        }
    }    
}
