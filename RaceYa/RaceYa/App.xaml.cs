﻿using RaceYa.Helpers;
using RaceYa.Models;
using Xamarin.Forms;

namespace RaceYa
{
    public partial class App : Application
    {
        public static GlobalContext Context = GlobalContext.Instance();

        public static DBQuickStartService DBQuickStart = DBQuickStartService.Instance();

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
