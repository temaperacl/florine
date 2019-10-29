using System;
using System.Collections.Generic;

namespace Florine
{
    // Primary game bottleneck/controller
    public interface IGameOption
    {
        String OptionName { get; }
        IImage Picture { get; }
		void ImpactPlayer(Player p);
		void AdjustNutrients(NutrientSet n);
		IGameOptionSet SubOptions { get; }
        bool Enabled { get; }
    }

    public interface IGameOptionSet : IList<IGameOption>
    {
        int SelectionLimit { get; }
        IGameOption Finalizer { get; }
    }
}
