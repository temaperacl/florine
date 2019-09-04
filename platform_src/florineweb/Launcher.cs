using System;
using Florine;

namespace FlorineWeb
{
    public class Launcher
    {
        private WebFoundry _foundry;
        private Florine.Controller _controller;        
        public Launcher(System.Web.UI.Page page)
        {            
            _foundry = new FlorineWeb.WebFoundry(page);
            _controller = new Florine.Controller(_foundry);
            IGameOption opt = _foundry.GetChosenOption();
            if (null != opt)
            {
                _controller.UserOption(opt);
            }
        }

        public System.Web.UI.Control PageRequest(bool debugInfo)
        {                        
            System.Web.UI.Control page = _foundry.RenderPage(_controller.GetCurrentPage(), debugInfo);
            if (!debugInfo)
            {
                _foundry.FinalizePage(_controller.CurrentState);
            }
            return page;
        }

    }
}
