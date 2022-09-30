using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Play.Messaging.Tests.Data.Messages;
using Play.Messaging.Tests.Data.Threads;

using Xunit;

namespace Play.Messaging.Tests.Threads;

public class CommandProcessingQueueTests
{
    #region InstanceValues

    private static readonly Random _Random = new Random();

    #endregion

    #region Instance Members

    [Fact]
    public void CommandProcessingQueue_EnqueueTestChannel1RequestMessage_MessageGetsEnqueuedAndProcessed()
    {
        TestCommandProcessingQueue _Queue = new();

        int expected = 22;
        TestChannel1RequestMessage message = new(expected);

        _Queue.Enqueue(message);

        Thread.Sleep(100);

        _Queue.ReceivingQueue.TryDequeue(out Message? actual);

        Assert.NotNull(actual);
        Assert.True(actual is TestChannel1RequestMessage);

        var testMessage = actual as TestChannel1RequestMessage;

        Assert.Equal(expected, testMessage.GetValue());
    }

    [Fact]
    public void CommandProcessingQueue_EnqueueTestChannel1MultipleRequestMessages_MessageGetsEnqueuedAndProcessed()
    {
        TestCommandProcessingQueue _Queue = new();

        List<int> expectedValues = new List<int>();

        for (int i = 0; i < 50; i++)
        {
            int value = (int)_Random.Next(0, int.MaxValue);
            TestChannel1RequestMessage message = new(value);
            _Queue.Enqueue(message);
            expectedValues.Add(value);
        }

        Thread.Sleep(100);

        Assert.Equal(50, _Queue.ReceivingQueue.Count);

        while(_Queue.ReceivingQueue.TryDequeue(out Message? message))
        {
            TestChannel1RequestMessage command = message as TestChannel1RequestMessage;

            Assert.True(expectedValues.Contains(command.GetValue()));
        }
    }

    [Fact]
    public void CommandProcessingQueueInMultipleConcurrencyScenario_EnqueueingMultipleMessages_MessagesGetEnqueuedAndProcessed()
    {
        TestCommandProcessingQueue _Queue = new();

        List<Task> processingTasks = new List<Task>();
        ConcurrentBag<int> expectedValues = new ConcurrentBag<int>();

        for (int i = 0; i < 100; i++)
        {
            Task t = Task.Run(() =>
            {
                int value = (int)_Random.Next(0, int.MaxValue);
                expectedValues.Add(value);
                TestChannel1RequestMessage message = new(value);
                _Queue.Enqueue(message);
            });

            processingTasks.Add(t);
        }

        Task.WaitAll(processingTasks.ToArray());

        ////Small sleep to be sure queue processing tasks finished.
        Thread.Sleep(100);

        Assert.Equal(100, _Queue.ReceivingQueue.Count);

        while (_Queue.ReceivingQueue.TryDequeue(out Message? message))
        {
            TestChannel1RequestMessage command = message as TestChannel1RequestMessage;

            Assert.NotNull(expectedValues.FirstOrDefault(x => x == command.GetValue()));
        }
    }

    [Fact]
    public void CommandProcessingQueueWithEnqueuedItems_Clear_ClearsQueue()
    {
        TestCommandProcessingQueue _Queue = new();

        for (int i = 0; i < 100; i++)
        {
            TestChannel1RequestMessage message = new(i);
            _Queue.Enqueue(message);
        }

        _Queue.Clear();

        Assert.Equal(0, _Queue.GetQueueLength());
    }

    [Fact]
    public void CommandProcessingQueue_CancelProcessing_NotAllElementsGetProcessed()
    {
        TestCommandProcessingQueue _Queue = new();

        List<Task> processingTasks = new List<Task>();
        ConcurrentBag<int> expectedValues = new ConcurrentBag<int>();

        for (int i = 0; i < 100; i++)
        {
            if (i == 50)
            {
                Task cancelTask = Task.Run(() => _Queue.Cancel());
                processingTasks.Add(cancelTask);
                continue;
            }

            Task t = Task.Run(() =>
            {
                int value = (int)_Random.Next(0, int.MaxValue);
                expectedValues.Add(value);
                TestChannel1RequestMessage message = new(value);
                _Queue.Enqueue(message);
            });

            processingTasks.Add(t);
        }

        Task.WaitAll(processingTasks.ToArray());

        Assert.True(_Queue.ReceivingQueue.Count > 0);
        Assert.True(_Queue.ReceivingQueue.Count < 100);
    }

    #endregion
}
