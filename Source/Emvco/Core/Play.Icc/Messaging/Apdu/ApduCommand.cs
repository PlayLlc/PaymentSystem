using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Extensions;

namespace Play.Icc.Messaging.Apdu;

// TODO: Figure out a way to extract the Header and Body, and only have a byte[] value so it's easier to initialize derived types
public abstract class ApduCommand : IApduCommand
{
    #region Instance Values

    // private readonly byte[] _Value; <- This should be how this class works. Then grab header and body
    private readonly byte _Class;
    private readonly byte[]? _Data;
    private readonly byte _Instruction;
    private readonly uint? _Lc;
    private readonly uint? _Le;
    private readonly byte _Parameter1;
    private readonly byte _Parameter2;

    #endregion

    #region Constructor

    protected ApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2)
    {
        _Class = @class;
        _Instruction = instruction;
        _Parameter1 = parameter1;
        _Parameter2 = parameter2;
    }

    protected ApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le)
    {
        _Class = @class;
        _Instruction = instruction;
        _Parameter1 = parameter1;
        _Parameter2 = parameter2;
        _Le = le;
    }

    protected ApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data)
    {
        _Class = @class;
        _Instruction = instruction;
        _Parameter1 = parameter1;
        _Parameter2 = parameter2;
        _Lc = (uint) data.Length;
        _Data = data.ToArray();
    }

    protected ApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le)
    {
        _Class = @class;
        _Instruction = instruction;
        _Parameter1 = parameter1;
        _Parameter2 = parameter2;
        _Lc = (uint) data.Length;
        _Data = data.ToArray();
        _Le = le;
    }

    #endregion

    #region Instance Members

    public byte GetClass() => _Class;
    public byte[]? GetData() => _Data;
    public byte GetInstruction() => _Instruction;
    public uint? GetLc() => _Lc;
    public uint? GetLe() => _Le;
    public byte GetParameter1() => _Parameter1;
    public byte GetParameter2() => _Parameter2;

    // TODO: This was being weird
    private byte GetByteCount() => (byte) ((4 + _Data?.Length + _Lc?.GetMostSignificantByte()) ?? (0 + _Le?.GetMostSignificantByte()) ?? 0);

    public byte[] Encode()
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(GetByteCount());
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = _Class;
        buffer[1] = _Instruction;
        ;
        buffer[2] = _Parameter1;
        buffer[3] = _Parameter2;

        int offset = 4;

        if (_Lc != null)
        {
            BitConverter.GetBytes(_Lc!.Value).CopyTo(buffer[offset..]);
            offset += _Lc?.GetMostSignificantByte() ?? 0;
        }

        if (_Data != null)
        {
            _Data.CopyTo(buffer[offset..]);
            offset += _Data?.Length ?? 0;
        }

        if (_Le != null)
            BitConverter.GetBytes(_Le!.Value).CopyTo(buffer[offset..]);

        return buffer.ToArray();
    }

    #endregion
}