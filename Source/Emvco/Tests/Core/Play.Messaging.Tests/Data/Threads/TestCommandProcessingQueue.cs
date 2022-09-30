using System.Threading;

using Play.Messaging.Threads;

namespace Play.Messaging.Tests.Data.Threads;

internal class TestCommandProcessingQueue : CommandProcessingQueue<Message>
{
    public TestCommandProcessingQueue(CancellationTokenSource cancellationTokenSource) : base(cancellationTokenSource)
    {
    }

    protected override void Handle(Message command) => ConsumeMessage(command);

    private void ConsumeMessage(Message command)
    {}
}
