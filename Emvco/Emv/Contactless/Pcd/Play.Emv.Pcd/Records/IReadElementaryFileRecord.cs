using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IReadElementaryFileRecord : ITransceiveData<ReadElementaryFileRecordRequest, ReadElementaryFileRecordResponse>
{ }