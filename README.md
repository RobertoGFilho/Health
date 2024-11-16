# Monitor de Saúde Pessoal



https://github.com/user-attachments/assets/e6a31254-c083-47e1-abcc-d23c5a78e692



## Descrição
O Monitor de Saúde Pessoal é um aplicativo completo para monitoramento de atividades físicas, saúde e alimentação. O app registra exercícios, sincroniza com APIs de saúde, exibe dados em tempo real e permite que o usuário acompanhe suas métricas ao longo do tempo.

## API Utilizada
- Apple HealthKit para iOS

**Nota:** A integração com a Google Fit API para Android foi descontinuada. O aplicativo atualmente suporta apenas a integração com a Apple HealthKit para dispositivos iOS.

## Requisitos

### Integração com APIs de saúde
- **HealthKit para iOS**: O aplicativo se integra com a Apple HealthKit para coletar dados de atividades físicas e saúde.

        public HealthKitService()
        {
            healthStore = new HKHealthStore();
            stepType = HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount);
        }

### Implementação de gráficos complexos
- **Gráficos de linha e barras**: O aplicativo utiliza gráficos de linha e barras para acompanhar o progresso do usuário ao longo do tempo. Os gráficos são implementados usando a biblioteca `Microcharts`.

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

### Sincronização de dados entre dispositivos e armazenamento local
- **Sincronização de dados**: O aplicativo sincroniza dados entre dispositivos usando a Apple HealthKit e armazena dados localmente para acesso offline. Isso garante que o usuário possa acessar suas métricas mesmo sem conexão à internet.

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

### Implementação de funcionalidades nativas avançadas
- **Integração com sensores**: O aplicativo integra-se com sensores como pedômetro e monitor de batimentos cardíacos para coletar dados em tempo real e fornecer informações precisas sobre a saúde do usuário.

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

### Uso do padrão MVVM com Dependency Injection
- **MVVM**: O aplicativo segue o padrão MVVM (Model-View-ViewModel) para separar a lógica de negócios da interface do usuário, facilitando a manutenção e a escalabilidade do código.
- **Dependency Injection**: O aplicativo utiliza injeção de dependência para gerenciar dependências e promover a reutilização de código.

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

### Diferenciação avançada de UI e UX entre Android e iOS
- **Human Interface Guidelines para iOS**: O aplicativo segue as diretrizes de Human Interface Guidelines para proporcionar uma experiência de usuário consistente e intuitiva em dispositivos iOS.

![Saude](https://github.com/user-attachments/assets/2504ad32-1605-4345-ab94-70ac33ff7997)


### Otimização do aplicativo para performance
- **Threading e tarefas assíncronas**: O aplicativo utiliza threading e tarefas assíncronas para otimizar a performance e garantir uma experiência de usuário fluida.

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
