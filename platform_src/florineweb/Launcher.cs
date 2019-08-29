using System;
using Florine;

namespace FlorineWeb
{
    public class Launcher
    {
        private Florine.Controller _controller;
        public Launcher()
        {
            _controller = new Florine.Controller(new FlorineWeb.WebFoundry());
        }

    }
}