﻿using Play.Emv.Selection.Configuration;

namespace MockPos.Configuration;

public class TransactionProfileDto
{
    #region Instance Values

    public int KernelId { get; set; }
    public string? ApplicationId { get; set; }
    public int TransactionType { get; set; }
    public int ApplicationPriorityIndicator { get; set; }
    public int ContactlessTransactionLimit { get; set; }
    public int ContactlessFloorLimit { get; set; }
    public int CvmRequiredLimit { get; set; }
    public int KernelConfiguration { get; set; }
    public int TerminalTransactionQualifiers { get; set; }
    public bool IsStatusCheckSupported { get; set; }
    public bool IsZeroAmountAllowed { get; set; }
    public bool IsZeroAmountAllowedForOffline { get; set; }
    public bool IsExtendedSelectionSupported { get; set; }

    #endregion

    #region Serialization

    public TransactionProfile Decode()
    { }

    #endregion
}