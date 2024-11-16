using Foundation;
using HealthKit;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(Health.iOS.Services.HealthKitService))]
namespace Health.iOS.Services
{
    public class HealthKitService : IHealthKitService
    {
        private readonly HKHealthStore healthStore;
        private readonly HKQuantityType stepType;

        public HealthKitService()
        {
            healthStore = new HKHealthStore();
            stepType = HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount);
        }

        public async Task<bool> RequestAuthorizationAsync()
        {
            var readTypes = new NSSet(new[] { stepType });
            var writeTypes = new NSSet(new[] { stepType });

            var success = await healthStore.RequestAuthorizationToShareAsync(writeTypes, readTypes);

            if (success.Item2 != null)
            {
                throw new Exception(success.Item2.LocalizedDescription);
            }

            return success.Item1;
        }

        public async Task<double> GetStepCountAsync(DateTime startDate, DateTime endDate)
        {
            var predicate = HKQuery.GetPredicateForSamples(startDate.ToNSDate(), endDate.ToNSDate(), HKQueryOptions.None);

            var tcs = new TaskCompletionSource<double>();
            var statisticsQuery = new HKStatisticsQuery(stepType, predicate, HKStatisticsOptions.CumulativeSum, (query, result, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(new Exception(error.LocalizedDescription));
                    return;
                }

                var quantity = result?.SumQuantity();
                var steps = quantity?.GetDoubleValue(HKUnit.Count) ?? 0;
                tcs.SetResult(steps);
            });

            healthStore.ExecuteQuery(statisticsQuery);
            return await tcs.Task;
        }
    }
}