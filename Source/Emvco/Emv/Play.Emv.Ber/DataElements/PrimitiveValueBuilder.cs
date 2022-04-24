using Play.Ber.DataObjects;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Provides mutable objects with an intermediate builder object to construct the final value of that object
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PrimitiveValueBuilder<T> where T : struct
{
    #region Instance Values

    protected T _Value;

    #endregion

    #region Instance Members

    public abstract PrimitiveValue Complete();
    protected abstract void Set(T bitsToSet);

    #endregion
}