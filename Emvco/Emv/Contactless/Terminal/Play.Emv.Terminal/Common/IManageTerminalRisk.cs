﻿using System.Threading.Tasks;

using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Terminal;

public interface IManageTerminalRisk
{
    #region Instance Members

    public Task<TerminalRiskManagementResponse> Process(TerminalRiskManagementCommand command);

    #endregion
}