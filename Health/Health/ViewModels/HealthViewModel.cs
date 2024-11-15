using Health.Models;
using Health.Services;
using MvvmHelpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Health.ViewModels
{
public class HealthViewModel : BaseViewModel
    {
        private readonly IHealthKitService healthKitService;
        private readonly HealthDataService healthDataService;

        public HealthViewModel()
        {
            healthKitService = DependencyService.Get<IHealthKitService>();
            healthDataService = new HealthDataService(App.DatabasePath);
        }

        public async Task InitializeHealthKitAsync()
        {
            var authorized = await healthKitService.RequestAuthorizationAsync();
            if (authorized)
            {
                var steps = await healthKitService.GetStepCountAsync(DateTime.Now.Date, DateTime.Now);
                var healthData = new HealthData
                {
                    Date = DateTime.Now.Date,
                    Steps = steps
                };
                await healthDataService.SaveHealthDataAsync(healthData);
            }
        }
    }
}
