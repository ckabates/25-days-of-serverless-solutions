using Day7.Configurations;
using Day7.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Day7.Startup))]
namespace Day7
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<UnsplashConfiguration>(config.GetSection("Unsplash"));

            builder.Services.AddHttpClient<IImageApiService, UnsplashApiService>();
        }
    }
}
