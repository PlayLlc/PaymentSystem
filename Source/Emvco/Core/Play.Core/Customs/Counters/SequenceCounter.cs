using Play.Core.Exceptions;

namespace Play.Core;

/// <summary>
///     A simple counter that retains the value of each incremental occurrence. When the maximum sequence number is
///     reached, the value will rollback its value to the minimum assigned value
/// </summary>
public abstract class SequenceCounter
{
    #region Instance Values

    protected readonly int _IncrementValue;
    protected readonly int _MaximumValue;
    protected readonly int _MinimumValue;
    protected int _Value;

    #endregion

    #region Constructor

    protected SequenceCounter(int minimumValue, int maximumValue, int increment)
    {
        if (minimumValue > maximumValue)
        {
            throw new PlayInternalException(
                $"The {nameof(SequenceCounter)} could not be initialized because the minimum value is greater than the maximum value");
        }

        _Value = minimumValue;
        _IncrementValue = increment;
        _MinimumValue = minimumValue;
        _MaximumValue = maximumValue;
    }

    #endregion

    #region Instance Members

    public abstract void Increment();

    public virtual void Reset()
    {
        _Value = _MinimumValue;
    }

    public virtual int GetSequenceValue() => _Value;

    #endregion
}