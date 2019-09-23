using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace FlorineSkiaSharpForms
{
    class Florine_SkiaPage : IPage
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
}
