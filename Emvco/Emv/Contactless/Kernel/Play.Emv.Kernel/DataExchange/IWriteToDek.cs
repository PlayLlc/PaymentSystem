using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.DataExchange;

public interface IWriteToDek
{
    public void Initialize(DataExchangeRequest list);
    public void Initialize(DekRequestType dekRequestType);
    public void Enqueue(DekRequestType type, params Tag[] listItems);
    public int ResolveKnownObjects(DekRequestType requestType);
    public void Initialize(DataExchangeResponse list);
    public void Initialize(DekResponseType dekResponseType);
    public void Enqueue(DekResponseType type, params PrimitiveValue[] listItems);
    public int Resolve(DekResponseType requestType, params PrimitiveValue[] values);
}