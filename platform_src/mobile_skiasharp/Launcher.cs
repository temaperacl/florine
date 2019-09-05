using System;
using Florine;

namespace FlorineSkiasSharpForms
{
    public class Launcher
    {
        private SkiaSharpFormsFoundry _foundry;
        private Florine.Controller _controller;        
        public Launcher(System.Web.UI.Page page)
        {            
            _foundry = new FlorineSkiaSharpForms.SkiaSharpFormsFoundry(page);
            _controller = new Florine.Controller(_foundry);
            IGameOption opt = _foundry.GetChosenOption();
            if (null != opt)
            {
                _controller.UserOption(opt);
            }
        }

        // Page Control

    }
}
