using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd;

public interface IReadApplicationData : ITransceiveDataBatches<ReadApplicationDataCommand, ReadApplicationDataResponse>
{ }