using Play.Emv.Interchange.DataFields;

namespace Play.Emv.Terminal.Common.Services.SequenceNumberManagement;

public interface IGenerateSequenceTraceAuditNumbers
{
    public SystemTraceAuditNumber Generate();

    //public void Reset(SettlementAcknowledgement settlementRequestAcknowledgement);
}