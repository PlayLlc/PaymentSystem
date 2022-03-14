using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services._TempNew
{
    public class CombinationCompatibilityValidator
    {
        #region Instance Members

        public void Process(ITlv database)
        {
            if (!database.IsPresentAndNotEmpty(ApplicationVersionNumberCard.Tag))
                return;

            ApplicationVersionNumberCard versionNumberCard =
                ApplicationVersionNumberCard.Decode(database.Get(ApplicationVersionNumberCard.Tag).EncodeValue().AsSpan());

            ApplicationVersionNumberTerminal versionNumberTerminal =
                ApplicationVersionNumberTerminal.Decode(database.Get(ApplicationVersionNumberTerminal.Tag).EncodeValue().AsSpan());

            if ((ushort) versionNumberCard != (ushort) versionNumberTerminal)
        }

        public void ApplicationVersionCheck()
        { }

        #endregion
    }
}