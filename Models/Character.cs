using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{

    public class Character
    {
        public string url { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string culture { get; set; }
        public string born { get; set; }
        public string died { get; set; }
        public string[] titles { get; set; }
        public string[] aliases { get; set; }
        public Character father { get; set; }
        public Character mother { get; set; }
        public Character spouse { get; set; }
        public House[] allegiances { get; set; }
        public Book[] books { get; set; }
        public Book[] povBooks { get; set; }
        public string[] tvSeries { get; set; }
        public string[] playedBy { get; set; }
    }

}
