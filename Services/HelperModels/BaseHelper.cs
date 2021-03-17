using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Services.HelperModels
{
    /// <summary>
    /// Base of the Helper classes. This class is used for databinding the preview object's name to the Main page, thus it implements the INPC interface.
    /// </summary>
    class BaseHelper : INotifyPropertyChanged
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
    }
}
