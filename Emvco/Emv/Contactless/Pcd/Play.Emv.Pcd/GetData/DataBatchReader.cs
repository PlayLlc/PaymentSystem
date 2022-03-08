using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Play.Emv.DataElements.Emv;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.GetData;

public class DataBatchReader : IReadIccDataBatch
{
    #region Instance Values

    private readonly IReadIccData _DataReader;

    #endregion

    #region Constructor

    public DataBatchReader(IReadIccData dataReader)
    {
        _DataReader = dataReader;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<GetDataBatchResponse> Transceive(GetDataBatchRequest command)
    {
        TagsToRead? tagsToRead = command.GetTagsToRead();
        List<GetDataResponse> buffer = new();

        for (int i = 0; i < tagsToRead.Count(); i++)
        {
            if (!tagsToRead.TryDequeue(out Tag result))
            {
                throw new InvalidOperationException(
                    $"The {nameof(DataBatchReader)} was unable to extract the next {nameof(Tag)} from the {nameof(TagsToRead)}");
            }

            buffer.Add(await _DataReader.Transceive(GetDataRequest.Create(result, command.GetTransactionSessionId()))
                .ConfigureAwait(false));
        }

        return new GetDataBatchResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), buffer.ToArray());
    }

    #endregion
}