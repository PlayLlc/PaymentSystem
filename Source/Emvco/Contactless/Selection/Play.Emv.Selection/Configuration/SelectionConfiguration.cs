using Play.Emv.Ber.DataElements;

namespace Play.Emv.Selection.Configuration
{
    public class SelectionConfiguration
    {
        #region Instance Values

        public readonly TransactionProfile[] TransactionProfiles;
        public readonly PoiInformation PoiInformation;

        #endregion

        #region Constructor

        public SelectionConfiguration(TransactionProfile[] transactionProfiles, PoiInformation poiInformation)
        {
            TransactionProfiles = transactionProfiles;
            PoiInformation = poiInformation;
        }

        #endregion
    }
}