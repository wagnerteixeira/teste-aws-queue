using System.Data;
using Amazon.SQS;
using BusinessLogic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Shared.Models;
using Npgsql;
using Serilog;

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
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(connectionString));
            services.AddSingleton<IAwsDynamoRepository<MessageModel>, AwsDynamoRepository<MessageModel>>();
            return services;
        }

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            return services.AddSingleton<IProcessMessages, ProcessMessages>();
        }

        public static void AddLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.ClearProviders();
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);
            var logger = loggerConfig.CreateLogger();
            builder.AddSerilog(logger);
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.Get<JobAppSettings>();
            services.AddSingleton(appSettings);
            services.AddSingleton<AppSettings>(appSettings);
            var options = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(options);
            
            services.AddAWSService<IAmazonSQS>();
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddTransient<IDynamoDBContext, DynamoDBContext>();

            
            // TODO testar services.Configure<AppSettings>(configuration);
            return services;
        }
    }
}