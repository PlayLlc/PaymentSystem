using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases.Certificates;
using Play.Emv.Terminal.Contracts;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase : IManageKernelDatabaseLifetime
{
    #region Instance Values

    protected KernelSessionId? _KernelSessionId;
    protected OutcomeParameterSet.Builder _OutcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
    protected UserInterfaceRequestData.Builder _UserInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();
    protected ErrorIndication.Builder _ErrorIndicationBuilder = ErrorIndication.GetBuilder();
    protected TerminalVerificationResults.Builder _TerminalVerificationResultBuilder = TerminalVerificationResults.GetBuilder();
    protected TerminalCapabilities.Builder _TerminalCapabilitiesBuilder = TerminalCapabilities.GetBuilder();

    #endregion

    #region Constructor

    public KernelDatabase(
        CertificateAuthorityDataset[] certificateAuthorityDataset, PersistentValues persistentValues, KnownObjects knownObjects)
    {
        _Certificates = certificateAuthorityDataset.ToImmutableSortedDictionary(a => a.GetRid(), b => b);
        _PersistentValues = persistentValues;
        _KnownObjects = knownObjects;
        _Database = new SortedDictionary<Tag, PrimitiveValue?>();
        SeedDatabase();
    }

    #endregion

    #region Instance Members

    /// <exception cref="TerminalDataException"></exception>
    public KernelConfiguration GetKernelConfiguration() => Get<KernelConfiguration>(KernelConfiguration.Tag);

    /// <summary>
    ///     Activate
    /// </summary>
    /// <param name="kernelSessionId"></param>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public virtual void Activate(KernelSessionId kernelSessionId)
    {
        if (IsActive())
        {
            throw new
                InvalidOperationException($"A command to initialize the Kernel Database was invoked but the {nameof(KernelDatabase)} is already active");
        }

        _KernelSessionId = kernelSessionId;
    }

    /// <summary>
    ///     Resets the transient values in the database to their default values. The persistent values
    ///     will remain unchanged during the database lifetime
    /// </summary>
    public virtual void Deactivate()
    {
        Clear();
        PurgeRevokedCertificates();
    }

    protected bool IsActive() => _KernelSessionId != null;

    #endregion
}