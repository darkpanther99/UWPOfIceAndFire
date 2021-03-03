using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace TXC54G_HF.ViewModels
{
    class SettingsPageViewModel
    {
        public ItemClass imageitem { get; set; } = new ItemClass() { Image = new BitmapImage(new Uri("ms-appx:///Assets/starklogo.png")) };

        public void RadioButtonSelectionChanged(string housename)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["favouritehouse"] = housename;
            imageitem.Image = new BitmapImage(new Uri($"ms-appx:///Assets/{housename}logo.png"));
        }

        public int GetCurrentlySetIndex()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string value = localSettings.Values["favouritehouse"] as string;
            int idx = 0;
            if (value != null)
            {
                switch (value.ToLower())
                {
                    case "stark":
                        idx = 0;
                        break;
                    case "lannister":
                        idx = 1;
                        break;
                    case "baratheon":
                        idx = 2;
                        break;
                    default:
                        break;

                }
            }
            return idx;
        }
    }

    public class ItemClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private BitmapImage image;
        public BitmapImage Image
        {
            get { return image; }
            set
            {
                image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        }

        public string Name { get; set; }
    }
}
