using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.GetData;

namespace Play.Emv.Pcd.Services;

// TODO: Implement some IDisposable patterns so we can wrap the session in a using
/// <summary>
///     A facade that encapsulates card reading and writing functionality
/// </summary>
public class CardClient : IReadApplicationData, ISelectApplicationDefinitionFileInformation, ISelectDirectoryDefinitionFileInformation,
    ISelectProximityPaymentSystemEnvironmentInfo, ISendPoiInformation, IGetProcessingOptions, IReadIccDataBatch, IManagePcdLifecycle
{
    #region Instance Values

    private readonly IReadApplicationData _ApplicationDataClient;
    private readonly ISelectApplicationDefinitionFileInformation _AppletClient;
    private readonly ISelectDirectoryDefinitionFileInformation _DirectoryFciClient;
    private readonly ISelectProximityPaymentSystemEnvironmentInfo _PpseClient;
    private readonly ISendPoiInformation _PoiClient;
    private readonly IGetProcessingOptions _GpoClient;
    private readonly IManagePcdLifecycle _PcdClient;
    private readonly IReadIccDataBatch _DataBatchReader;

    #endregion

    #region Constructor

    public CardClient(IProximityCouplingDeviceClient client)
    {
        _PcdClient = client;
        _DataBatchReader = new DataBatchReader(new DataReader(client));

        _AppletClient = new ApplicationDefinitionFileInfoSelector(client);
        _DirectoryFciClient = new DirectoryDefinitionFileInformationSelector(client);
        _PpseClient = new ProximityPaymentSystemEnvironmentInfoSelector(client);
        _PoiClient = default;
        _GpoClient = new ProcessingOptionsRetriever(client);
        _ApplicationDataClient = new ApplicationDataReader(new ElementaryFileRecordReader(client));
    }

    #endregion

    #region Instance Members

    public async Task<GetProcessingOptionsResponse> Transceive(GetProcessingOptionsRequest command) =>
        await _GpoClient.Transceive(command).ConfigureAwait(false);

    public async Task<SelectProximityPaymentSystemEnvironmentResponse> Transceive(SelectProximityPaymentSystemEnvironmentRequest command) =>
        await _PpseClient.Transceive(command).ConfigureAwait(false);

    public void Abort()
    {
        _PcdClient.Abort();
    }

    public void Activate()
    {
        _PcdClient.Activate();
    }

    public void CloseSession()
    {
        _PcdClient.CloseSession();
    }

    public void CloseSessionCardCheck()
    {
        _PcdClient.CloseSessionCardCheck();
    }

    public async Task<ReadApplicationDataResponse> Transceive(ReadApplicationDataRequest command) =>
        await _ApplicationDataClient.Transceive(command).ConfigureAwait(false);

    public async Task<SelectApplicationDefinitionFileInfoResponse> Transceive(SelectApplicationDefinitionFileInfoRequest command) =>
        await _AppletClient.Transceive(command).ConfigureAwait(false);

    public async Task<SelectDirectoryDefinitionFileResponse> Transceive(SelectDirectoryDefinitionFileRequest command) =>
        await _DirectoryFciClient.Transceive(command).ConfigureAwait(false);

    public async Task<SendPoiInformationResponse> Transceive(SendPoiInformationRequest command) =>
        await _PoiClient.Transceive(command).ConfigureAwait(false);

    public Task<GetDataBatchResponse> Transceive(GetDataBatchRequest command) => _DataBatchReader.Transceive(command);

    #endregion
}