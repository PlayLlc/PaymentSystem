using System.Collections.Generic;
using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Emv.Pcd;

public class ApplicationDataReader : IReadApplicationData
{
    #region Instance Values

    private readonly IReadElementaryFileRecords _RecordRangeReader;

    #endregion

    #region Constructor

    public ApplicationDataReader(IReadElementaryFileRecords recordRangeRangeReader)
    {
        _RecordRangeReader = recordRangeRangeReader;
    }

    #endregion

    #region Instance Members

    public async Task<ReadApplicationDataResponse> Transceive(ReadApplicationDataCommand command)
    {
        List<ReadElementaryFileRecordRangeResponse> buffer = new();

        foreach (RecordRange range in command.GetRecordRanges())
        {
            buffer.Add(await _RecordRangeReader
                           .Transceive(ReadElementaryFileRecordRangeCommand.Create(command.GetTransactionSessionId(), range))
                           .ConfigureAwait(false));
        }

        return new ReadApplicationDataResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), buffer.ToArray());
    }

    #endregion
}