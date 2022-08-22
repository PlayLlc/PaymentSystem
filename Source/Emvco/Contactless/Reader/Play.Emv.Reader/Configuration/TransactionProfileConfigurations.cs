using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber;
using Play.Emv.Identifiers;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Reader.Configuration
{
    internal class TransactionProfileConfigurations
    {
        #region Instance Values

        private readonly Dictionary<CombinationCompositeKey, TransactionProfile> _TransactionProfiles;
        private readonly List<TransactionType> _SupportedTransactionTypes;

        #endregion

        #region Constructor

        public TransactionProfileConfigurations(TransactionProfile[] values)
        {
            _TransactionProfiles = new Dictionary<CombinationCompositeKey, TransactionProfile>();
            List<CombinationCompositeKey> keys = values.Select(a => a.GetCombinationCompositeKey()).Distinct().ToList();

            foreach (var key in keys)
                _TransactionProfiles.Add(key, values.First(a => a.GetCombinationCompositeKey() == key));
            _SupportedTransactionTypes = values.Select(a => a.GetTransactionType()).Distinct().ToList();
        }

        #endregion

        #region Instance Members

        public TransactionProfile? GetTransactionProfile(CombinationCompositeKey value)
        {
            _TransactionProfiles.TryGetValue(value, out TransactionProfile? result);

            return result;
        }

        public TransactionType[] GetSupportedTransactionTypes() => _SupportedTransactionTypes.ToArray();
        public bool IsTransactionSupported(TransactionType value) => _SupportedTransactionTypes.Exists(a => a == value);

        #endregion
    }
}