using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{

    public class Book : BaseModel
    {
        public string url { get; set; }
        private string _name;
        public string name { get { return _name;  } set { SetProperty(ref _name, value); } }
        private string _isbn;
        public string isbn { get { return _isbn; } set { SetProperty(ref _isbn, value); } }
        public ObservableCollection<string> authors { get; set; }
        private int _numberOfPages;
        public int numberOfPages { get { return _numberOfPages; } set { SetProperty(ref _numberOfPages, value); } }
        private string _publisher;
        public string publisher { get { return _publisher; } set { SetProperty(ref _publisher, value); } }
        private string _country;
        public string country { get { return _country; } set { SetProperty(ref _country, value); } }
        private string _mediaType;
        public string mediaType { get { return _mediaType; } set { SetProperty(ref _mediaType, value); } }
        private DateTime _released;
        public DateTime released { get { return _released; } set { SetProperty(ref _released, value); } }
        public ObservableCollection<Character> characters { get; set; }
        public ObservableCollection<Character> povCharacters { get; set; }

    }



}
