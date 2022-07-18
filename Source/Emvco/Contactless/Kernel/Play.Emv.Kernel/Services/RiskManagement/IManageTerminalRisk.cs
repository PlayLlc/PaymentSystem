using System.Threading.Tasks;

using Play.Emv.Configuration;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Kernel.Services;

public interface IManageTerminalRisk
{
    #region Instance Members

    public void Process(KernelDatabase database, TerminalRiskManagementConfiguration configuration);

    #endregion
}