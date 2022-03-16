using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Services.Conditions;

namespace Play.Emv.Kernel.Services.CardholderVerification
{
    internal class CvmSelector
    {
        #region Instance Values

        private readonly List<CardholderVerificationRule> _Rules;
        private readonly CvmList _CvmList;
        private byte _Offset = 0;

        #endregion

        #region Instance Members

        public CvmSelector(List<CardholderVerificationRule> rules, CvmList cvmList)
        {
            _Rules = rules;
            _CvmList = cvmList;
        }

        /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
        public bool TrySelect(IQueryTlvDatabase database, out CardholderVerificationRule? rule)
        {
            if (_Rules.Count < (_Offset - 1))
            {
                rule = null;

                return false;
            }

            var a = _CvmList.GetXAmount()

            for()

            for (int i = _Offset; i < _Rules.Count; i++)
            {
                if (_Rules[i].IsSupported(database))
                {
                    rule = _Rules[i];

                    return true;
                }
            }

            rule = null;

            return false;
        }

        #endregion
    }
}