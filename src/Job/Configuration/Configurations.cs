using System.Data;
using Amazon.SQS;
using BusinessLogic;
using BusinessLogic.Interfaces;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Shared.Models;
using Npgsql;

namespace Job.Configuration
{
    public static class Configurations
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAwsRepository, AwsRepository>();
            services.AddSingleton<IPostgreSqlRepository, PostgreSqlRepository>();
            var appSettings = configuration.Get<JobAppSettings>();
            var postgresSettings = appSettings.PostgresSettings;
            var connectionString = $"Host={postgresSettings.Host};Username={postgresSettings.Username};Password={postgresSettings.Password};Database={postgresSettings.Database};Port={postgresSettings.Port}";
            Console.WriteLine($"connectionString-> {connectionString}");
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(connectionString));

            return services;
        }

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            return services.AddSingleton<IProcessMessages, ProcessMessages>();
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.Get<JobAppSettings>();
            services.AddSingleton(appSettings);
            services.AddSingleton<AppSettings>(appSettings);
            services.AddAWSService<IAmazonSQS>();

            var options = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(options);
            // TODO testar services.Configure<AppSettings>(configuration);
            return services;
        }
    }
}