using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Icc.Exceptions;

namespace Play.Icc.Messaging.Apdu.GetData;

/// <summary>
///     The GET DATA command is used to retrieve a primitive data object not encapsulated in a record within the current
///     application.
/// </summary>
public class GetDataApduCommand : ApduCommand
{
    #region Constructor

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    protected GetDataApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint le) : base(
        @class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Create
    /// </summary>
    /// <param name="proprietaryMessageIdentifier"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="StatusBytesException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static GetDataApduCommand Create(ProprietaryMessageIdentifier proprietaryMessageIdentifier, Tag tag)
    {
        if (tag.GetByteCount() > 2)
        {
            throw new IccProtocolException(
                $"The {nameof(ApduCommand)} could not be generated because the argument {nameof(Tag)} exceeded the maximum byte count of 2");
        }

        if (tag.GetByteCount() == 2)
        {
            if (!tag.IsPrimitive())
            {
                throw new IccProtocolException(
                    $"The {nameof(ApduCommand)} could not be generated because the argument {nameof(Tag)} had a length of 2 bytes but did not have a {nameof(DataObjectType)} of {nameof(DataObjectType.Primitive)}");
            }

            Span<byte> buffer = stackalloc byte[2];
            tag.Serialize().CopyTo(buffer);

            return new GetDataApduCommand(new Class(proprietaryMessageIdentifier), Instruction.GetData, buffer[0], buffer[1]);
        }

        if (tag.IsPrimitive())
            return new GetDataApduCommand(new Class(proprietaryMessageIdentifier), Instruction.GetData, 0, tag.Serialize()[0]);

        if (tag.IsApplicationSpecific())
            return new GetDataApduCommand(new Class(proprietaryMessageIdentifier), Instruction.GetData, 1, tag.Serialize()[0]);

        if (tag.IsConstructed())
            return new GetDataApduCommand(new Class(proprietaryMessageIdentifier), Instruction.GetData, 2, tag.Serialize()[0]);

        throw new StatusBytesException(
            $"The {nameof(ApduCommand)} could not be generated because the argument {nameof(Tag)} format wasn't recognized for a {nameof(GetDataApduCommand)}");
    }

    #endregion
}