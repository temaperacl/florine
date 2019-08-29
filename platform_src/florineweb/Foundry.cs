using System;
using Florine;

namespace FlorineWeb
{
    public class WebFoundry : IPlatformFoundry
    {
        public GameState LoadGameState()
        {
            return new GameState();
        }

        public IPage GetPage(IPage GenericPage)
        {
            return GenericPage;
        }
    }
}
