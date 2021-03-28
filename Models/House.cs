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
    /// House Model class. Can be used for databinding, because of the ObservableCollections and INPC interface.
    /// Stores its references to other Model entities using C# references(pointers).
    /// </summary>
    public class House : BaseModel
    {
        //public string url { get; set; }
        private string _name;
        public string name { get { return _name; } set { SetProperty(ref _name, value); } }
        private string _region;
        public string region { get { return _region; } set { SetProperty(ref _region, value); } }
        private string _coatOfArms;
        public string coatOfArms { get { return _coatOfArms; } set { SetProperty(ref _coatOfArms, value); } }
        private string _words;
        public string words { get { return _words; } set { SetProperty(ref _words, value); } }
        public ObservableCollection<string> titles { get; set; }
        public ObservableCollection<string> seats { get; set; }
        private Character _currentLord;
        public Character currentLord { get { return _currentLord; } set { SetProperty(ref _currentLord, value); } }
        private Character _heir;
        public Character heir { get { return _heir; } set { SetProperty(ref _heir, value); } }
        private House _overlord;
        public House overlord { get { return _overlord; } set { SetProperty(ref _overlord, value); } }
        private string _founded;
        public string founded { get { return _founded; } set { SetProperty(ref _founded, value); } }
        private Character _founder;
        public Character founder { get { return _founder; } set { SetProperty(ref _founder, value); } }
        private string _diedOut;
        public string diedOut { get { return _diedOut; } set { SetProperty(ref _diedOut, value); } }
        public ObservableCollection<string> ancestralWeapons { get; set; }
        public ObservableCollection<House> cadetBranches { get; set; }
        public ObservableCollection<Character> swornMembers { get; set; }

        
    }

}
