﻿using Play.Ber.Emv.DataObjects;
using Play.Icc.Messaging.Apdu;

namespace Play.Icc.Emv.GetProcessingOptions;

public class GetProcessingOptionsCApduSignal : CApduSignal
{
    #region Constructor

    public GetProcessingOptionsCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction,
                                                                                                                   parameter1, parameter2)
    { }

    public GetProcessingOptionsCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) :
        base(@class, instruction, parameter1, parameter2, le)
    { }

    public GetProcessingOptionsCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    public GetProcessingOptionsCApduSignal(
        byte @class,
        byte instruction,
        byte parameter1,
        byte parameter2,
        ReadOnlySpan<byte> data,
        uint? le) : base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static GetProcessingOptionsCApduSignal Create()
    {
        return new GetProcessingOptionsCApduSignal(new Class(ProprietaryMessageIdentifier._8x), Instruction.GetProcessingOptions, 0, 0,
                                                   new byte[] {0x83, 0});
    }

    /// <param name="pdolResult">
    ///     A list of objects requested by the ICC in a Processing Options Data Object List
    /// </param>
    public static GetProcessingOptionsCApduSignal Create(DataObjectListResult pdolResult)
    {
        return new(new Class(ProprietaryMessageIdentifier._8x), Instruction.GetProcessingOptions, 0, 0,
                   pdolResult.AsCommandTemplate().EncodeTagLengthValue());
    }

    /// <param name="commandTemplate">
    ///     A template created from a list of objects requested by the ICC in a Processing Options Data Object List
    /// </param>
    public static GetProcessingOptionsCApduSignal Create(CommandTemplate commandTemplate)
    {
        return new(new Class(ProprietaryMessageIdentifier._8x), Instruction.GetProcessingOptions, 0, 0,
                   commandTemplate.EncodeTagLengthValue());
    }

    #endregion
}