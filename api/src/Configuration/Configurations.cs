using Amazon.SQS;
using Data.Repositories;
using Data.Repositories.Interfaces;
using CrossCutting.Models;

namespace Api.Configuration
{
    public static class Configurations
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAwsRepository, AwsRepository>();
            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
         {
            var appSettings = configuration.Get<AppSettings>();
            Console.WriteLine($"QueueUrl {appSettings.Queue.Url} Fifo {appSettings.Queue.Fifo}");
            services.AddSingleton(appSettings);
            // services.Configure<AppSettings>(configuration);
            services.AddAWSService<IAmazonSQS>();

            var options = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(options);
            // TODO testar services.Configure<AppSettings>(configuration);
            return services;
        }
    }
}