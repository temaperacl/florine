using System;
namespace Florine
{
    public interface IPage
    {
        GameState.PageType MainType { get; }
        GameState.PageSubType SubType { get; }

        IImage Background { get; }
        IGameOptionSet PrimaryOptions { get; }
        String Title { get; }
        String Message { get; }
        NutrientSet NutrientState { get; }
        NutrientSet NutrientDelta { get; }

    }
}
