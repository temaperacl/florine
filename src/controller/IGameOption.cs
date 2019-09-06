using System;
using System.Collections.Generic;

namespace Florine
{
    // Primary game bottleneck/controller
    public interface IGameOption
    {
        String OptionName { get; }
        IImage Picture { get; }
        void AdjustNutrients(NutrientSet target);
    }

    public interface IGameOptionSet : IList<IGameOption>
    {
        int SelectionLimit { get; }
        IGameOption Finalizer { get; }
    }
}
