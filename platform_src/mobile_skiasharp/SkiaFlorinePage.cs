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
        public SkiaFlorinePage(Controller GameController, SkiaSharpFormsFoundry Foundry)
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                }
            };
        }
    }
}