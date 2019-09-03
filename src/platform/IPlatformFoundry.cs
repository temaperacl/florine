using System;

namespace Florine
{
    public interface IPlatformFoundry
    {
        IPage GetPage(IPage GenericPage);
        GameState LoadGameState();
        List<Food> LoadFood();
        List<Nutrient> LoadNutrients();
    }
}
