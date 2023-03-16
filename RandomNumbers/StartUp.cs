using Microsoft.Extensions.DependencyInjection;

namespace RandomNumbers
{
    public class StartUp
    {
        private readonly IServiceProvider _serviceProvider;

        public StartUp()
        {
            _serviceProvider = new ServiceCollection()
                .AddTransient<IRandomNumbersService, RandomNumbersService>()
                .BuildServiceProvider();
        }

        public void Start()
        {
            _serviceProvider.GetService<IRandomNumbersService>().Run();
        }
    }
}
