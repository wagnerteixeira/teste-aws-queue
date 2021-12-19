using Amazon.SQS;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Serilog;
using Shared.Models;

namespace Api.Configuration
{
    public static class Configurations
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAwsRepository, AwsRepository>();
            services.AddSingleton<IAwsDynamoRepository, AwsDynamoRepository>();
            return services;
        }

        public static void AddLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration);
            var logger = loggerConfig.CreateLogger();

            builder.Logging.AddSerilog(logger);
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