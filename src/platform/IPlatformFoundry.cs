using System;

namespace Florine
{
    public interface IPlatformFoundry
    {
        GameState LoadGameState();
        IPage GetPage(IPage GenericPage);
    }
}
