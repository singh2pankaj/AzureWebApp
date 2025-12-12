using Azure.Messaging.ServiceBus;

public class ServiceBusSenderService
{
    private readonly string _connectionString;
    private readonly string _queueName;

    public ServiceBusSenderService(IConfiguration config)
    {
        _connectionString = config["ServiceBus:ConnectionString"];
        _queueName = config["ServiceBus:QueueName"];
    }

    public async Task SendMessageAsync(string messageText)
    {
        await using var client = new ServiceBusClient(_connectionString);
        var sender = client.CreateSender(_queueName);

        var message = new ServiceBusMessage(messageText);
        await sender.SendMessageAsync(message);
    }
}