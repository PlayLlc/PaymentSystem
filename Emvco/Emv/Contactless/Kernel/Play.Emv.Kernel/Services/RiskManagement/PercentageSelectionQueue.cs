using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Play.Core.Math;

namespace Play.Emv.Kernel.Services;

internal class PercentageSelectionQueue : IPercentageSelectionQueue
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Values

    private readonly ushort _EnqueueCeiling;
    private readonly ushort _EnqueueFloor;
    private readonly ConcurrentQueue<Percentage> _RandomNumberQueue;

    #endregion

    #region Constructor

    public PercentageSelectionQueue()
    {
        _RandomNumberQueue = new ConcurrentQueue<Percentage>();
        _EnqueueFloor = 10;
        _EnqueueCeiling = 100;
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="enqueueCeiling"></param>
    /// <param name="enqueueFloor"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public PercentageSelectionQueue(ushort enqueueCeiling, ushort enqueueFloor)
    {
        if (enqueueFloor >= enqueueCeiling)
            throw new InvalidOperationException($"The argument {nameof(enqueueCeiling)} must be greater than {nameof(enqueueFloor)}");

        _RandomNumberQueue = new ConcurrentQueue<Percentage>();
        _EnqueueFloor = enqueueFloor;
        _EnqueueCeiling = enqueueCeiling;
    }

    #endregion

    #region Instance Members

    private void EnqueueRandomPercentage()
    {
        _RandomNumberQueue.Enqueue(GetRandomPercentage());
    }

    private Percentage GetRandomPercentage() => new((byte) _Random.Next(0, 99));

    public async Task<bool> IsRandomSelection(Percentage threshold)
    {
        if (!_RandomNumberQueue.TryDequeue(out Percentage result))
        {
            Percentage randomPercentage = GetRandomPercentage();

            if (_RandomNumberQueue.Count < _EnqueueFloor)
                await UpdateQueue().ConfigureAwait(false);

            return randomPercentage <= threshold;
        }

        if (_RandomNumberQueue.Count < _EnqueueFloor)
            await UpdateQueue().ConfigureAwait(false);

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