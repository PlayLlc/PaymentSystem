using System;

using Play.Ber.InternalFactories;

namespace Play.Ber.Codecs;

/// <summary>
///     A simple codec base class that can be inherited to customize an encoding class specific to the encoding rules of
///     the implementing class
/// </summary>
public interface IPlayCodec
{
    #region Instance Members

    public ushort GetByteCount<T>(T value) where T : struct;
    public ushort GetByteCount<T>(T[] value) where T : struct;

    /// <summary>
    ///     This is for external validation of a sequence and will not throw an exception
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsValid(ReadOnlySpan<byte> value);

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<T>(T value) where T : struct;

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content to be encoded
    /// </param>
    /// <param name="length">
    ///     The length in bytes that the encoded result will return. If the length is smaller than the value provided
    ///     then the result will be truncated. If the length is larger then the result will be padded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<T>(T value, int length) where T : struct;

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<T>(T[] value) where T : struct;

    /// <summary>
    ///     Encodes the Value content according to this implementation's encoding rules
    /// </summary>
    /// <typeparam name="T">
    ///     The decoded Value content that is capable of being encoded by this implementation
    /// </typeparam>
    /// <param name="value">
    ///     The Value content sequence to be encoded
    /// </param>
    /// <param name="length">
    ///     The length in bytes that the encoded result will return. If the length is smaller than the value provided
    ///     then the result will be truncated. If the length is larger then the result will be padded
    /// </param>
    /// <returns>
    ///     The raw encoded bytes of the value provided
    /// </returns>
    public byte[] Encode<T>(T[] value, int length) where T : struct;

    public void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct;
    public void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct;
    public void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct;
    public void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct;

    #endregion

    #region Serialization

    public DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}