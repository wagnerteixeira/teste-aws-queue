using Job;
using Job.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddSettings(builder.Configuration);
        services.AddRepositories(builder.Configuration);
        services.AddBusinessLogic();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
