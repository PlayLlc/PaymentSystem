using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;

namespace Play.Emv.Reader.Database
{
    public interface IReaderTransactionDatabase
    {
        #region Instance Members

        /// <summary>
        ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
        ///     zero
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public bool IsPresent(Tag tag);

        /// <summary>
        ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
        ///     <see cref="PrimitiveValue" /> in an out parameter
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <exception cref="TerminalDataException"></exception>
        public bool TryGet(Tag tag, out PrimitiveValue? result);

        /// <summary>
        ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
        ///     <see cref="PrimitiveValue" /> in an out parameter
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <exception cref="TerminalDataException"></exception>
        bool TryGet<T>(Tag tag, out T? result) where T : PrimitiveValue;

        #endregion
    }

    public partial class ReaderDatabase : IReaderTransactionDatabase
    {
        #region Instance Values

        private readonly Dictionary<Tag, PrimitiveValue> _TransactionValues;
        private TransactionSessionId? _TransactionSessionId;

        #endregion

        #region Instance Members

        /// <summary>
        ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
        ///     zero
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public bool IsPresent(Tag tag)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(IsPresent)} cannot be accessed because {nameof(KernelDatabase)} is not active");

            return _TransactionValues.ContainsKey(tag);
        }

        /// <summary>
        ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
        ///     <see cref="PrimitiveValue" /> in an out parameter
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <exception cref="TerminalDataException"></exception>
        public bool TryGet(Tag tag, out PrimitiveValue? result)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");

            if (!_TransactionValues.TryGetValue(tag, out PrimitiveValue? databaseValue))
            {
                result = null;

                return false;
            }

            result = databaseValue;

            return true;
        }

        /// <summary>
        ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
        ///     <see cref="PrimitiveValue" /> in an out parameter
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <exception cref="TerminalDataException"></exception>
        public bool TryGet<T>(Tag tag, out T? result) where T : PrimitiveValue
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");

            if (!_TransactionValues.TryGetValue(tag, out PrimitiveValue? databaseValue))
            {
                result = null;

                return false;
            }

            result = (T) databaseValue!;

            return true;
        }

        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="TerminalException"></exception>
        public virtual void Activate(TransactionSessionId kernelSessionId)
        {
            if (IsActive())
            {
                throw new TerminalException(
                    new InvalidOperationException(
                        $"A command to initialize the Kernel Database was invoked but the {nameof(ReaderDatabase)} is already active"));
            }

            _TransactionSessionId = kernelSessionId;
            Seed();
        }

        private void Seed()
        {
            foreach (PrimitiveValue value in _ReaderConfiguration)
                _TransactionValues.Add(value.GetTag(), value);
        }

        /// <summary>
        ///     Resets the transient values in the database to their default values. The persistent values
        ///     will remain unchanged during the database lifetime
        /// </summary>
        public virtual void Deactivate()
        {
            _TransactionValues.Clear();
        }

        protected bool IsActive() => _TransactionSessionId != null;
        public TransactionSessionId? GetKernelSessionId() => _TransactionSessionId;

        #endregion
    }
}