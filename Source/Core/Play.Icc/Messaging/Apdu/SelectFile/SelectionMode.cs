using System.Collections.Generic;
using System.Collections.Immutable;

namespace Play.Icc.Messaging.Apdu.SelectFile;

public readonly struct SelectionMode
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, SelectionMode> _ValueObjectMap;

    /// <summary>
    ///     Select an Dedicated File that is a child of the current Dedicated File. Data field is file identifier
    /// </summary>
    /// <value>0x01</value>
    public static readonly SelectionMode DedicatedFileChild;

    /// <summary>
    ///     Select a Dedicated File by its Dedicated File Name
    /// </summary>
    /// <value>0x04</value>
    public static readonly SelectionMode DedicatedFileName;

    /// <summary>
    ///     Select a Dedicated File that is a parent of the current Dedicated File
    /// </summary>
    /// <value>0x03</value>
    public static readonly SelectionMode DedicatedFileParent;

    /// <summary>
    ///     Select an Elementary File that is a child of the current Dedicated File. Data field is file identifier
    /// </summary>
    /// <value>0x02</value>
    public static readonly SelectionMode ElementaryFileChild;

    /// <summary>
    ///     Select a Master File, Dedicated File, or Elementary File. Data field is a file identifier
    /// </summary>
    /// <value>0x00</value>
    public static readonly SelectionMode File;

    /// <summary>
    ///     Select a file by supplying the file path from the current Dedicated File. Dedicated File Identifier is not
    ///     included in the path
    /// </summary>
    /// <value>0x09</value>
    public static readonly SelectionMode PathFromDedicatedFile;

    /// <summary>
    ///     Select a file by supplying the file path from the Master File. Master File Identifier is not
    ///     included in the path
    /// </summary>
    /// <value>0x08</value>
    public static readonly SelectionMode PathFromMasterFile;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static SelectionMode()
    {
        const byte file = 0x00;
        const byte dedicatedFileChild = 0x01;
        const byte elementaryFileChild = 0x02;
        const byte dedicatedFileParent = 0x03;
        const byte dedicatedFileName = 0x04;
        const byte pathFromMasterFile = 0x08;
        const byte pathFromDedicatedFile = 0x09;

        File = new SelectionMode(file);
        DedicatedFileChild = new SelectionMode(dedicatedFileChild);
        ElementaryFileChild = new SelectionMode(elementaryFileChild);
        DedicatedFileParent = new SelectionMode(dedicatedFileParent);
        DedicatedFileName = new SelectionMode(dedicatedFileName);
        PathFromMasterFile = new SelectionMode(pathFromMasterFile);
        PathFromDedicatedFile = new SelectionMode(pathFromDedicatedFile);

        _ValueObjectMap = new Dictionary<byte, SelectionMode>
        {
            {file, File},
            {dedicatedFileChild, DedicatedFileChild},
            {elementaryFileChild, ElementaryFileChild},
            {dedicatedFileParent, DedicatedFileParent},
            {dedicatedFileName, DedicatedFileName},
            {pathFromMasterFile, PathFromMasterFile},
            {pathFromDedicatedFile, PathFromDedicatedFile}
        }.ToImmutableSortedDictionary();
    }

    private SelectionMode(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Equality

    //public static SelectionMode Get(byte value)
    //{
    //    const byte secureMessagingMask = (byte)(BitCount.Eight | BitCount.Seven | BitCount.Six | BitCount.Five);
    //    return _ValueObjectMap[value.GetMaskedValue(secureMessagingMask)];
    //}
    public override bool Equals(object? obj) => obj is SelectionMode secureMessaging && Equals(secureMessaging);
    public bool Equals(SelectionMode other) => _Value == other._Value;
    public bool Equals(SelectionMode x, SelectionMode y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 10544431;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(SelectionMode left, SelectionMode right) => left._Value == right._Value;
    public static bool operator ==(SelectionMode left, byte right) => left._Value == right;
    public static bool operator ==(byte left, SelectionMode right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(SelectionMode value) => (sbyte) value._Value;
    public static explicit operator short(SelectionMode value) => value._Value;
    public static explicit operator ushort(SelectionMode value) => value._Value;
    public static explicit operator int(SelectionMode value) => value._Value;
    public static explicit operator uint(SelectionMode value) => value._Value;
    public static explicit operator long(SelectionMode value) => value._Value;
    public static explicit operator ulong(SelectionMode value) => value._Value;
    public static implicit operator byte(SelectionMode value) => value._Value;
    public static bool operator !=(SelectionMode left, SelectionMode right) => !(left == right);
    public static bool operator !=(SelectionMode left, byte right) => !(left == right);
    public static bool operator !=(byte left, SelectionMode right) => !(left == right);

    #endregion
}