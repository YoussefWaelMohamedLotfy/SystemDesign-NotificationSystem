using MassTransit;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using Worker.EmailService.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog((context, configuration) =>
    {
        configuration
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSendGrid(options =>
        {
            options.ApiKey = context.Configuration.GetValue<string>("SendGridApiKey");
        });

        services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(true));

            x.AddConsumer<SendNotificationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => 
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });


        });

        //services.AddHostedService<EmailWorker>();

    })
    .Build();

await host.RunAsync();
