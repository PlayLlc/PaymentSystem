using Play.Emv.Icc;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

/// <summary>
///     Response from an the ACT signal <see cref="ActivatePcdRequest" />
/// </summary>
public record ActivatePcdResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(ActivatePcdResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.ProximityCouplingDevice;

    #endregion

    #region Instance Values

    private readonly Level1Error _Level1Error;
    private readonly bool _IsCollisionDetected;

    #endregion

    #region Constructor

    public ActivatePcdResponse(CorrelationId correlationId, bool isCollisionDetected, Level1Error level1Error) : base(correlationId,
     MessageTypeId, ChannelTypeId)
    {
        _Level1Error = level1Error;
        _IsCollisionDetected = isCollisionDetected;
    }

    #endregion

    #region Instance Members

    public bool Successful() => _Level1Error == Level1Error.Ok;
    public Level1Error GetLevel1Error() => _Level1Error;
    public bool IsCollisionDetected() => _IsCollisionDetected;

    #endregion
}