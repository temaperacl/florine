using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Florine;
using SkiaSharp;
using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public abstract class OptionView
    {
        public static OptionView FromOptionSet(IGameOptionSet Options) {
        }

        public static OptionView FromOption(IGameOption Option) {
        }

        virtual Xamarin.Forms.View AsView { get; set; }

        /* Roots */
        private class _ToggleOptionView : OptionView {
            private IGameOption _option;
            public _ToggleOptionView(opt) {
                _option = opt;
            };

        }
        private class _ChoiceOptionView : OptionView {
            private IGameOption _option;
            public _ChoiceOptionView(opt) {
                _option = opt;
            };
        }

        private class _OptionSetView : OptionView {
            private IGameOptionSet _options;
            public _OptionSetView(opt) {
                _options = opt;
            };
        
            public void Layout() {
                Grid grid = MakeGrid(
                    this.CanvasSize,
                    new int[] { 1, 1 },
                    new int[] { 1, 1, 1 }
                );

                // Place Options
            }
        }

        /* Util_f */    
        private void MakeGrid(SKSize Dim, int[] Horizontal, int[] Vertical)
        {
            var grid = new Grid();
            Grid subgrid = new Grid();

            int row = 0;
            int col = 0;
            float Height = Dim.Height;
            float Width = Dim.Width;

            int[] Rows = Vertical;
            int[] Cols = Horizontal;

            if (Height < Width)
            {
                // Landscape
                Rows = Horizontal;
                Cols = Vertical;
            }
            foreach(int weight in Rows) {                
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Weight, GridUnitType.Star) });
            }

            foreach(int weight in Cols) {                
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(weight, GridUnitType.Star) });
            }
            //grid.Children.Add(_displays["hairB"], 1 * col, 1 * row+1);
            //grid.Children.Add(_displays["hairR"], 1 * col, 1 * row);
            return grid;
        }
    }
}
