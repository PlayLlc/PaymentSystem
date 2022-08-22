using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Database._Temp
{
    public abstract partial class TlvDatabase : ITlvReaderAndWriter
    {
        #region Instance Values

        private readonly SortedDictionary<Tag, PrimitiveValue?> _Database;
        private readonly PrimitiveValue[] _PersistentConfiguration;
        private readonly KnownObjects _KnownObjects;

        #endregion

        #region Lifetime Management

        public void Clear()
        {
            foreach (KeyValuePair<Tag, PrimitiveValue?> a in _Database)
                _Database[a.Key] = null;

            SeedDatabase();
        }

        protected virtual void SeedDatabase()
        {
            foreach (PrimitiveValue persistentValue in _PersistentConfiguration)
                _Database.Add(persistentValue.GetTag(), persistentValue);
        }

        #endregion

        #region Read

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
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(TlvDatabase)} is not active");

            if (!_Database.TryGetValue(tag, out PrimitiveValue? databaseValue))
            {
                result = null;

                return false;
            }

            result = databaseValue;

            return true;
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
                throw new TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(TlvDatabase)} is not active");

            if (!_Database.TryGetValue(tag, out PrimitiveValue? result))
                throw new TerminalDataException($"The argument {nameof(tag)} with the value: [{tag}] provided does not exist in {nameof(TlvDatabase)}");

            return result!;
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
                throw new TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(TlvDatabase)} is not active");

            if (!_Database.TryGetValue(tag, out PrimitiveValue? databaseValue))
            {
                result = null;

                return false;
            }

            result = (T) databaseValue!;

            return true;
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
                throw new TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(TlvDatabase)} is not active");

            if (!_Database.TryGetValue(tag, out PrimitiveValue? result))
                throw new TerminalDataException($"The argument {nameof(tag)} provided does not exist in {nameof(TlvDatabase)}");

            return (T) result!;
        }

        /// <summary>
        ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
        /// </summary>
        /// <param name="tag"></param>
        public bool IsKnown(Tag tag) => _KnownObjects.Exists(tag);

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
                throw new TerminalDataException($"The method {nameof(IsPresent)} cannot be accessed because {nameof(TlvDatabase)} is not active");

            return _Database.ContainsKey(tag);
        }

        /// <summary>
        ///     Returns TRUE if:
        ///     • The Database includes a data object with the provided <see cref="Tag" />
        ///     • The length of the data object is non-zero
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public bool IsPresentAndNotEmpty(Tag tag)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(IsPresentAndNotEmpty)} cannot be accessed because {nameof(TlvDatabase)} is not active");

            return IsPresent(tag) && (_Database[tag] != null);
        }

        #endregion

        #region Write

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
                throw new TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(TlvDatabase)} is not active");

            if (!IsKnown(value.GetTag()))
                return;

            if (_Database.ContainsKey(value.GetTag()))
                _Database[value.GetTag()] = value;
            else
                _Database.Add(value.GetTag(), value);
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
                throw new TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(TlvDatabase)} is not active");

            for (int i = 0; i < values.Length; i++)
                Update(values[i]);
        }

        /// <summary>
        ///     Initialize
        /// </summary>
        /// <param name="tag"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Initialize(Tag tag)
        {
            if (!IsActive())
                throw new TerminalDataException($"The method {nameof(Initialize)} cannot be accessed because the {nameof(TlvDatabase)} is not active");

            if (!IsKnown(tag))
                return;

            _Database.Add(tag, null);
        }

        /// <summary>
        ///     Initialize
        /// </summary>
        /// <param name="tag"></param>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="OverflowException"></exception>
        public void Initialize(params Tag[] tag)
        {
            for (int i = 0; i < tag.Length; i++)
            {
                if (!IsKnown(tag[i]))
                    continue;

                Initialize(tag[i]);
            }
        }

        #endregion
    }
}