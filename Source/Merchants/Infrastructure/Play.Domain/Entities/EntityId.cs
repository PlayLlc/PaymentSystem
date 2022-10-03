using Play.Globalization.Time;

namespace Play.Domain.Entities;

public abstract record EntityId<_TId>
{
    #region Instance Values

    public readonly _TId Id;

    #endregion

    #region Constructor

    protected EntityId(_TId id)
    {
        Id = id;
    }

    #endregion

    #region Instance Members

    public static string GenerateStringId()
    {
        return Guid.NewGuid() + DateTimeUtc.Now.ToString();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(EntityId<_TId> left, _TId right)
    {
        return left!.Id!.Equals(right);
    }

    public static bool operator ==(_TId left, EntityId<_TId> right)
    {
        return right!.Id!.Equals(left);
    }

    public static implicit operator _TId(EntityId<_TId> enumObjectObject)
    {
        return enumObjectObject.Id;
    }

    public static bool operator !=(EntityId<_TId> left, _TId right)
    {
        return !left!.Id!.Equals(right);
    }

    public static bool operator !=(_TId left, EntityId<_TId> right)
    {
        return !right!.Id!.Equals(left);
    }

    #endregion
}