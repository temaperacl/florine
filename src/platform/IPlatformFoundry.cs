using System;

namespace Florine
{
    public interface IPlatformFoundry
    {
        GameState LoadGameState();
        IPage GetPage(GameState.PageType mainType, GameState.PageSubType subType, GameState context);
    }
}