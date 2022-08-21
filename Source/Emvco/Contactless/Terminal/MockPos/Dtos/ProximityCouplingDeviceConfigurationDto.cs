using Play.Emv.Ber.DataElements;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time;

namespace MockPos.Dtos;

public class ProximityCouplingDeviceConfigurationDto
{
    #region Instance Values

    public ushort TimeoutValue { get; set; }

    #endregion

    #region Serialization

    public PcdConfiguration Decode() => new PcdConfiguration(new TimeoutValue(new Milliseconds(TimeoutValue)));

    #endregion
}