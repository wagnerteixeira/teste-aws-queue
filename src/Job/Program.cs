using Job;
using Job.Configuration;
using Serilog;

IConfiguration? configuration = null;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        configuration = builder.Configuration;
        services.AddSettings(builder.Configuration);
        services.AddRepositories(builder.Configuration);
        services.AddBusinessLogic();
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(builder =>
    {
        builder.AddLogging(configuration!);
    })
    .Build();

await host.RunAsync();
