using Health.Models;
using Health.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Health.ViewModels
{
    public class HealthViewModel : BaseViewModel
    {
        private int totalSteps;
        private readonly IHealthKitService healthKitService;
        private readonly HealthDataService healthDataService;

        public ObservableRangeCollection<HealthData> HealthDataList { get; }
                
        public int TotalSteps
        {
            get => totalSteps;
            set => SetProperty(ref totalSteps, value);
        }

        public HealthViewModel()
        {
            healthKitService = DependencyService.Get<IHealthKitService>();
            healthDataService = new HealthDataService(App.DatabasePath);
            HealthDataList = new ObservableRangeCollection<HealthData>();
        }

        public async Task InitializeHealthKitAsync()
        {
            var authorized = await healthKitService.RequestAuthorizationAsync();
            if (authorized)
            {
                await UpdateHealthDataAsync();
                await LoadHealthDataAsync();
            }
        }

        private async Task UpdateHealthDataAsync()
        {
            // Delete all existing data
            await healthDataService.DeleteAllHealthDataAsync();

            // Get steps for the last 7 days
            var stepsList = await GetStepsForLast7DaysAsync();

            // Save the new data
            await SaveStepsToDatabaseAsync(stepsList);
        }

        private async Task<List<HealthData>> GetStepsForLast7DaysAsync()
        {
            var stepsList = new List<HealthData>();
            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.Today.AddDays(-i);
                var steps = await healthKitService.GetStepCountAsync(date, date.AddDays(1));
                stepsList.Add(new HealthData { Date = date, Steps = steps });
            }
            return stepsList.OrderBy(o=> o.Date).ToList();
        }

        private async Task SaveStepsToDatabaseAsync(List<HealthData> stepsList)
        {
            foreach (var stepData in stepsList)
            {
                await healthDataService.SaveHealthDataAsync(stepData);
            }
        }

        private async Task LoadHealthDataAsync()
        {
            var healthData = await healthDataService.GetHealthDataAsync();
            HealthDataList.ReplaceRange(healthData);
            TotalSteps = (int)healthData.Sum(data => data.Steps);
        }
    }
}
