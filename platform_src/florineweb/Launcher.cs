using System;
using Florine;

namespace FlorineWeb
{
    public class Launcher
    {
        private WebFoundry _foundry;
        private Florine.Controller _controller;
        public Launcher()
        {
            _foundry = new FlorineWeb.WebFoundry();
            _controller = new Florine.Controller(_foundry);
        }



    }
}
