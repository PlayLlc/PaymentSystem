using System;
using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Exceptions;
using Play.Emv.Pcd.GetData;

namespace Play.Emv.Pcd.Services;

// BUG: Need to update the try-catch logic for all of these methods. We need to make sure we're logging the relevant information. We should also catch more specific exceptions so we can intercept and throw the most appropriate exception type

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

    /// <summary>
    /// Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CardReadException"></exception>
    public async Task<GetProcessingOptionsResponse> Transceive(GetProcessingOptionsRequest command)
    {
        // TODO: catch more specific exceptions
        try
        {
            return await _GpoClient.Transceive(command).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

 
    /// <summary>
    /// Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CardReadException"></exception>
    public async Task<SelectProximityPaymentSystemEnvironmentResponse> Transceive(SelectProximityPaymentSystemEnvironmentRequest command)
    {
        // TODO: catch more specific exceptions
        try
        {
            return await _PpseClient.Transceive(command).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// Abort
    /// </summary>
    /// <exception cref="CardReadException"></exception>
    public void Abort()
    {
        // TODO: catch more specific exceptions
        try
        {
            _PcdClient.Abort();
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// Activate
    /// </summary>
    /// <exception cref="CardReadException"></exception>
    public void Activate()
    {
        // TODO: catch more specific exceptions
        try
        {
            _PcdClient.Activate();
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// CloseSession
    /// </summary>
    /// <exception cref="CardReadException"></exception>
    public void CloseSession()
    {
        // TODO: catch more specific exceptions
        try
        {
            _PcdClient.CloseSession();
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// CloseSessionCardCheck
    /// </summary>
    /// <exception cref="CardReadException"></exception>
    public void CloseSessionCardCheck()
    {
        _PcdClient.CloseSessionCardCheck();

        // TODO: catch more specific exceptions
        try
        {
            _PcdClient.CloseSession();
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CardReadException"></exception>
    public async Task<ReadApplicationDataResponse> Transceive(ReadApplicationDataRequest command)
    {
        // TODO: catch more specific exceptions
        try
        {
            return await _ApplicationDataClient.Transceive(command).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CardReadException"></exception>
    public async Task<SelectApplicationDefinitionFileInfoResponse> Transceive(SelectApplicationDefinitionFileInfoRequest command)
    {
        // TODO: catch more specific exceptions
        try
        {
            return await _AppletClient.Transceive(command).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CardReadException"></exception>
    public async Task<SelectDirectoryDefinitionFileResponse> Transceive(SelectDirectoryDefinitionFileRequest command)
    {
        // TODO: catch more specific exceptions
        try
        {
            return await _DirectoryFciClient.Transceive(command).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CardReadException"></exception>
    public async Task<SendPoiInformationResponse> Transceive(SendPoiInformationRequest command)
    {
        // TODO: catch more specific exceptions
        try
        {
            return await _PoiClient.Transceive(command).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    /// <summary>
    /// Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CardReadException"></exception>
    public Task<GetDataBatchResponse> Transceive(GetDataBatchRequest command)
    {
        // TODO: catch more specific exceptions
        try
        {
            return _DataBatchReader.Transceive(command);
        }
        catch (Exception exception)
        {
            // TODO: log and shit

            throw new CardReadException(exception);
        }
    }

    #endregion
}