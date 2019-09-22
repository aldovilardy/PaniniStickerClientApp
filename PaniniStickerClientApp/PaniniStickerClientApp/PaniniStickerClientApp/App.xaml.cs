using PaniniStickerClientApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PaniniStickerClientApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            //MainPage = new MediaPage();
            MainPage = new NavigationPage(new FormPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
