using System.Reflection.Metadata.Ecma335;

using Play.Emv.Terminal.Session;

namespace MockPos.Dtos;

public class SequenceConfigurationDto
{
    #region Instance Values

    public uint Threshold { get; set; }
    public uint InitializationValue { get; set; }

    #endregion

    #region Serialization

    public SequenceConfiguration Decode() => new SequenceConfiguration(Threshold, InitializationValue);

    #endregion
}