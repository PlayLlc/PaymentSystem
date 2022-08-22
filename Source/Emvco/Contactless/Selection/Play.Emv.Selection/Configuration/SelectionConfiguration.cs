using Play.Emv.Ber.DataElements;
using Play.Emv.Reader.Configuration;
using Play.Emv.Selection.Contracts;

namespace Play.Emv.Selection.Configuration
{
    public class SelectionConfiguration
    {
        #region Instance Values

        public readonly TransactionProfiles TransactionProfiles;
        public readonly PoiInformation PoiInformation;

        #endregion

        #region Constructor

        public SelectionConfiguration(TransactionProfiles transactionProfiles, PoiInformation poiInformation)
        {
            TransactionProfiles = transactionProfiles;
            PoiInformation = poiInformation;
        }

        #endregion
    }
}