using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{

    public class House : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string url { get; set; }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(name)));
                }
            }
        }
        private string _region;
        public string region { get {return _region; }
            set
            {
                if (_region != value)
                {
                    _region = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(region)));
                }
            }
        }
        private string _coatOfArms;
        public string coatOfArms { get { return _coatOfArms; }
            set 
            {
                if (_coatOfArms != value)
                {
                    _coatOfArms = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(coatOfArms)));
                }
            } 
        }
        private string _words;
        public string words { get { return _words; }
            set 
            {
                if (_words != value)
                {
                    _words = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(words)));
                }
            }
        }
        public string[] titles { get; set; }
        public string[] seats { get; set; }
        private Character _currentLord;
        public Character currentLord { get {return _currentLord; }
            set
            {
                if (_currentLord != value)
                {
                    _currentLord = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentLord)));
                }
            } 
        }
        private Character _heir;
        public Character heir
        {
            get { return _heir; }
            set
            {
                if (_heir != value)
                {
                    _heir = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(heir)));
                }
            }
        }
        private House _overlord;
        public House overlord { get { return _overlord; } 
            set 
            {
                if (_overlord != value)
                {
                    _overlord = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(overlord)));
                }
            } 
        }
        private string _founded;
        public string founded { get { return _founded; }
            set 
            {
                if (_founded != value)
                {
                    _founded = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(founded)));
                }
            } 
        }
        private Character _founder;
        public Character founder
        {
            get { return _founder; }
            set
            {
                if (_founder != value)
                {
                    _founder = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(founder)));
                }
            }
        }
        private string _diedOut;
        public string diedOut
        {
            get { return _diedOut; }
            set
            {
                if (_diedOut != value)
                {
                    _diedOut = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(diedOut)));
                }
            }
        }
        public string[] ancestralWeapons { get; set; }
        public List<House> cadetBranches { get; set; }
        public List<Character> swornMembers { get; set; }

        
    }

}
