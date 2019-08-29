using System;
namespace Florine
{
    // Primary game bottleneck/controller
    public interface IGameOption
    {
        String OptionName { get; }
        void AdjustNutrients(NutrientSet target);
    }

    public interface IGameOptionSet
    {        
    }
}
