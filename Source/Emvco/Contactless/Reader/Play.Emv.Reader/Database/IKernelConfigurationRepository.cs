﻿using System.Collections.Generic;

using Play.Emv.Ber.DataElements;

namespace Play.Emv.Reader.Database;

public interface IKernelConfigurationRepository
{
    #region Instance Members

    public Dictionary<KernelId, KernelConfiguration> Get(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);

    #endregion
}