﻿using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Selection.Start;

public class CardCollisionHandler
{
    #region Instance Values

    private readonly IHandleDisplayRequests _DisplayProcess;

    #endregion

    #region Constructor

    public CardCollisionHandler(IHandleDisplayRequests displayProcess)
    {
        _DisplayProcess = displayProcess;
    }

    #endregion

    #region Instance Members

    #region Main

    /// <summary>
    ///     HandleCardCollisions
    /// </summary>
    /// <param name="request"></param>
    /// <param name="outcome"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void HandleCardCollisions(ActivatePcdResponse request, Outcome outcome)
    {
        if (!outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData))
        {
            throw new
                InvalidOperationException($"There is supposed to be a {nameof(UserInterfaceRequestData)} at this stage of the transaction but none could be found");
        }

        if (userInterfaceRequestData!.GetMessageIdentifier() != MessageIdentifiers.PleasePresentOneCardOnly)
            return;

        if (request.IsCollisionDetected())
            HandleCardCollision(outcome);
        else
            HandleCollisionHasBeenResolved(outcome);
    }

    #endregion

    #region 3.2.1.5

    private void HandleCollisionHasBeenResolved(Outcome outcome)
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(MessageIdentifiers.PleasePresentOneCardOnly);
        builder.Set(Status.ReadyToRead);
        outcome.Update(builder);

        _ = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);

        _DisplayProcess.Request(new DisplayMessageRequest(userInterfaceRequestData!));
    }

    #endregion

    #region 3.2.1.4

    private void HandleCardCollision(Outcome outcome)
    {
        UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(MessageIdentifiers.PleasePresentOneCardOnly);
        builder.Set(Status.ProcessingError);
        outcome.Update(builder);

        _ = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);

        _DisplayProcess.Request(new DisplayMessageRequest(userInterfaceRequestData!));
    }

    #endregion

    #endregion
}