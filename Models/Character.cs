using System;
using System.Collections.Generic;
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
        public string gender { get; set; }
        public string culture { get; set; }
        public string born { get; set; }
        public string died { get; set; }
        public string[] titles { get; set; }
        public string[] aliases { get; set; }
        public Character father { get; set; }
        public Character mother { get; set; }
        public Character spouse { get; set; }
        public List<House> allegiances { get; set; }
        public List<Book> books { get; set; }
        public List<Book> povBooks { get; set; }
        public string[] tvSeries { get; set; }
        public string[] playedBy { get; set; }

    }

}
