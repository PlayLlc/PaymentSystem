namespace Play.Emv.Kernel.Services;

public interface IManageTornTransactions : ICleanTornTransactions, IWriteTornTransactions, IReadTornTransactions
{ }