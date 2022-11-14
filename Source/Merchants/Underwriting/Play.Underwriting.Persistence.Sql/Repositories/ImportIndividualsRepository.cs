using Microsoft.EntityFrameworkCore;
using Play.Persistence.Sql;
using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Persistence.Persistence;

namespace Play.Underwriting.Persistence.Sql.Repositories;

public class ImportIndividualsRepository : IImportIndividualsRepository
{
    #region Instance Values

    private readonly DbContext _DbContext;
    private readonly DbSet<Individual> _Individuals;

    #endregion

    #region Constructor

    public ImportIndividualsRepository(UnderwritingDbContext dbContext)
    {
        _DbContext = dbContext;
        _Individuals = _DbContext.Set<Individual>();
    }

    #endregion

    #region Instance Members

    public void AddIndividual(Individual individual)
    {
        _Individuals.Add(individual);
    }

    public async Task AddIndividuals(IEnumerable<Individual> individuals)
    {
        await _Individuals.AddRangeAsync(individuals);
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new EntityFrameworkRepositoryException(
                    $"The {nameof(ImportIndividualsRepository)} encountered an exception while {nameof(SaveChangesAsync)}", ex);
        }
    }

    public async Task BackupAndResetData()
    {
        await BackupData();

        await ResetData();
    }

    public void ResetChangeTracker()
    {
        _DbContext.ChangeTracker.Clear();
    }

    private async Task ResetData()
    {
        using var transaction = _DbContext.Database.BeginTransaction();

        try
        {
            await _DbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE dbo.Aliases");
            await _DbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE dbo.Addresses");
            await _DbContext.Database.ExecuteSqlRawAsync($"DELETE FROM dbo.Individuals");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new EntityFrameworkRepositoryException(
                    $"The {nameof(ImportIndividualsRepository)} encountered an exception while {nameof(SaveChangesAsync)}", ex);

        }
        finally
        {
            await transaction.CommitAsync();
        }
    }

    private async Task BackupData()
    {
        using var transaction = _DbContext.Database.BeginTransaction();

        try
        {
            await _DbContext.Database.ExecuteSqlRawAsync($"SELECT * INTO Individuals_Backup FROM Individuals");
            await _DbContext.Database.ExecuteSqlRawAsync($"SELECT * INTO Addresses_Backup  FROM Addresses");
            await _DbContext.Database.ExecuteSqlRawAsync($"SELECT * INTO Aliases_Backup  FROM Aliases");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new EntityFrameworkRepositoryException(
                    $"The {nameof(ImportIndividualsRepository)} encountered an exception while {nameof(SaveChangesAsync)}", ex);

        }
        finally
        {
            await transaction.CommitAsync();
        }
    }

    public async Task RestoreData()
    {
        using var transaction = _DbContext.Database.BeginTransaction();

        try
        {
            await _DbContext.Database.ExecuteSqlRawAsync($"INSERT INTO Individuals SELECT* FROM Individuals_Backup");
            await _DbContext.Database.ExecuteSqlRawAsync($"INSERT INTO Addresses SELECT * FROM Addresses_Backup");
            await _DbContext.Database.ExecuteSqlRawAsync($"INSERT INTO Aliases SELECT * FROM Aliases_Backup");

            await _DbContext.Database.ExecuteSqlRawAsync($"DROP TABLE Addresses_Backup ");
            await _DbContext.Database.ExecuteSqlRawAsync($"DROP TABLE Aliases_Backup");
            await _DbContext.Database.ExecuteSqlRawAsync($"DROP TABLE Individuals_Backup");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            throw new EntityFrameworkRepositoryException(
                    $"The {nameof(ImportIndividualsRepository)} encountered an exception while {nameof(SaveChangesAsync)}", ex);

        }
        finally
        {
            await transaction.CommitAsync();
        }
    }

    public async Task CleanBackups()
    {
        using var transaction = _DbContext.Database.BeginTransaction();

        try
        {
            await _DbContext.Database.ExecuteSqlRawAsync($"DROP TABLE Addresses_Backup ");
            await _DbContext.Database.ExecuteSqlRawAsync($"DROP TABLE Aliases_Backup");
            await _DbContext.Database.ExecuteSqlRawAsync($"DROP TABLE Individuals_Backup");
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();

            throw new EntityFrameworkRepositoryException(
                $"The {nameof(ImportIndividualsRepository)} encountered an exception while {nameof(SaveChangesAsync)}", ex);
        }
        finally
        {
            await transaction.CommitAsync();
        }
    }

    #endregion
}
