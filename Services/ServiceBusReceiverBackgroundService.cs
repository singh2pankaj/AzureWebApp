using Azure.Messaging.ServiceBus;

public class ServiceBusReceiverBackgroundService : BackgroundService
{
    private readonly IConfiguration _config;
    private ServiceBusProcessor _processor;

    public ServiceBusReceiverBackgroundService(IConfiguration config)
    {
        _config = config;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        string connectionString = _config["ServiceBus:ConnectionString"];
        string queueName = _config["ServiceBus:QueueName"];

        var client = new ServiceBusClient(connectionString);

        _processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync();
        Console.WriteLine("Service Bus Receiver Started.");

        await base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Nothing required here because StartAsync() already starts the processor.
        // But this method must exist.
        return Task.CompletedTask;
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();

        Console.WriteLine($"Received message: {body}");

        // Here do your work
        // Save to DB, send email, call APIs, logs, etc.

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error receiving message: {args.Exception}");
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
        Console.WriteLine("Service Bus Receiver Stopped.");
        await base.StopAsync(cancellationToken);
    }
}