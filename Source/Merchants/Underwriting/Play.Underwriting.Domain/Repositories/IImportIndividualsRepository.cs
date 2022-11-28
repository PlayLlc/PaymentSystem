using Play.Underwriting.Domain.Aggregates;

namespace Play.Underwriting.Domain.Repositories;

public interface IImportIndividualsRepository
{
    #region Instance Members

    void AddIndividual(Individual individual);

    Task AddIndividuals(IEnumerable<Individual> individuals);

    Task SaveChangesAsync();

    Task BackupAndResetData();

    Task RestoreData();

    Task CleanBackups();

    void ResetChangeTracker();

    #endregion
}