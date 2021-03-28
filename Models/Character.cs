using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{
    /// <summary>
    /// Character Model class. Can be used for databinding, because of the ObservableCollections and INPC interface.
    /// Stores its references to other Model entities using C# references(pointers).
    /// </summary>
    public class Character : BaseModel
    {
        private string _name;
        public string name { get { return _name; } set { SetProperty(ref _name, value); } }
        private string _gender;
        public string gender { get { return _gender; } set { SetProperty(ref _gender, value); } }
        private string _culture;
        public string culture { get { return _culture; } set { SetProperty(ref _culture, value); } }
        private string _born;
        public string born { get { return _born; } set { SetProperty(ref _born, value); } }
        private string _died;
        public string died { get { return _died; } set { SetProperty(ref _died, value); } }
        public ObservableCollection<string> titles { get; set; }
        public ObservableCollection<string> aliases { get; set; }
        private Character _father;
        public Character father { get { return _father; } set { SetProperty(ref _father, value); } }
        private Character _mother;
        public Character mother { get { return _mother; } set { SetProperty(ref _mother, value); } }
        private Character _spouse;
        public Character spouse { get { return _spouse; } set { SetProperty(ref _spouse, value); } }
        public ObservableCollection<House> allegiances { get; set; }
        public ObservableCollection<Book> books { get; set; }
        public ObservableCollection<Book> povBooks { get; set; }
        public ObservableCollection<string> tvSeries { get; set; }
        public ObservableCollection<string> playedBy { get; set; }

    }

}
