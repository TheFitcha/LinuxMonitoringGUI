using StatuxGUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StatuxGUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            App.Current.Resources["PrimaryColor"] = AppSettingsManager.Settings["layout_color"];
            App.Current.Resources["FlyoutTextColor"] = AppSettingsManager.Settings["flyout_text_color"];
        }
    }
}