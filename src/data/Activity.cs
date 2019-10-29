using System;
using System.Collections.Generic;

namespace Florine
{
    public class Activity : IGameOption
    {		
        public NutrientSet Impact { get; set;}
        public bool Enabled { get; set; } = true;
        public String OptionName { get; set; }
		public String Description { get; set; }
		public IImage Picture { get; set; }
        public int Pay { get; set; } = 0;
		public IGameOptionSet SubOptions { get; set; }
		public virtual void ImpactPlayer(Player target) 
		{
			if(null != Impact) {
			    AdjustNutrients(target.Nutrients);
			}
		}
		public virtual void AdjustNutrients(NutrientSet target) {
			// Probably should change NutrientSet type to inherit directly or implement IEnumX
			foreach( KeyValuePair<Nutrient, NutrientAmount> kvp in Impact ) {				
				NutrientAmount val = 0;
				target.TryGetValue(kvp.Key, out val);
				target[kvp.Key] = val + kvp.Value;
			}
		}
		
    }
	
	public interface IActivityPath {
	    Activity GetActivity(Player p, GameState gs);	
	}
	
	public class BasicActivityPath {
		protected Dictionary<GameState.PageSubType, IActivityPath> SubPaths =
			new Dictionary<GameState.PageSubType, IActivityPath>();
		
		public virtual Activity GetActivity(GameState gs)
		{
			IActivityPath nextPath;
			if(SubPaths.TryGetValue(gs.CurrentPage.SubType, out nextPath)) {
				// Basic sanity check
				if (nextPath != this) {
					return nextPath.GetActivity(gs.Player, gs);
				}
			}
			return ResolveActivityForGameState(gs);
		}		
		
		public virtual Activity ResolveActivityForGameState(GameState gs)
		{
			return null;
		}
	}
}
