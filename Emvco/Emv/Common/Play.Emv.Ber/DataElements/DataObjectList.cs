using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     This object encapsulates Primitive <see cref="TagLength" /> Values requested by the ICC. These are ordered by the
///     expected concatenated response values
/// </summary>
public abstract record DataObjectList : DataElement<TagLength[]>
{
    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    protected DataObjectList(params TagLength[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     IsRequestedDataAvailable
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsRequestedDataAvailable(IReadTlvDatabase database)
    {
        foreach (TagLength item in _Value)
        {
            if (!database.IsKnown(item.GetTag()))
                return false;

            if (database.IsPresent(item.GetTag()))
                return false;
        }

        return true;
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public Tag[] GetNeededData(IReadTlvDatabase database)
    {
        List<Tag> result = new();

        foreach (TagLength item in _Value)
        {
            if (!database.IsPresentAndNotEmpty(item.GetTag()))
                result.Add(item.GetTag());
        }

        return result.ToArray();
    }

    /// <remarks>Book 3 Section 5.4</remarks>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public virtual DataObjectListResult AsDataObjectListResult(IReadTlvDatabase database)
    {
        TagLengthValue[] buffer = new TagLengthValue[_Value.Length];

        for (int i = 0; i < _Value.Length; i++)
            buffer[i] = ResolveDataObject(database, _Value[i]);

        return new DataObjectListResult(buffer);
    }

    /// <remarks>EMVco Book 3 Section 5.4</remarks>
    private static TagLengthValue ResolveDataObject(IReadTlvDatabase database, TagLength requestedDataObject)
    {
        if (!database.IsPresentAndNotEmpty(requestedDataObject.GetTag()))
            return new UnknownPrimitiveValue(requestedDataObject).AsTagLengthValue(_Codec);

        PrimitiveValue persistedValue = database.Get(requestedDataObject.GetTag());

        return new TagLengthValue(requestedDataObject.GetTag(), persistedValue.EncodeValue(_Codec, requestedDataObject.GetValueByteCount()));
    }

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public virtual CommandTemplate AsCommandTemplate(IReadTlvDatabase database) => AsDataObjectListResult(database).AsCommandTemplate();

    /// <summary>
    ///     Contains
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public bool Exists(Tag tag)
    {
        return _Value.Any(a => a.GetTag() == tag);
    }

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public new int GetValueByteCount()
    {
        return _Value.Sum(a => a.GetTagLengthByteCount());
    }

    public TagLength[] GetRequestedItems() => _Value;

    #endregion
}