using Play.Ber.DataObjects;
using Play.Ber.Tags;

namespace Play.Emv.Kernel.DataExchange;

public interface IQueryDek
{
    #region Instance Members

    public bool IsEmpty(DekRequestType type);
    public bool IsEmpty(DekResponseType type);
    public bool TryPeek(DekRequestType type, out Tag result);
    public bool TryPeek(DekResponseType type, out PrimitiveValue? result);

    #endregion
}