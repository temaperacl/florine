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

        public IPage GetPage(GameState.PageType mainType, GameState.PageSubType subType, GameState context)
        {
            return null;
        }
    }
}