using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IReadElementaryFileRecords : ITransceiveData<ReadElementaryFileRecordRequest, ReadElementaryFileRecordResponse>,
    ITransceiveDataBatches<ReadElementaryFileRecordRangeRequest, ReadElementaryFileRecordRangeResponse>
{ }