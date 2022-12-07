using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IReadRecords : ITransceiveData<ReadRecordRequest, ReadRecordResponse>
{ }