namespace SimpleProjectTemplate.SharedLibrary.AzureServiceBus.EmailQueue;

public sealed record EmailQueueMessageBody (
    string To,
    string Subject,
    string Body
) : IMessage;