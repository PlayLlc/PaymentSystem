using System;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Outcomes;

namespace Play.Emv.Reader
{
    public partial class ReaderDatabase
    {
        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="TerminalException"></exception>
        public Outcome GetOutcome()
        {
            if (!IsActive())
            {
                throw new TerminalException(
                    new InvalidOperationException(
                        $"A command to initialize the Kernel Database was invoked but the {nameof(ReaderDatabase)} is already active"));
            }

            TryGet(DiscretionaryData.Tag, out DiscretionaryData? discretionaryData);
            TryGet(ErrorIndication.Tag, out ErrorIndication? errorIndication);
            TryGet(OutcomeParameterSet.Tag, out OutcomeParameterSet? outcomeParameterSet);
            TryGet(DataRecord.Tag, out DataRecord? dataRecord);
            TryGet(UserInterfaceRequestData.Tag, out UserInterfaceRequestData? userInterfaceRequestData);

            return new Outcome(errorIndication!, outcomeParameterSet!, userInterfaceRequestData, dataRecord, discretionaryData);
        }

        #endregion
    }
}