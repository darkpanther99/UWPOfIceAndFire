using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{

    public class Character : INotifyPropertyChanged
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
        private string _gender;
        public string gender { get { return _gender; } 
            set 
            {
                if (_gender != value)
                {
                    _gender = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(gender)));
                }
            } 
        }
        private string _culture;
        public string culture { get { return _culture; } 
            set {
                if (_culture != value)
                {
                    _culture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(culture)));
                }
            }
        }
        private string _born;
        public string born { get { return _born; }
            set
            {
                if (_born != value)
                {
                    _born = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(born)));
                }
            }
        }
        private string _died;
        public string died { get {return _died;} 
            set 
            {
                if (_died != value)
                {
                    _died = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(died)));
                }
            } 
        }
        public string[] titles { get; set; }
        public string[] aliases { get; set; }
        private Character _father;
        public Character father { get { return _father; }
            set
            {
                if (_father != value)
                {
                    _father = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(father)));
                }
            } 
        }
        private Character _mother;
        public Character mother { get { return _mother; }
            set 
            {
                if (_mother != value)
                {
                    _mother = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(mother)));
                }
            } 
        }
        private Character _spouse;
        public Character spouse { get { return _spouse; }
            set
            {
                if (_spouse != value)
                {
                    _spouse = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(spouse)));
                }
            }
        }
        public List<House> allegiances { get; set; }
        public List<Book> books { get; set; }
        public List<Book> povBooks { get; set; }
        public string[] tvSeries { get; set; }
        public string[] playedBy { get; set; }

    }

}
