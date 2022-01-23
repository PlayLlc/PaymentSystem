﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Contracts;

namespace Play.Emv.Security.Encryption;

internal class HashAlgorithmProvider : IHashAlgorithmProvider
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<HashAlgorithmIndicator, IHashGenerator> _HashAlgorithmMap;

    #endregion

    #region Constructor

    public HashAlgorithmProvider()
    {
        _HashAlgorithmMap = CreateHashAlgorithmMap();
    }

    #endregion

    #region Instance Members

    private ImmutableSortedDictionary<HashAlgorithmIndicator, IHashGenerator> CreateHashAlgorithmMap() =>
        new Dictionary<HashAlgorithmIndicator, IHashGenerator> {{HashAlgorithmIndicator.Sha1, new Sha1HashGenerator()}}
            .ToImmutableSortedDictionary();

    public Hash Generate(ReadOnlySpan<byte> clearText, HashAlgorithmIndicator hashAlgorithmIndicator)
    {
        if (!_HashAlgorithmMap.TryGetValue(hashAlgorithmIndicator, out IHashGenerator? hashGenerator))
        {
            throw new ArgumentOutOfRangeException(nameof(hashAlgorithmIndicator),
                $"There was no {nameof(IHashGenerator)} available for the argument {nameof(hashAlgorithmIndicator)} with value {hashAlgorithmIndicator}");
        }

        return hashGenerator.Generate(clearText);
    }

    #endregion
}