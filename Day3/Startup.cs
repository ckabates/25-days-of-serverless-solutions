using Day3.Configurations;
using Day3.Handlers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Day3.Startup))]
namespace Day3
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<SecretSantaCosmodDbConfiguration>(config.GetSection("SecretSanta:CosmosDb"));

            builder.Services.AddScoped<IGitPushHandler, GitPushHandler>();
        }
    }
}
