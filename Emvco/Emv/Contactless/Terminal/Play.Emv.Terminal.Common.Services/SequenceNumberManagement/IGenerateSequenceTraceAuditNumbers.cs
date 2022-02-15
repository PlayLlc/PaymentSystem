namespace Play.Emv.Terminal.Common.Services.SequenceNumberManagement;

public interface IGenerateSequenceTraceAuditNumbers
{
    public ushort Generate();

    //public void Reset(SettlementAcknowledgement settlementRequestAcknowledgement);
}