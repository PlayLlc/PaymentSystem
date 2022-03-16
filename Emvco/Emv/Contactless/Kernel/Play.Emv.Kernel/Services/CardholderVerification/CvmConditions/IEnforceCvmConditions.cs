using Play.Emv.Ber.DataObjects;

namespace Play.Emv.Kernel.Services.Conditions.CvmConditions;

public interface IEnforceCvmConditions
{
    public bool IsConditionSatisfied(CvmConditionCode code, IQueryTlvDatabase database);
}