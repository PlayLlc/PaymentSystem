namespace Play.Domain.Repositories;

public interface IReadOnlyRepository<_Dto, in _TId> where _Dto : IDto where _TId : IEquatable<_TId>
{
    #region Instance Members

    public Task<_Dto?> GetByIdAsync(_TId id);
    public _Dto? GetById(_TId id);

    #endregion
}