using Acr.UserDialogs;
using Plugin.Settings;
using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sit
{
    public partial class MainPage : ContentPage
    {

        public Encounter Encounter = new Encounter();

        public static MainPage Instance { get; private set; }


        public MainPage()
        {
            string savedEncounter = CrossSettings.Current.GetValueOrDefault("Encounter", "", "thensave.txt"); // must be before InitializeComponent or it will be overwritten
            Instance = this;
            InitializeComponent();
            this.BindingContext = this;
            Encounter.Deserialize(savedEncounter);
            this.MasterList.ItemsSource = Encounter.Creatures;
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }


        private void AddHero_Clicked(object sender, EventArgs e)
        {
            Encounter.AddCreature(new Creature("Hero #" + (Encounter.Creatures.Count(cr => cr.Friendly) + 1), true));
        }


        private void Undo_Clicked(object sender, EventArgs e)
        {
            if (RecordingServices.DefaultRecorder.CanUndo)
            {

                UserDialogs.Instance.Toast(new ToastConfig("Undid " + RecordingServices.DefaultRecorder.UndoOperations.Last().Name)
                {
                    Position = ToastPosition.Top
                });
                RecordingServices.DefaultRecorder.Undo();
            }
        }

        private void Redo_Clicked(object sender, EventArgs e)
        {
            if (RecordingServices.DefaultRecorder.CanRedo)
            {
                RecordingServices.DefaultRecorder.Redo();
            }
        }

        private async void EnterInitiatives_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ManualEntryPage(this.Encounter));
        }

        private void AddMonster_Clicked(object sender, EventArgs e)
        {
            Encounter.AddCreature(new Creature("Monster #" + (Encounter.Creatures.Count(cr => !cr.Friendly) + 1), false));
        }

        

        private void Right_Clicked(object sender, EventArgs e)
        {
            Encounter.MoveInitiative(1);
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            Encounter.MoveInitiative(-1);
        }

       
    }
}
