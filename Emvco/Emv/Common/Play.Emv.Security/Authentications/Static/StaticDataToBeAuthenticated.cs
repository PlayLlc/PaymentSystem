using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security.Exceptions;

namespace Play.Emv.Security.Authentications.Static;

public class StaticDataToBeAuthenticated
{
    #region Instance Values

    private readonly List<byte> _Value = new();

    #endregion

    #region Instance Members

    public int GetByteCount() => _Value.Count;

    /// <comments>
    ///     Only those records identified in the AFL as participating in offline data authentication are to be processed.
    ///     Records are processed in the same sequence in which they appear within AFL entries. The records identified by a
    ///     single AFL entry are to be processed in record number sequence. The first record begins the input for the
    ///     authentication process, and each succeeding record is concatenated at the end of the previous record.
    ///     The data from each record to be included in the offline data authentication input depends upon the SFI of the file
    ///     from which the record was read.
    ///     • For files with SFI in the range 1 to 10, the record tag ('70') and the record length are excluded from the
    ///     offline data authentication process. All other data in the data field of the response to the READ RECORD command
    ///     (excluding SW1 SW2) is included.
    ///     • For files with SFI in the range 11 to 30, the record tag('70') and the record length are not excluded from the
    ///     offline data authentication process.Thus all data in the data field of the response to the READ RECORD
    ///     command(excluding SW1 SW2) is included
    ///     If the records read for offline data authentication are not TLV-coded with tag equal to '70' then offline data
    ///     authentication shall be considered to have been performed and to have failed; that is, the terminal shall set the
    ///     ‘Offline data authentication was performed’ bit in the TSI to 1, and shall set the appropriate ‘SDA failed’ or ‘DDA
    ///     failed’ or ‘CDA failed’ bit in the TVR. The bytes of the record are included in the concatenation in the order in
    ///     which they appear in the command response.
    /// </comments>
    /// <param name="codec"></param>
    /// <param name="rapdu"></param>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <remarks>EMVco Book 3 Section 10.3</remarks>
    public void Enqueue(EmvCodec codec, ReadRecordResponse rapdu)
    {
        if (codec.DecodeTag(rapdu.GetData()) != ReadRecordResponseTemplate.Tag)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"The {nameof(StaticDataToBeAuthenticated)} encountered a {nameof(ReadRecordResponse)} with a {nameof(Tag)} that was not equal to {ReadRecordResponseTemplate.Tag}");
        }

        if (rapdu.GetShortFileId().IsUniversalFile())
        {
            _Value.AddRange(codec.GetContentOctets(rapdu.GetData()));

            return;
        }

        _Value.AddRange(rapdu.GetData());
    }

    /// <comments>
    ///     After all records identified by the AFL have been processed, the Static Data Authentication Tag List is processed,
    ///     if it exists. If the Static Data Authentication Tag List exists, it shall contain only the tag for the Application
    ///     Interchange Profile. The tag must represent the AIP available in the current application. The value field of the
    ///     AIP is to be concatenated to the current end of the input string. The tag and length of the AIP are not included in
    ///     the concatenation.
    /// </comments>
    /// <param name="tagList"></param>
    /// <param name="database"></param>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Enqueue(StaticDataAuthenticationTagList tagList, IQueryTlvDatabase database)
    {
        Tag[] requiredTags = tagList.GetRequiredTags();

        if (requiredTags.Length > 1)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"Static Data Authentication has failed because {nameof(StaticDataAuthenticationTagList)} contained an unexpected {nameof(Tag)}. Only {ApplicationInterchangeProfile.Tag} should be present");
        }

        if (requiredTags[0] != ApplicationInterchangeProfile.Tag)
        {
            throw new
                CryptographicAuthenticationMethodFailedException($"Static Data Authentication has failed because {nameof(StaticDataAuthenticationTagList)} contained an unexpected {nameof(Tag)}. Only {ApplicationInterchangeProfile.Tag} should be provided but the {nameof(Tag)} value: [{requiredTags[0]}] was present");
        }

        TagLengthValue aip = database.Get(ApplicationInterchangeProfile.Tag);

        _Value.AddRange(aip.EncodeValue());
    }

    public byte[] Encode() => _Value.ToArray();

    public void Encode(Span<byte> buffer, ref int offset)
    {
        _Value.ToArray().AsSpan().CopyTo(buffer[offset..]);
    }

    #endregion
}