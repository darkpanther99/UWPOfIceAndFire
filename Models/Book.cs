using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{

    public class Book
    {
        public string url { get; set; }
        public string name { get; set; }
        public string isbn { get; set; }
        public string[] authors { get; set; }
        public int numberOfPages { get; set; }
        public string publisher { get; set; }
        public string country { get; set; }
        public string mediaType { get; set; }
        public DateTime released { get; set; }
        public Character[] characters { get; set; }
        public Character[] povCharacters { get; set; }

    }



}
