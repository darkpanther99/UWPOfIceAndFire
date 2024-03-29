﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TXC54G_HF
{
    /// <summary>
    /// A xaml page for setting the logo, which appears in the application.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        /// <summary>
        /// Constructor, which loads the page and sets the radiobuttons'  to match the saved logo.
        /// </summary>
        public SettingsPage()
        {
            this.InitializeComponent();
            HouseButtons.SelectedIndex = ViewModel.GetCurrentlySetIndex(); 
        }

        /// <summary>
        /// Event handler, which lets the user go back to the previous xaml page.
        /// </summary>
        private void NavBarSide_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            _ = App.TryGoBack();
        }

        /// <summary>
        /// Event handler, which sends the selected radiobutton's value to the ViewModel.
        /// </summary>
        private void RadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is muxc.RadioButtons rb)
            {
                string housename = rb.SelectedItem as string;
                ViewModel.RadioButtonSelectionChanged(housename);
            }
        }
    }
}
