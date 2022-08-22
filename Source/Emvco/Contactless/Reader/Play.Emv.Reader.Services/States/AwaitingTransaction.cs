namespace Play.Emv.Reader.Services.States;

public record AwaitingTransaction(ReaderDatabase ReaderDatabase) : MainState(ReaderDatabase)
{ }