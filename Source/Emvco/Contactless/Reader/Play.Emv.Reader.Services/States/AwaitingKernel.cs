using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Reader.Services.States;

public record AwaitingKernel(
    KernelSessionId KernelSessionId, TransactionSessionId TransactionSessionId, CorrelationId CorrelationId,
    ReaderConfiguration ReaderConfiguration) : AwaitingSelection(TransactionSessionId, CorrelationId, ReaderConfiguration)
{ }