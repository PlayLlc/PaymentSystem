using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Play.Core;

namespace Play.Emv.Kernel.Services;

public class ProbabilitySelectionQueue : IProbabilitySelectionQueue
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Values

    private readonly ushort _EnqueueCeiling;
    private readonly ushort _EnqueueFloor;
    private readonly ConcurrentQueue<Probability> _RandomNumberQueue;

    #endregion

    #region Constructor

    public ProbabilitySelectionQueue()
    {
        _RandomNumberQueue = new ConcurrentQueue<Probability>();
        _EnqueueFloor = 10;
        _EnqueueCeiling = 100;
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="enqueueCeiling"></param>
    /// <param name="enqueueFloor"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public ProbabilitySelectionQueue(ushort enqueueCeiling, ushort enqueueFloor)
    {
        if (enqueueFloor >= enqueueCeiling)
            throw new InvalidOperationException($"The argument {nameof(enqueueCeiling)} must be greater than {nameof(enqueueFloor)}");

        _RandomNumberQueue = new ConcurrentQueue<Probability>();
        _EnqueueFloor = enqueueFloor;
        _EnqueueCeiling = enqueueCeiling;
    }

    #endregion

    #region Instance Members

    private void EnqueueRandomPercentage()
    {
        _RandomNumberQueue.Enqueue(GetRandomPercentage());
    }

    private static Probability GetRandomPercentage() => new((byte) _Random.Next(0, 99));

    /*
     The terminal shall generate a random
     number in the range of 1 to 99. If this random number is less than or equal to the  ‘Target Percentage to be used for Random Selection’, the transaction shall be selected
     */
    public bool IsRandomSelection(Probability threshold)
    {
        if (!_RandomNumberQueue.TryDequeue(out Probability result))
        {
            Probability randomProbability = GetRandomPercentage();

            if (_RandomNumberQueue.Count < _EnqueueFloor)
                Task.WhenAny(UpdateQueue());

            return randomProbability <= threshold;
        }

        if (_RandomNumberQueue.Count < _EnqueueFloor)
            Task.WhenAny(UpdateQueue());

        return result <= threshold;
    }

    private async Task UpdateQueue()
    {
        await Task.Run(() =>
        {
            for (; _RandomNumberQueue.Count < _EnqueueCeiling;)
                EnqueueRandomPercentage();
        }).ConfigureAwait(false);
    }

    #endregion
}