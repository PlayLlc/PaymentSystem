namespace Play.Emv.Pcd;

public interface IManagePcdLifecycle
{
    #region Instance Members

    /// <summary>
    ///     STOP(Abort) DataExchangeSignal. Remove the field immediately without card removal procedure
    /// </summary>
    public void Abort();

    /// <summary>
    ///     ACT DataExchangeSignal. Generate an Answer to Reset, start polling for an PICC or HCE
    /// </summary>
    public void Activate();

    /// <summary>
    ///     STOP(CloseSession) DataExchangeSignal. Perform card removal as described in [EMV CL L1] and indicate when the Card
    ///     has been removed.
    ///     CloseSession” starts the removal sequence and returns a Signal L1RSP(Card Removed) when the Card has been removed.
    /// </summary>
    public void CloseSession();

    /// <summary>
    ///     STOP(CloseSessionCardCheckCommand) DataExchangeSignal. Perform card removal as described in [EMV CL L1] and
    ///     indicate when
    ///     the Card has been
    ///     “CloseSessionCardCheckCommand” includes a request to check for Card presence. If the Card is still present, then it
    ///     causes
    ///     a “Please Remove
    ///     Card” message to be displayed as part of the removal sequence and returns L1RSP(Card Removed) when the Card has
    ///     been removed. If the
    ///     Card has been removed already, then no message is displayed and an L1RSP(Card Removed) is returned immediately.
    ///     removed.
    /// </summary>
    public void CloseSessionCardCheck();

    #endregion
}