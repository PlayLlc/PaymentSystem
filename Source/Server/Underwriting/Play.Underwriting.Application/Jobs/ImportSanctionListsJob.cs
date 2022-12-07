using Microsoft.Extensions.Logging;

using Play.Core.Exceptions;
using Play.Domain.Exceptions;
using Play.Globalization.Time;
using Play.Underwriting.Application.Common.Exceptions;
using Play.Underwriting.Application.DataServices.USTreasury;
using Play.Underwriting.Application.Parser;
using Play.Underwriting.Application.Parser.Mappings;
using Play.Underwriting.Domain.Aggregates;
using Play.Underwriting.Domain.Entities;
using Play.Underwriting.Domain.Repositories;

using Quartz;

using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Play.Underwriting.Application.Jobs;

public class ImportSanctionListsJob : IScheduledCronJob
{
    #region Static Metadata

    private const string _PrimFile = "sdn.csv";
    private const string _AddrFile = "add.csv";
    private const string _AltFile = "alt.csv";

    private const string _ConsPrimFile = "cons_prim.csv";
    private const string _ConsAddrFile = "cons_add.csv";
    private const string _ConsAltFile = "cons_alt.csv";

    #endregion

    #region Instance Values

    private readonly IUsTreasuryClient _DataServiceClient;
    private readonly IImportIndividualsRepository _ImportRepository;
    private readonly ILogger<ImportSanctionListsJob> _Logger;

    #endregion

    #region Constructor

    public ImportSanctionListsJob(IUsTreasuryClient usTreasuryClient, IImportIndividualsRepository importRepository, ILogger<ImportSanctionListsJob> logger)
    {
        _DataServiceClient = usTreasuryClient;
        _ImportRepository = importRepository;
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    public async Task Execute(IJobExecutionContext context)
    {
        _Logger.LogInformation("Job {jobKey}: starting job", context.JobDetail.Key);

        try
        {
            await _ImportRepository.BackupAndResetData();

            Task<IEnumerable<Individual>> individualsTask = ImportAndProcessSanctionLists();

            Task<IEnumerable<Individual>> consolidatedIndividualsTask = ImportAndProcessConsolidatedSanctionLists();

            await Task.WhenAll(individualsTask, consolidatedIndividualsTask);

            IEnumerable<Individual> individuals = individualsTask.Result;
            IEnumerable<Individual> consolidatedIndividuals = consolidatedIndividualsTask.Result;

            Dictionary<ulong, Individual> result = individuals.Concat(consolidatedIndividuals)
                .ToLookup(pair => pair.Number, pair => pair)
                .ToDictionary(group => group.Key, group => group.First());

            await SaveItems(result);

            _Logger.LogInformation("Job {jobKey}: job finished running successfully at {dateTime}", context.JobDetail.Key, DateTimeUtc.Now);
        }
        catch (Exception ex) when (ex is HttpRequestException || ex is InvalidOperationException)
        {
            _Logger.LogError(ex, "Job {jobKey}: could not request the necessary data !", context.JobDetail.Key);

            await _ImportRepository.RestoreData().ConfigureAwait(false);

            if (context.RefireCount < 3)
                throw new JobExecutionException(ex, true);

            throw new JobExecutionException(ex, true);
        }
        catch (ParsingException ex)
        {
            _Logger.LogError(ex, "Job {jobKey}: could not parse the imported sanctions lists", context.JobDetail.Key);

            await _ImportRepository.RestoreData().ConfigureAwait(false);

            throw new JobExecutionException(ex, false) {UnscheduleAllTriggers = true};
        }
        catch (RepositoryException ex)
        {
            _Logger.LogError(ex, "Job {jobKey}: could not persist the imported sanctions lists", context.JobDetail.Key);

            await _ImportRepository.RestoreData().ConfigureAwait(false);

            throw new JobExecutionException(ex, false) {UnscheduleAllTriggers = true};
        }
        catch (PlayInternalException ex)
        {
            _Logger.LogError(ex, "Job {jobKey}: something wrong happened", context.JobDetail.Key);

            await _ImportRepository.RestoreData().ConfigureAwait(false);

            throw new JobExecutionException(ex, false) {UnscheduleAllTriggers = true};
        }
        finally
        {
            await _ImportRepository.CleanBackups().ConfigureAwait(false);
        }
    }

    private async Task<IEnumerable<Individual>> ImportAndProcessSanctionLists()
    {
        CsvParserOptions options = new(false, ',');

        Task<IEnumerable<Individual>> individualsTask = ImportIndividuals(options, _PrimFile);
        Task<IEnumerable<Address>> addressesTask = ImportAddresses(options, _AddrFile);
        Task<IEnumerable<Alias>> alternateIdentitiesTask = ImportAliases(options, _AltFile);

        await Task.WhenAll(individualsTask, addressesTask, alternateIdentitiesTask);

        IEnumerable<Individual> individuals = individualsTask.Result;
        IEnumerable<Address> addresses = addressesTask.Result;
        IEnumerable<Alias> alternateIdentities = alternateIdentitiesTask.Result;

        UpdateIndividuals(individuals, addresses, alternateIdentities);

        return individuals;
    }

    private async Task<IEnumerable<Individual>> ImportAndProcessConsolidatedSanctionLists()
    {
        CsvParserOptions options = new(false, ',');

        Task<IEnumerable<Individual>> individualsTask = ImportIndividuals(options, _ConsPrimFile, true);
        Task<IEnumerable<Address>> addressesTask = ImportAddresses(options, _ConsAddrFile, true);
        Task<IEnumerable<Alias>> alternateIdentitiesTask = ImportAliases(options, _ConsAltFile, true);

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
        Dictionary<ulong, Alias[]> individualsAlternateIdentities =
            alternateIdentities.GroupBy(o => o.IndividualNumber).ToDictionary(g => g.Key, g => g.ToArray());

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
        await _ImportRepository.AddIndividuals(individuals.Select(x => x.Value));

        await _ImportRepository.SaveChangesAsync();
    }

    private async Task<IEnumerable<Individual>> ImportIndividuals(CsvParserOptions parserOptions, string fileName, bool isConsolidatedFile = false)
    {
        string content;

        if (isConsolidatedFile)
            content = await _DataServiceClient.GetConsolidatedCsvFile(fileName).ConfigureAwait(false);
        else
            content = await _DataServiceClient.GetCsvFile(fileName).ConfigureAwait(false);

        List<CsvMappingResult<Individual>> result = CsvParser.ParseFromString(FileHelper.SanitizeConsolidatedCsvListFile(content, fileName), parserOptions,
                new IndividualCsvMapping(), fileName)
            .ToList();

        return result.Select(x => x.Result);
    }

    private async Task<IEnumerable<Address>> ImportAddresses(CsvParserOptions parserOptions, string fileName, bool isConsolidatedFile = false)
    {
        string content;

        if (isConsolidatedFile)
            content = await _DataServiceClient.GetConsolidatedCsvFile(fileName).ConfigureAwait(false);
        else
            content = await _DataServiceClient.GetCsvFile(fileName).ConfigureAwait(false);

        List<CsvMappingResult<Address>> result = CsvParser
            .ParseFromString(FileHelper.SanitizeConsolidatedCsvListFile(content, fileName), parserOptions, new AddressCsvMapping(), fileName)
            .ToList();

        return result.Select(x => x.Result);
    }

    private async Task<IEnumerable<Alias>> ImportAliases(CsvParserOptions parserOptions, string fileName, bool isConsolidatedFile = false)
    {
        string content;

        if (isConsolidatedFile)
            content = await _DataServiceClient.GetConsolidatedCsvFile(fileName).ConfigureAwait(false);
        else
            content = await _DataServiceClient.GetCsvFile(fileName).ConfigureAwait(false);

        List<CsvMappingResult<Alias>> result = CsvParser
            .ParseFromString(FileHelper.SanitizeConsolidatedCsvListFile(content, fileName), parserOptions, new AliasCsvMapping(), fileName)
            .ToList();

        return result.Select(x => x.Result);
    }

    public string GetCronSchedule() => throw new NotImplementedException();

    public string GetJobName() => throw new NotImplementedException();

    #endregion
}