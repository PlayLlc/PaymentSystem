using Microsoft.Extensions.Logging;
using Play.Core.Exceptions;
using Play.Domain.Exceptions;
using Play.Globalization.Time;
using Play.Scheduling;
using Play.Underwriting.Common.Exceptions;
using Play.Underwriting.DataServices.USTreasury;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Repositories;
using Play.Underwriting.Parser;
using Play.Underwriting.Parser.Mappings;
using Quartz;
using System.Data;
using TinyCsvParser;

namespace Play.Underwriting.Jobs;

public class ImportSanctionListsJob : IScheduledCronJob
{
    private readonly IUsTreasuryClient _DataServiceClient;
    private readonly IUnderwritingRepository _UnderwritingRepository;
    private readonly ILogger<ImportSanctionListsJob> _Logger;

    private const string prim_file = "sdn.csv";
    private const string addr_file = "add.csv";
    private const string alt_file = "alt.csv";

    private const string cons_prim_file = "cons_prim.csv";
    private const string cons_addr_file = "cons_add.csv";
    private const string cons_alt_file = "cons_alt.csv";

    public ImportSanctionListsJob(IUsTreasuryClient usTreasuryClient, IUnderwritingRepository underwritingRepository, ILogger<ImportSanctionListsJob> logger)
    {
        _DataServiceClient = usTreasuryClient;
        _UnderwritingRepository = underwritingRepository;
        _Logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _Logger.LogInformation("Job {jobKey}: starting job", context.JobDetail.Key);

        try
        {
            await _UnderwritingRepository.BackupAndResetData();

            Task<IEnumerable<Individual>> individualsTask = ImportAndProcessSanctionLists();

            Task<IEnumerable<Individual>> consolidatedIndividualsTask = ImportAndProcessConsolidatedSanctionLists();

            await Task.WhenAll(individualsTask, consolidatedIndividualsTask);

            IEnumerable<Individual> individuals = individualsTask.Result;
            IEnumerable<Individual> consolidatedIndividuals = consolidatedIndividualsTask.Result;

            var result = individuals.Concat(consolidatedIndividuals).ToLookup(pair => pair.Number, pair => pair).ToDictionary(group => group.Key, group => group.First());

            await SaveItems(result);

            _Logger.LogInformation("Job {jobKey}: job finished running successfully at {dateTime}", context.JobDetail.Key, DateTimeUtc.Now);
        }
        catch (Exception ex) when(ex is HttpRequestException || ex is InvalidOperationException)
        {
            _Logger.LogError(ex, "Job {jobKey}: could not request the necessary data !", context.JobDetail.Key);

            await _UnderwritingRepository.RestoreData().ConfigureAwait(false);

            if (context.RefireCount < 3)
                throw new JobExecutionException(ex, true);

            throw new JobExecutionException(ex, true);
        }
        catch(ParsingException ex)
        {
            _Logger.LogError(ex, "Job {jobKey}: could not parse the imported sanctions lists", context.JobDetail.Key);

            await _UnderwritingRepository.RestoreData().ConfigureAwait(false);

            throw new JobExecutionException(ex, false) { UnscheduleAllTriggers = true };
        }
        catch(RepositoryException ex)
        {
            _Logger.LogError(ex, "Job {jobKey}: could not persist the imported sanctions lists", context.JobDetail.Key);

            await _UnderwritingRepository.RestoreData().ConfigureAwait(false);

            throw new JobExecutionException(ex, false) { UnscheduleAllTriggers = true };
        }
        catch(PlayInternalException ex)
        {
            _Logger.LogError(ex, "Job {jobKey}: something wrong happened", context.JobDetail.Key);

            await _UnderwritingRepository.RestoreData().ConfigureAwait(false);

            throw new JobExecutionException(ex, false) { UnscheduleAllTriggers = true };
        }
        finally
        {
            await _UnderwritingRepository.CleanBackups().ConfigureAwait(false);
        }
    }

    private async Task<IEnumerable<Individual>> ImportAndProcessSanctionLists()
    {
        CsvParserOptions options = new CsvParserOptions(skipHeader: false, ',');

        Task<IEnumerable<Individual>> individualsTask = ImportIndividuals(options, prim_file);
        Task<IEnumerable<Address>> addressesTask = ImportAddresses(options, addr_file);
        Task<IEnumerable<Alias>> alternateIdentitiesTask = ImportAliases(options, alt_file);

        await Task.WhenAll(individualsTask, addressesTask, alternateIdentitiesTask);

        IEnumerable<Individual> individuals = individualsTask.Result;
        IEnumerable<Address> addresses = addressesTask.Result;
        IEnumerable<Alias> alternateIdentities = alternateIdentitiesTask.Result;

        UpdateIndividuals(individuals, addresses, alternateIdentities);

        return individuals;
    }

    private async Task<IEnumerable<Individual>> ImportAndProcessConsolidatedSanctionLists()
    {
        CsvParserOptions options = new CsvParserOptions(skipHeader: false, ',');

        Task<IEnumerable<Individual>> individualsTask = ImportIndividuals(options, cons_prim_file, true);
        Task<IEnumerable<Address>> addressesTask = ImportAddresses(options, cons_addr_file, true);
        Task<IEnumerable<Alias>> alternateIdentitiesTask = ImportAliases(options, cons_alt_file, true);

        await Task.WhenAll(individualsTask, addressesTask, alternateIdentitiesTask);

        IEnumerable<Individual> individuals = individualsTask.Result;
        IEnumerable<Address> addresses = addressesTask.Result;
        IEnumerable<Alias> alternateIdentities = alternateIdentitiesTask.Result;

        UpdateIndividuals(individuals, addresses, alternateIdentities);

        return individuals;
    }

    private void UpdateIndividuals(IEnumerable<Individual> individuals, IEnumerable<Address> addresses, IEnumerable<Alias> alternateIdentities)
    {
        Dictionary<ulong, Address[]> individualsAddresses = addresses.GroupBy(o => o.IndividualNumber).ToDictionary(g => g.Key, g => g.ToArray());
        Dictionary<ulong, Alias[]> individualsAlternateIdentities = alternateIdentities.GroupBy(o => o.IndividualNumber).ToDictionary(g => g.Key, g => g.ToArray());

        foreach (Individual individual in individuals)
        {
            if (individualsAddresses.TryGetValue(individual.Number, out Address[]? individualAddresses))
                individual.AddAddresses(individualAddresses);

            if (individualsAlternateIdentities.TryGetValue(individual.Number, out Alias[]? individualAlternateIdentities))
                individual.AddAlternateIdentities(individualAlternateIdentities);
        }
    }

    private async Task SaveItems(Dictionary<ulong, Individual> individuals)
    {
        await _UnderwritingRepository.AddIndividuals(individuals.Select(x => x.Value));

        await _UnderwritingRepository.SaveChangesAsync();
    }

    private async Task<IEnumerable<Individual>> ImportIndividuals(CsvParserOptions parserOptions, string fileName, bool isConsolidatedFile = false)
    {
        string content;

        if (isConsolidatedFile)
            content = await _DataServiceClient.GetConsolidatedCsvFile(fileName).ConfigureAwait(false);
        else
            content = await _DataServiceClient.GetCsvFile(fileName).ConfigureAwait(false);

        var result = CsvParser.ParseFromString(FileHelper.SanitizeConsolidatedCsvListFile(content, fileName), parserOptions, new IndividualCsvMapping(), fileName).ToList();

        return result.Select(x => x.Result);
    }

    private async Task<IEnumerable<Address>> ImportAddresses(CsvParserOptions parserOptions, string fileName, bool isConsolidatedFile = false)
    {
        string content;

        if (isConsolidatedFile)
            content = await _DataServiceClient.GetConsolidatedCsvFile(fileName).ConfigureAwait(false);
        else
            content = await _DataServiceClient.GetCsvFile(fileName).ConfigureAwait(false);

        var result = CsvParser.ParseFromString(FileHelper.SanitizeConsolidatedCsvListFile(content, fileName), parserOptions, new AddressCsvMapping(), fileName).ToList();

        return result.Select(x => x.Result);
    }

    private async Task<IEnumerable<Alias>> ImportAliases(CsvParserOptions parserOptions, string fileName, bool isConsolidatedFile = false)
    {
        string content;

        if (isConsolidatedFile)
            content = await _DataServiceClient.GetConsolidatedCsvFile(fileName).ConfigureAwait(false);
        else
            content = await _DataServiceClient.GetCsvFile(fileName).ConfigureAwait(false);

        var result = CsvParser.ParseFromString(FileHelper.SanitizeConsolidatedCsvListFile(content, fileName), parserOptions, new AliasCsvMapping(), fileName).ToList();

        return result.Select(x => x.Result);
    }

    public string GetCronSchedule()
    {
        throw new NotImplementedException();
    }

    public string GetJobName()
    {
        throw new NotImplementedException();
    }
}
