using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Databases.Tlv;

namespace Play.Emv.Kernel.Databases
{
    public abstract partial class KernelDatabase
    {
        #region Instance Members

        /// <summary>
        ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
        ///     <see cref="DatabaseValue" /> in an out parameter
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <exception cref="TerminalDataException"></exception>
        public virtual bool TryGet(Tag tag, out TagLengthValue? result)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
            }

            return _TlvDatabase.TryGet(tag, out result);
        }

        /// <summary>
        /// </summary>
        /// <param name="tag"></param>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual TagLengthValue Get(Tag tag)
        {
            if (!IsActive())
                throw new
                    TerminalDataException($"The method {nameof(Get)} cannot be accessed because {nameof(KernelDatabase)} is not active");

            return _TlvDatabase.Get(tag);
        }

        /// <summary>
        ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
        /// </summary>
        /// <param name="tag"></param>
        public abstract bool IsKnown(Tag tag);

        /// <summary>
        ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
        ///     zero
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public virtual bool IsPresent(Tag tag)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(IsPresent)} cannot be accessed because {nameof(KernelDatabase)} is not active");
            }

            return _TlvDatabase.IsPresent(tag);
        }

        /// <summary>
        ///     Returns TRUE if:
        ///     • The Database includes a data object with the provided <see cref="Tag" />
        ///     • The length of the data object is non-zero
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        public virtual bool IsPresentAndNotEmpty(Tag tag)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(IsPresentAndNotEmpty)} cannot be accessed because {nameof(KernelDatabase)} is not active");
            }

            return _TlvDatabase.IsPresentAndNotEmpty(tag);
        }

        /// <summary>
        ///     Updates the database with the
        ///     <param name="value"></param>
        ///     if it is a recognized object and discards the value if
        ///     it is not recognized
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public virtual void Update(TagLengthValue value)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
            }

            _TlvDatabase.Update(value);
        }

        /// <summary>
        ///     Updates the the database with all recognized
        ///     <param name="values" />
        ///     provided it is not recognized
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="TerminalDataException"></exception>
        public virtual void Update(TagLengthValue[] values)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(Update)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
            }

            _TlvDatabase.UpdateRange(values);
        }

        /// <summary>
        ///     Initialize
        /// </summary>
        /// <param name="tag"></param>
        /// <exception cref="TerminalDataException"></exception>
        public virtual void Initialize(Tag tag)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(Initialize)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
            }

            _TlvDatabase.Update(new DatabaseValue(tag));
        }

        #endregion
    }
}