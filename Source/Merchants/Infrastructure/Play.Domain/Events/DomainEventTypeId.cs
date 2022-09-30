using System.Text;

namespace Play.Domain.Events;

public readonly record struct DomainEventTypeId
{
    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public DomainEventTypeId(string type)
    {
        _Value = 17;

        byte[] bytes = Encoding.ASCII.GetBytes(type);

        for (int index = 0; index < bytes.Length; index++)
            unchecked
            {
                _Value *= bytes[index];
            }
    }

    #endregion

    #region Equality

    public bool Equals(DomainEventTypeId x, DomainEventTypeId y)
    {
        return x._Value == y._Value;
    }

    public int GetHashCode(DomainEventTypeId obj)
    {
        return obj._Value.GetHashCode();
    }

    #endregion
}