using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IPcdTransceiver
{
    #region Instance Members

    /// <summary>
    ///     CA(C-APDU) DataExchangeSignal. Send a C-APDU to the Card and return either an R-APDU or an error indication. The
    ///     parameter to the DataExchangeSignal
    ///     is the command to be sent to the Card
    /// </summary>
    /// <exception cref="PcdProtocolException"></exception>
    /// <exception cref="PcdTimeoutException"></exception>
    public Task<byte[]> Transceive(byte[] command);

    #endregion
}