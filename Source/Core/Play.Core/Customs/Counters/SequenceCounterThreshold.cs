namespace Play.Core;

/// <summary>
///     This <see cref="SequenceCounter" /> will continue to increment its internal value but when the threshold value is
///     reached it will return the threshold value until this counter is reset
/// </summary>
public class SequenceCounterThreshold : SequenceCounter
{
    #region Constructor

    public SequenceCounterThreshold(int minimumValue, int maximumValue, int increment) : base(minimumValue, maximumValue, increment)
    { }

    #endregion

    #region Instance Members

    public override void Increment()
    {
        _Value += _IncrementValue;
    }

    public override int GetSequenceValue() => _Value > _MaximumValue ? _MaximumValue : _Value;

    #endregion
}