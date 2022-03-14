using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv.Primitives.Card.Icccc;

namespace Play.Emv.Terminal.Common.Services._TempNew.ProcessingRestrictions
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

            if((ushort)versionNumberCard != (ushort)versionNumberTerminal)

        }

        public void ApplicationVersionCheck()
        { }

        #endregion
    }
}