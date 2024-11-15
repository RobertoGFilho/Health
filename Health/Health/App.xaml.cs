using Health.Services;
using Health.ViewModels;
using Health.Views;
using System;
using Xamarin.Forms;

namespace Health
{
    public partial class App : Application
    {
        public static string DatabasePath { get; private set; }
        public App(string dbPath)
        {
            InitializeComponent();

            ConfigureServices(dbPath);
            DatabasePath = dbPath;
            MainPage = new NavigationPage(DependencyService.Get<HealthPage>());
        }

        private void ConfigureServices(string dbPath)
        {
            DependencyService.RegisterSingleton(new HealthDataService(dbPath));
            DependencyService.Register<HealthViewModel>();
            DependencyService.Register<HealthPage>();
        }
    }
}
