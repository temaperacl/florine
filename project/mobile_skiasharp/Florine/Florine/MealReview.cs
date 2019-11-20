using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Florine
{
    public class MealReview : ContentPage
    {
        List<Food> Selected = new List<Food>();
        public MealReview(List<Food> foodList)
        {
            Selected = foodList;

            Content = new StackLayout
            {
                Children = {
                    new Label { Text = string.Join(", ", Selected) }
                }
            };
        }
    }
}