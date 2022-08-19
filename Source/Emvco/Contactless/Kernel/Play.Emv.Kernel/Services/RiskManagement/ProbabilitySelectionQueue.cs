using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Play.Core;

namespace Play.Emv.Kernel.Services;

internal class ProbabilitySelectionQueue : IProbabilitySelectionQueue
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Constructor

    public ProbabilitySelectionQueue()
    { }

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
    }

    #endregion

    #region Instance Members

    private static Probability GetRandomPercentage() => new((byte) _Random.Next(0, 99));
    public bool IsRandomSelection(Probability threshold) => GetRandomPercentage() <= threshold;

    #endregion
}