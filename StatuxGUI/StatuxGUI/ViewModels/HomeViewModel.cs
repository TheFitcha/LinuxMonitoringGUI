using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MvvmHelpers;
using StatuxGUI.Services;

namespace StatuxGUI.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private string backgroundColor;
        public string BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }

        public HomeViewModel()
        {
            BackgroundColor = AppSettingsManager.Settings["background_color"];
        }
    }
}
