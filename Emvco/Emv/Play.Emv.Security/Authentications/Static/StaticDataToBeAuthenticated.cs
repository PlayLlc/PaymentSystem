using System;
using System.Linq;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;

namespace Play.Emv.DataElements;

public class StaticDataToBeAuthenticated
{
    #region Instance Values

    /// <summary>
    ///     The records read from the ApplicationFileLocator that were returned by the card with a 0x70 template tag
    /// </summary>
    private readonly TagLengthValue[] _ApplicationFileLocatorResults;

    private readonly ApplicationInterchangeProfile? _ApplicationInterchangeProfile;
    private readonly StaticDataAuthenticationTagList? _StaticDataAuthenticationTagList;

    #endregion

    #region Constructor

    public StaticDataToBeAuthenticated(TagLengthValue[] applicationFileLocatorResults)
    {
        _ApplicationFileLocatorResults = applicationFileLocatorResults;
    }

    public StaticDataToBeAuthenticated(
        TagLengthValue[] applicationFileLocatorResults, // TODO: this is weak
        StaticDataAuthenticationTagList staticDataAuthenticationTagList,
        ApplicationInterchangeProfile applicationInterchangeProfile)
    {
        _ApplicationFileLocatorResults = applicationFileLocatorResults;
        _StaticDataAuthenticationTagList = staticDataAuthenticationTagList;
        _ApplicationInterchangeProfile = applicationInterchangeProfile;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Input to the authentication process is formed from the records identified by the ApplicationFileLocator, followed
    ///     by the
    ///     data elements identified by the optional StaticDataAuthenticationTagList (tag '9F4A'). If the
    ///     StaticDataAuthenticationTagList
    ///     is present, it shall contain only the value of the ApplicationInterchangeProfile. The value field of the
    ///     ApplicationInterchangeProfile is to be concatenated to the end of the list, without the Tag and Length included.
    /// </summary>
    /// <remarks>
    ///     Book 3 Section 10.3
    /// </remarks>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public byte[] AsByteArray()
    {
        // TODO: According to BOOK 3 Section 10.3 - might need to iterate through each result and ensure that the
        // TODO: Record Result Template Format that returned is of tag 0x70. We would also need to strip some of the
        // TODO: primitives down to the TLV encoded Value field if the SFI is 11-30. We can do that by passing a
        // TODO: RecordResult Factory - or can take care of this at a higher level process

        using SpanOwner<byte> spanOwner =
            SpanOwner<byte>.Allocate((int) (_ApplicationFileLocatorResults.Sum(a => a.GetTagLengthValueByteCount())
                                         + (_StaticDataAuthenticationTagList == null
                                             ? 0
                                             : _ApplicationInterchangeProfile!.GetValueByteCount())));
        Span<byte> buffer = spanOwner.Span;

        _ApplicationFileLocatorResults.SelectMany(a => a.EncodeTagLengthValue()).ToArray().AsSpan().CopyTo(buffer);

        if (_StaticDataAuthenticationTagList is not null)
            _ApplicationInterchangeProfile?.Encode().AsSpan().CopyTo(buffer[^2..]);

        return buffer.ToArray();
    }

    public int GetByteCount()
    {
        return (int) (_ApplicationFileLocatorResults.Sum(a => a.GetTagLengthValueByteCount())
            + (_StaticDataAuthenticationTagList == null ? 0 : _ApplicationInterchangeProfile!.GetValueByteCount()));
    }

    public bool IsValid()
    {
        if (_StaticDataAuthenticationTagList is null)
            return true;

        // TODO: check if _StaticDataAuthenticationTagList && contains tags other that 0x82
        throw new NotImplementedException();
    }

    #endregion
}