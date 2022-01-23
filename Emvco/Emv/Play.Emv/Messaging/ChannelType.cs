using System;

using Play.Codecs;
using Play.Core;
using Play.Messaging;

namespace Play.Emv.Messaging;

public record ChannelType : EnumObject<ChannelTypeId>
{
    #region Static Metadata

    public static readonly ChannelType Terminal = new(new ChannelTypeId(nameof(Terminal)));
    public static readonly ChannelType Reader = new(new ChannelTypeId(nameof(Reader)));
    public static readonly ChannelType Kernel = new(new ChannelTypeId(nameof(Kernel)));
    public static readonly ChannelType ProximityCouplingDevice = new(new ChannelTypeId(nameof(ProximityCouplingDevice)));
    public static readonly ChannelType Display = new(new ChannelTypeId(nameof(Display)));
    public static readonly ChannelType Selection = new(new ChannelTypeId(nameof(Selection)));

    #endregion

    #region Constructor

    public ChannelType(ChannelTypeId value) : base(value)
    { }

    #endregion

    #region Instance Members

    private static ulong GetChannelTypeId(Type type) => PlayEncoding.UnsignedInteger.GetUInt64(PlayEncoding.ASCII.GetBytes(type.FullName));

    public static string GetChannelTypeName(ChannelTypeId value)
    {
        if (value == Terminal)
            return nameof(Terminal);
        if (value == Reader)
            return nameof(Reader);
        if (value == Kernel)
            return nameof(Kernel);
        if (value == ProximityCouplingDevice)
            return nameof(ProximityCouplingDevice);
        if (value == Display)
            return nameof(Display);
        if (value == Selection)
            return nameof(Selection);

        throw new ArgumentOutOfRangeException(nameof(value),
            $"No {nameof(ChannelType)} could not be found with the {nameof(ChannelTypeId)}: [{value}]");
    }

    #endregion
}