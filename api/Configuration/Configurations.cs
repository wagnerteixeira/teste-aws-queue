using Amazon.SQS;
using api.Repositories;
using api.Repositories.Interfaces;

namespace api.Configuration
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