using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Play.Emv.DataElements.Emv;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Security.Certificates;

/// <summary>
///     A composite key that uniquely identifies a certificate for an Issuer
/// </summary>
public class CaPublicKeyCertificateIdentifier : IEqualityComparer<CaPublicKeyCertificateIdentifier>,
    IEquatable<CaPublicKeyCertificateIdentifier>
{
    #region Instance Values

    private readonly CaPublicKeyIndex _Index;
    private readonly RegisteredApplicationProviderIndicator _RegisteredApplicationProviderIndicator;

    #endregion

    #region Constructor

    public CaPublicKeyCertificateIdentifier(
        CaPublicKeyIndex caPublicKeyIndex,
        RegisteredApplicationProviderIndicator registeredApplicationProviderIndicator)
    {
        _RegisteredApplicationProviderIndicator = registeredApplicationProviderIndicator;
        _Index = caPublicKeyIndex;
    }

    #endregion

    #region Instance Members

    public CaPublicKeyIndex GetCaPublicKeyIndex() => _Index;
    public RegisteredApplicationProviderIndicator GetRegisteredApplicationProviderIndicator() => _RegisteredApplicationProviderIndicator;

    #endregion

    #region Equality

    public bool Equals([AllowNull] CaPublicKeyCertificateIdentifier other)
    {
        if (other == null)
            return false;

        return (_RegisteredApplicationProviderIndicator == other._RegisteredApplicationProviderIndicator) && (_Index == other._Index);
    }

    public bool Equals([AllowNull] CaPublicKeyCertificateIdentifier x, [AllowNull] CaPublicKeyCertificateIdentifier y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals([AllowNull] object obj) =>
        obj is CaPublicKeyCertificateIdentifier caPublicKeyCertificateKey && Equals(caPublicKeyCertificateKey);

    public int GetHashCode(CaPublicKeyCertificateIdentifier obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 14407;

        return unchecked((hash * _RegisteredApplicationProviderIndicator.GetHashCode()) + (hash * _Index.GetHashCode()));
    }

    #endregion
}