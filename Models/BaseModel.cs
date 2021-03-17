using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TXC54G_HF.Models
{
    /// <summary>
    /// Base Model class. Acts as a base class for all entities in the Models package.
    /// Implements the INPC interface for databinding, and defines a function, which can be used to simplify code of the PropertyChanged notifications.
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        public string url { get; set; }
    }
}
