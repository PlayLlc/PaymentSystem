﻿using System.Threading.Tasks;

using Play.Emv.Icc.ReadRecord;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public class RecordReader : IReadRecords
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public RecordReader(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    public async Task<ReadRecordResponse> Transceive(ReadRecordRequest command)
    {
        ReadRecordRApduSignal response = new(await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false),
            command.GetShortFileId());

        // TODO Handle for Status  Words

        return new ReadRecordResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response, command.GetShortFileId());
    }

    #endregion
}