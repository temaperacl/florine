using System;
namespace Florine
{
    public class Player
    {
        public Player() {
            Name = "Florine";
            Avatar = new Avatar();
            Nutrients = new NutrientSet();
        }
        public String Name { get; set; }
        public Avatar Avatar { get; set; }
        public NutrientSet Nutrients { get; set; }


        public void ApplyOption(IGameOption option)
        {
            option.AdjustNutrients(this.Nutrients);
        }
    }
}
