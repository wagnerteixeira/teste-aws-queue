using job;
using job.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddRepositories();          
        services.AddSettings(builder.Configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
