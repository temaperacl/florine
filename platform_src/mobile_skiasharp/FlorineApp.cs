using System;
using Florine;
using Xamarin.Forms;

namespace FlorineSkiaSharpForms
{
    public class FlorineApp : Application
    {
        private SkiaSharpFormsFoundry _foundry;
        private Florine.Controller _controller;
        private ISkiaSharpFlorineDataSource _data;
        public FlorineApp(ISkiaSharpFlorineDataSource dataHolder) : base()
        {
            _data = dataHolder;
            ResourceLoader.DataSource = _data;
            _foundry = new FlorineSkiaSharpForms.SkiaSharpFormsFoundry();
            _controller = new Florine.Controller(_foundry);
            _controller.Init();
            MainPage = new SkiaFlorinePage(_controller, _foundry);
            
        }
        

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        // Page Control

    }
}
