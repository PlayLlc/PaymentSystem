using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IReadElementaryFileRecords :
    ITransceiveDataBatches<ReadElementaryFileRecordRangeRequest, ReadElementaryFileRecordRangeResponse>
{ }