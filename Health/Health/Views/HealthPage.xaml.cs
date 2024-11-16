using Health.ViewModels;
using Microcharts;
using SkiaSharp;
using System.Linq;
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
            UpdateChart();
        }

        private void UpdateChart()
        {
            var entries = viewModel.HealthDataList.Select(data => new ChartEntry((float)data.Steps)
            {
                Label = data.Date.ToString("dd"),
                ValueLabel = data.Steps.ToString(),
                Color = SKColor.Parse("#00BFFF"),
                TextColor = SKColor.Parse("#00BFFF"),
                ValueLabelColor = SKColor.Parse("#00BFFF")
            }).ToArray();

            chartView.Chart = new PointChart
            {
                Entries = entries,
                LabelTextSize = 30,
                PointSize = 50,
                ValueLabelOrientation = Orientation.Horizontal, // Orientação horizontal para ValueLabels
                LabelOrientation = Orientation.Horizontal, // Orientação horizontal para Labels
                Margin = 20 // Adicionar margem para aumentar o espaço
            };
        }
    }
}