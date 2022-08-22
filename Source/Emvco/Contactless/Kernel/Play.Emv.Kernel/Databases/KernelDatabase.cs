using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Core;
using Play.Emv.Ber;
using Play.Emv.Ber.Database._Temp;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase : TlvDatabase
{
    #region Constructor

    public KernelDatabase(
        CertificateAuthorityDataset[] certificateAuthorityDataset, PrimitiveValue[] kernelPersistentConfiguration, KnownObjects knownObjects,
        ScratchPad scratchPad) : base(kernelPersistentConfiguration, knownObjects)
    {
        _ScratchPad = scratchPad;
        _Certificates = certificateAuthorityDataset.ToImmutableDictionary(a => a.GetRid(), b => b);
        FailedMagstripeCounter = new SequenceCounterThreshold(0, int.MaxValue, 1);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Resets the transient values in the database to their default values. The persistent values
    ///     will remain unchanged during the database lifetime
    /// </summary>
    public override void Deactivate()
    {
        Clear();
        PurgeRevokedCertificates();
    }

    #endregion
}