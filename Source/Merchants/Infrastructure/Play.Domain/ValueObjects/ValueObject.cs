namespace Play.Domain.ValueObjects;

public abstract record ValueObject<_T>
{
    #region Instance Values

    public _T Value { get; protected set; }

    #endregion

    #region Constructor

    protected ValueObject()
    { }

    protected ValueObject(_T value)
    {
        Value = value;
    }

    #endregion
}