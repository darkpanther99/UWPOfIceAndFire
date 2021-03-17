using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Services.HelperModels
{
    /// <summary>
    /// Character helper class. Stores the referenced books, characters, houses using URI strings.
    /// </summary>
    class CharacterHelper : BaseHelper
    {
        public string gender { get; set; }
        public string culture { get; set; }
        public string born { get; set; }
        public string died { get; set; }
        public string[] titles { get; set; }
        public string[] aliases { get; set; }
        public string father { get; set; }
        public string mother { get; set; }
        public string spouse { get; set; }
        public string[] allegiances { get; set; }
        public string[] books { get; set; }
        public string[] povBooks { get; set; }
        public string[] tvSeries { get; set; }
        public string[] playedBy { get; set; }
    }
}
