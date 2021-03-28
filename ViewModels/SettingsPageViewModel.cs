using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXC54G_HF.ViewModels.Utilities;
using Windows.UI.Xaml.Media.Imaging;

namespace TXC54G_HF.ViewModels
{
    /// <summary>
    /// VM of the Settings page, it handles the code behind's requests.
    /// </summary>
    class SettingsPageViewModel
    {
        /// <summary>
        /// A property, which wraps an image's location, andm akes it possible to databind it to the UI.
        /// </summary>
        public ImageWrapper imageitem { get; set; } = new ImageWrapper() { Image = new BitmapImage(new Uri("ms-appx:///Assets/starklogo.png")) };

        /// <summary>
        /// Changes the currently selected image.
        /// </summary>
        /// <param name="housename">The name of the house, that we want to see the logo of.</param>
        public void RadioButtonSelectionChanged(string housename)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["favouritehouse"] = housename;
            imageitem.Image = new BitmapImage(new Uri($"ms-appx:///Assets/{housename}logo.png"));
        }

        /// <summary>
        /// Returns the index of the currently selected house logo.
        /// </summary>
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

}
