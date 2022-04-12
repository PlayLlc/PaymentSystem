using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.Services;

namespace Play.Emv.Kernel.Databases
{
    public partial class KernelDatabase
    {
        #region Instance Values

        private readonly ScratchPad _ScratchPad;

        #endregion

        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        public void Add(TornRecord tornRecord) => _ScratchPad.Add(this, Get<ApplicationIdentifier>(ApplicationIdentifier.Tag), tornRecord);

        /// <exception cref="TerminalDataException"></exception>
        public bool TryGet(TornEntry tornEntry, out TornRecord? result) =>
            _ScratchPad.TryGet(Get<ApplicationIdentifier>(ApplicationIdentifier.Tag), tornEntry, out result);

        #endregion
    }
}