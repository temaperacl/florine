using System;
namespace Florine
{
    public interface IPage
    {
        GameState.PageType MainType { get; }
        GameState.PageSubType SubType { get; }

        IImage Background { get; }
		// Options To Apply
        IGameOptionSet PrimaryOptions { get; }
		// Options that were applied
		IGameOptionSet AppliedOptions { get; }		
        String Title { get; }
        String Message { get; }
        NutrientSet NutrientState { get; }
        NutrientSet NutrientDelta { get; }

    }
/*    public interface IPagePrototype
    {
        GameState.PageType MainType { get; }
        GameState.PageSubType SubType { get; }

        IImage Background { get; }
        IGameOptionSet PrimaryOptions { get; }
        String Title { get; }
        String Message { get; }        
    }
    */
}
