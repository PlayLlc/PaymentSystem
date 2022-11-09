using Play.Underwriting.Domain.Entities;

namespace Play.Underwriting.Domain.Repositories;

public interface IUnderwritingRepository
{
    void AddIndividual(Individual individual);

    Task AddIndividuals(IEnumerable<Individual> individuals);

    Task SaveChangesAsync();

    Task BackupAndResetData();

    Task RestoreData();

    Task CleanBackups();

    void ResetChangeTracker();
}
