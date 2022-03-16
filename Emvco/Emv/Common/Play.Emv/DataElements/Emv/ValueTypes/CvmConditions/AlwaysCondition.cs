using System;

using Play.Ber.Identifiers;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Kernel.Services.Conditions;

namespace Play.Emv.DataElements.CvmConditions
{
    internal record AlwaysCondition : CvmCondition
    {
        #region Static Metadata

        public static readonly CvmConditionCode Code = new(0);

        #endregion

        #region Instance Values

        protected override Tag[] RequiredData => throw new NotImplementedException();

        #endregion

        #region Instance Members

        public override CvmConditionCode GetConditionCode() => Code;
        protected override bool IsConditionSatisfied(IQueryTlvDatabase database) => throw new NotImplementedException();

        #endregion
    }
}