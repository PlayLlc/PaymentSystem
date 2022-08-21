using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Contracts;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Selection.Start;

public class CardCollisionHandler
{
    #region Instance Values

    private readonly IEndpointClient _EndpointClient;

    #endregion

    #region Constructor

    public CardCollisionHandler(IEndpointClient endpointClient)
    {
        _EndpointClient = endpointClient;
    }

    #endregion

    #region Instance Members

    #region Main

    /// <summary>
    ///     HandleCardCollisions
    /// </summary>
    /// <param name="response"></param>
    /// <param name="outcome"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void HandleCardCollisions(ActivatePcdResponse response, Outcome outcome)
    {
        if (!outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData))
        {
            throw new InvalidOperationException(
                $"There is supposed to be a {nameof(UserInterfaceRequestData)} at this stage of the transaction but none could be found");
        }

        if (userInterfaceRequestData!.GetMessageIdentifier() != DisplayMessageIdentifiers.PleasePresentOneCardOnly)
            return;

        if (response.IsCollisionDetected())
            HandleCardCollision(outcome);
        else
            HandleCollisionHasBeenResolved(outcome);
    }

    #endregion

    #region 3.2.1.5

    private void HandleCollisionHasBeenResolved(Outcome outcome)
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(DisplayMessageIdentifiers.PleasePresentOneCardOnly);
        builder.Set(DisplayStatuses.ReadyToRead);

        _EndpointClient.Send(new DisplayMessageRequest(builder.Complete()));
    }

    #endregion

    #region 3.2.1.4

    private void HandleCardCollision(Outcome outcome)
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(DisplayMessageIdentifiers.PleasePresentOneCardOnly);
        builder.Set(DisplayStatuses.ProcessingError);

        _ = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);

        _EndpointClient.Send(new DisplayMessageRequest(builder.Complete()));
    }

    #endregion

    #endregion
}