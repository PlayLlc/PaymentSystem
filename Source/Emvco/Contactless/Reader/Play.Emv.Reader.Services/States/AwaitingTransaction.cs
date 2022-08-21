namespace Play.Emv.Reader.Services.States;

public record AwaitingTransaction(ReaderConfiguration ReaderConfiguration) : MainState(ReaderConfiguration)
{ }