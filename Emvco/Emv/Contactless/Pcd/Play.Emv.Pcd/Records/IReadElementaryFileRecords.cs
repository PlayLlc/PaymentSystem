using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IReadElementaryFileRecords : ITransceiveData<ReadElementaryFileRecordCommand, ReadElementaryFileRecordResponse>,
    ITransceiveDataBatches<ReadElementaryFileRecordRangeCommand, ReadElementaryFileRecordRangeResponse>
{ }