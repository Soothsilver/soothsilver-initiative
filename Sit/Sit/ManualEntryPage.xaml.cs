using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sit
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualEntryPage : ContentPage
	{
        private readonly Encounter encounter;

        public ManualEntryPage (Encounter encounter)
		{
            var creatures = encounter.Creatures;
			InitializeComponent ();
            foreach(var cr in creatures)
            {
                cr.TargetInitiative = int.MinValue;
            }
            this.ManualEntryList.ItemsSource = creatures;
            this.encounter = encounter;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            encounter.AfterManualInitiatives();
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}