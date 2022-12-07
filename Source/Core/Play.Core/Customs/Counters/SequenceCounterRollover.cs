using System;

namespace Play.Core;

public class SequenceCounterRollover : SequenceCounter
{
    #region Instance Values

    /// <summary>
    ///     An option action that occurs when the maximum value is reached and the counter resets its value to the minimum
    ///     value
    /// </summary>
    private readonly Action? _ThresholdCallback;

    #endregion

    #region Constructor

    public SequenceCounterRollover(int minimumValue, int maximumValue, int increment) : base(minimumValue, maximumValue, increment)
    { }

    public SequenceCounterRollover(int minimumValue, int maximumValue, int increment, Action thresholdCallback) : base(minimumValue, maximumValue, increment)
    {
        _ThresholdCallback = thresholdCallback;
    }

    #endregion

    #region Instance Members

    public override void Increment()
    {
        if (_Value == _MaximumValue)
        {
            _Value = _MinimumValue;

            if (_ThresholdCallback is not null)
                _ThresholdCallback.Invoke();
        }
    }

    #endregion
}