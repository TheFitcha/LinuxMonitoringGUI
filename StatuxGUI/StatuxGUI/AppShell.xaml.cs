using StatuxGUI.Services;
using StatuxGUI.ViewModels;
using StatuxGUI.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace StatuxGUI
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProcessListPage), typeof(ProcessListPage));
            Routing.RegisterRoute(nameof(MachineDetailsPage), typeof(MachineDetailsPage));
            Routing.RegisterRoute(nameof(ProcessDetailsPage), typeof(ProcessDetailsPage));
        }
    }
}
