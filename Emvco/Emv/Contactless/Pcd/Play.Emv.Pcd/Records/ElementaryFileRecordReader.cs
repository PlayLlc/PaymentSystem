using System.Collections.Generic;
using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public class ElementaryFileRecordReader : IReadElementaryFileRecords
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public ElementaryFileRecordReader(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    public async Task<ReadElementaryFileRecordRangeResponse> Transceive(ReadElementaryFileRecordRangeRequest command)
    {
        List<ReadElementaryFileRecordResponse> buffer = new();

        foreach (RecordNumber recordNumber in command.GetRecordRange().GetRecords())
        {
            buffer.Add(await Transceive(ReadElementaryFileRecordRequest.Create(command.GetTransactionSessionId(), command.GetShortFileId(),
                recordNumber)).ConfigureAwait(false));
        }

        return new ReadElementaryFileRecordRangeResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), buffer.ToArray());
    }

    public async Task<ReadElementaryFileRecordResponse> Transceive(ReadElementaryFileRecordRequest command)
    {
        ReadRecordRApduSignal response = new(await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false));

        return new ReadElementaryFileRecordResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
    }

    #endregion
}