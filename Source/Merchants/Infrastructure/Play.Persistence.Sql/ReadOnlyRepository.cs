using Microsoft.Extensions.Logging;

using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Persistence.Sql;

public class ReadOnlyRepository<_Aggregate, _Dto, _TId> : IReadOnlyRepository<_Dto, _TId>
    where _Aggregate : Aggregate<_TId> where _Dto : IDto where _TId : IEquatable<_TId>
{
    #region Instance Values

    private readonly Repository<_Aggregate, _TId> _Repository;
    private readonly ILogger<Repository<_Aggregate, _TId>> _Logger;

    #endregion

    #region Constructor

    public ReadOnlyRepository(Repository<_Aggregate, _TId> repository, ILogger<Repository<_Aggregate, _TId>> logger)
    {
        _Repository = repository;
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public _Dto? GetById(_TId id)
    {
        try
        {
            return (_Dto?) _Repository.GetById(id)?.AsDto();
        }
        catch (Exception ex)
        {
            EntityFrameworkRepositoryException exception = new EntityFrameworkRepositoryException(
                $"The {nameof(ReadOnlyRepository<_Aggregate, _Dto, _TId>)} encountered an exception retrieving {nameof(_Dto)} with the Identifier: [{id}]", ex);
            _Logger.LogError(new EventId(id.GetHashCode(), $"{nameof(ReadOnlyRepository<_Aggregate, _Dto, _TId>)}-{nameof(_Aggregate)}-{nameof(GetById)}"),
                exception, exception.Message);

            throw exception;
        }
    }

    /// <exception cref="EntityFrameworkRepositoryException"></exception>
    public async Task<_Dto?> GetByIdAsync(_TId id)
    {
        try
        {
            return (_Dto?) (await _Repository.GetByIdAsync(id).ConfigureAwait(false))?.AsDto();
        }
        catch (Exception ex)
        {
            var exception = new EntityFrameworkRepositoryException(
                $"The {nameof(ReadOnlyRepository<_Aggregate, _Dto, _TId>)} encountered an exception retrieving {nameof(_Dto)} with the Identifier: [{id}]", ex);
            _Logger.LogError(new EventId(id.GetHashCode(), $"{nameof(ReadOnlyRepository<_Aggregate, _Dto, _TId>)}-{nameof(_Aggregate)}-{nameof(GetByIdAsync)}"),
                exception, exception.Message);

            throw exception;
        }
    }

    #endregion
}