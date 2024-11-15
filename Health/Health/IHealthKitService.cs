using System;
using System.Threading.Tasks;

namespace Health
{
    public interface IHealthKitService
    {
        Task<bool> RequestAuthorizationAsync();
        Task<double> GetStepCountAsync(DateTime startDate, DateTime endDate);
    }
}