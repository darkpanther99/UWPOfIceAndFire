using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{

    public class House
    {
        public string url { get; set; }
        public string name { get; set; }
        public string region { get; set; }
        public string coatOfArms { get; set; }
        public string words { get; set; }
        public string[] titles { get; set; }
        public string[] seats { get; set; }
        public Character currentLord { get; set; }
        public Character heir { get; set; }
        public House overlord { get; set; }
        public string founded { get; set; }
        public Character founder { get; set; }
        public string diedOut { get; set; }
        public string[] ancestralWeapons { get; set; }
        public House[] cadetBranches { get; set; }
        public Character[] swornMembers { get; set; }
    }

}
