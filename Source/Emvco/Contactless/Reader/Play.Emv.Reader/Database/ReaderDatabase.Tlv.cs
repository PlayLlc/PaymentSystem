using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Messaging;

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

        public bool TryGetTransactionSessionId(out TransactionSessionId? result)
        {
            result = _TransactionSessionId;

            return result is not null;
        }

        /// <summary>
        ///     Updates the database with the
        ///     <param name="value"></param>
        ///     if it is a recognized object and discards the value if it is not recognized
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Update(PrimitiveValue value)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

            if (_TransactionValues.ContainsKey(value.GetTag()))
                _TransactionValues[value.GetTag()] = value;
            else
                _TransactionValues.Add(value.GetTag(), value);
        }

        /// <summary>
        ///     Updates the the database with all recognized
        ///     <param name="values" />
        ///     provided it is not recognized
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Update(PrimitiveValue[] values)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

            for (int i = 0; i < values.Length; i++)
                Update(values[i]);
        }

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
                throw new TerminalDataException($"The method {nameof(IsPresent)} cannot be accessed because {nameof(ReaderDatabase)} is not active");

            return _TransactionValues.ContainsKey(tag);
        }

        /// <summary>
        ///     Gets the <see cref="PrimitiveValue" /> associated with the <see cref="Tag" /> in the arg
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public T Get<T>(Tag tag) where T : PrimitiveValue
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(ReaderDatabase)} is not active");

            if (!_TransactionValues.TryGetValue(tag, out PrimitiveValue? result))
                throw new TerminalDataException($"The argument {nameof(tag)} provided does not exist in {nameof(ReaderDatabase)}");

            return (T) result!;
        }

        /// <summary>
        ///     Gets the <see cref="PrimitiveValue" /> associated with the <see cref="Tag" /> in the arg
        /// </summary>
        /// <param name="tag"></param>
        /// <exception cref="TerminalDataException">
        ///     This exception gets thrown internally because something was coded or incorrectly configured in our code base. An
        ///     assumption was made that the database value was present when it was not.
        /// </exception>
        /// <exception cref="TerminalDataException"></exception>
        public PrimitiveValue Get(Tag tag)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(ReaderDatabase)} is not active");

            if (!_TransactionValues.TryGetValue(tag, out PrimitiveValue? result))
                throw new TerminalDataException($"The argument {nameof(tag)} with the value: [{tag}] provided does not exist in {nameof(ReaderDatabase)}");

            return result!;
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
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

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
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(ReaderDatabase)} is not active");

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

        public bool IsActive() => _TransactionSessionId != null;
        public TransactionSessionId? GetKernelSessionId() => _TransactionSessionId;

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

            return new Outcome(errorIndication!, outcomeParameterSet!, dataRecord, discretionaryData, userInterfaceRequestData);
        }

        /// <exception cref="TerminalException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        public Transaction GetTransaction()
        {
            if (!IsActive())
            {
                throw new TerminalException(
                    new InvalidOperationException(
                        $"A command to initialize the Kernel Database was invoked but the {nameof(ReaderDatabase)} is already active"));
            }

            TryGet(AccountType.Tag, out AccountType? accountType);
            TryGet(AmountAuthorizedNumeric.Tag, out AmountAuthorizedNumeric? amountAuthorizedNumeric);
            TryGet(AmountOtherNumeric.Tag, out AmountOtherNumeric? amountOtherNumeric);
            TryGet(LanguagePreference.Tag, out LanguagePreference? languagePreference);
            TryGet(TerminalCountryCode.Tag, out TerminalCountryCode? terminalCountryCode);
            TryGet(TransactionDate.Tag, out TransactionDate? transactionDate);
            TryGet(TransactionTime.Tag, out TransactionTime? transactionTime);

            TryGet(TransactionType.Tag, out TransactionType? transactionType);
            TryGet(TransactionCurrencyCode.Tag, out TransactionCurrencyCode? transactionCurrencyCode);
            TryGet(TransactionCurrencyExponent.Tag, out TransactionCurrencyExponent? transactionCurrencyExponent);

            Outcome outcome = GetOutcome();

            return new Transaction(_TransactionSessionId!.Value, accountType!, amountAuthorizedNumeric!, amountOtherNumeric!, transactionType!,
                languagePreference!, terminalCountryCode!, transactionDate!, transactionTime!, transactionCurrencyExponent!, transactionCurrencyCode!, outcome);
        }

        #endregion
    }
}