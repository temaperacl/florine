using System;
using System.Collections.Generic;

namespace Florine
{
    public interface IPlatformFoundry
    {
        IPage GetPage(IPage GenericPage);
        GameState LoadGameState();
        bool SaveGameState(GameState CurrentState);
        IList<Food> LoadFood();
        IList<Nutrient> LoadNutrients();
		Activity AutomaticActivity(GameState CurrentState);
		bool GetNextGameState(GameState CurrentState,
						  out GameState.PageType nextType,
						  out GameState.PageSubType nextSubType
						  );
    }
}
