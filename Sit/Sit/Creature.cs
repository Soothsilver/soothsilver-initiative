using Acr.UserDialogs;
using System.Windows.Input;
using Xamarin.Forms;
using PostSharp.Patterns.Model;
using System.ComponentModel;
using PostSharp.Patterns.Recording;

namespace Sit
{
    [NotifyPropertyChanged]
    [Recordable]
    public class Creature : INotifyPropertyChanged
    {
        public string Name { get;
            [ThenSave]
            set; }
        public bool Friendly { get; set; }
        public int Initiative { get; set; }

        [NotRecorded]
        public int TargetInitiative { get; set; }

        [SafeForDependencyAnalysis]
        public bool Active
        {
            get
            {
                return MainPage.Instance.Encounter.ActiveCreature == this;
            }
        }


        [NotRecorded, Reference]
        public ICommand UpCommand { set; get; }
        [NotRecorded, Reference]
        public ICommand DownCommand { set; get; }
        [NotRecorded, Reference]
        public ICommand KillCommand { set; get; }
        [NotRecorded, Reference]
        public ICommand TapNameCommand { get; set; }

        public Creature()
        {
            KillCommand = new Command(() =>
            {
                MainPage.Instance.Encounter.Kill(this);
            });
            UpCommand = new Command(() =>
            {
                MainPage.Instance.Encounter.Up(this);
            });
            DownCommand = new Command(() =>
            {
                MainPage.Instance.Encounter.Down(this);
            });
            TapNameCommand = new Command(async () =>
            {

                var result = await UserDialogs.Instance.PromptAsync("New name: ", "Enter new name for the creature:", "Rename", "Cancel", this.Name);
                if (result.Ok)
                {
                    this.Name = result.Text;
                }

            });
        }
        public Creature(string name, bool friendly) : this()
        {
            Name = name;
            Friendly = friendly;
        }
        [SafeForDependencyAnalysis]
        public Xamarin.Forms.Color BackgroundColor
        {
            get
            {
                if (Friendly)
                {
                    return Color.PaleGreen;
                }
                else
                {
                    return Color.LightSalmon;
                }
            }
        }

        [SafeForDependencyAnalysis]
        public Xamarin.Forms.Color ActiveBackground
        {
            get
            {
                if (Active)
                {
                    return Color.Yellow;
                }
                else
                {
                    return Color.Transparent;
                }
            }
        }

        public string AsString
        {
            get
            {
                return Name + " (" + Initiative + ", " + (Friendly ? "Hero" : "Monster") + ")";
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}