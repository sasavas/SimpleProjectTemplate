using SimpleProjectTemplate.Application.Ports;

namespace SimpleProjectTemplate.Infrastructure.Mocks;

public class MessageSenderGatewayMock : IMessageSenderGateway
{
    public Task SendMessageAsync(string queueName, object messageContent)
    {
        Console.WriteLine($"Message sent to queue {queueName}, message content: {messageContent}");
        return Task.CompletedTask;
    }
}