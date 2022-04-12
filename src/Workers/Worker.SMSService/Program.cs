using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;
using Twilio;
using Worker.SMSService.Consumers;
using Worker.SMSService.Models;

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
        services.Configure<TwilioOptions>(context.Configuration.GetSection("Twilio"));

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
    })
    .Build();

var twilioOptions = host.Services.GetRequiredService<IOptions<TwilioOptions>>().Value;
TwilioClient.Init(
    username: twilioOptions.AccountSid,
    password: twilioOptions.AuthToken
);

await host.RunAsync();
