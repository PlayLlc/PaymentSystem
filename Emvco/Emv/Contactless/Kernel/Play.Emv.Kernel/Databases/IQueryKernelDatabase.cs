using System;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases.Tlv;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;

public interface IQueryKernelDatabase
{
    #region Instance Members

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public TagLengthValue Get(Tag tag);

    /// <summary>
    ///     Returns TRUE if tag T is defined in the data dictionary of the Kernel applicable for the Implementation Option
    /// </summary>
    /// <param name="tag"></param>
    public bool IsKnown(Tag tag);

    /// <summary>
    ///     Returns TRUE if the TLV Database includes a data object with tag T. Note that the length of the data object may be
    ///     zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool IsPresent(Tag tag);

    /// <summary>
    ///     Returns TRUE if:
    ///     • The Database includes a data object with the provided <see cref="Tag" />
    ///     • The length of the data object is non-zero
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool IsPresentAndNotEmpty(Tag tag);

    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex);
    public void PurgeRevokedCertificates();
    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result);

    /// <summary>
    ///     Returns true if a TLV object with the provided <see cref="Tag" /> exists in the database and the corresponding
    ///     <see cref="DatabaseValue" /> in an out parameter
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="result"></param>
    public bool TryGet(Tag tag, out TagLengthValue? result);

    /// <summary>
    ///     If the Kernel Database is active, true is returned and the KernelSessionId is set as the out parameter.
    ///     Otherwise false is returned
    /// </summary>
    public bool TryGetKernelSessionId(out KernelSessionId? result);

    /// <summary>
    ///     Updates the database with the
    ///     <param name="value"></param>
    ///     if it is a recognized object and discards the value if
    ///     it is not recognized
    /// </summary>
    /// <param name="value"></param>
    public void Update(TagLengthValue value);

    /// <summary>
    ///     Updates the the database with all recognized
    ///     <param name="values" />
    ///     provided it is not recognized
    /// </summary>
    /// <param name="values"></param>
    public void Update(TagLengthValue[] values);

    #endregion
}