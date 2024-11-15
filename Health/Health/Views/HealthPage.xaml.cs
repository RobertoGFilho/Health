using Health.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Health.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthPage : ContentPage
    {
        private readonly HealthViewModel viewModel;

        public HealthPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HealthViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.InitializeHealthKitAsync();
        }
    }
}