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
            _controller.Init();

            IGameOption opt = _foundry.GetChosenOption(_controller);
            if (null != opt)
            {
                _controller.UserOption(opt);
            }
        }
		public System.Web.UI.Control PageInit(bool debugInfo)
        {
            System.Web.UI.Control page = _foundry.RenderPage(
				_controller.CurrentState,
				_controller.GetCurrentPage(),
				debugInfo);            
            return page;
        }
		
		
        public void PageRequest(System.Web.UI.ControlCollection Controls)
        {
            _foundry.UpdatePage(_controller.GetCurrentPage());
            _foundry.FinalizePage(_controller.CurrentState);
        }

    }
}
