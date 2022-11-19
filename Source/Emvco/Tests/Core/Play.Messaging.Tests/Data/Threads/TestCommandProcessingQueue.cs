using System.Collections.Concurrent;
using System.Threading;

using Play.Messaging.Threads;

namespace Play.Messaging.Tests.Data.Threads;

internal class TestCommandProcessingQueue : CommandProcessingQueue<Message>
{
    private readonly ConcurrentQueue<Message> _ReceivingQueue;


    public TestCommandProcessingQueue() : base(new CancellationTokenSource())
    {
        _ReceivingQueue = new ConcurrentQueue<Message>();
    }

    public ConcurrentQueue<Message> ReceivingQueue => _ReceivingQueue;

    public void Clear() => base.Clear();

    public void Cancel() => base.Cancel();

    public int GetQueueLength() => _Queue.Count;

    protected override void Handle(Message command) => ConsumeMessage(command);

    private void ConsumeMessage(Message command)
    {
        _ReceivingQueue.Enqueue(command);
    }
}
