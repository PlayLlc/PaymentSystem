using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.Services.GenerateAcSetup
{
    /// <summary>
    ///     Prepares a Generate AC CAPDU for Terminal implementations with IDS capabilities implemented
    /// </summary>
    /// <remarks>Book EMVco C-2 Section 7.6 GAC</remarks>
    public class PrepareGenerateAcService
    {
        #region Instance Values

        private readonly IPerformTerminalActionAnalysis _TerminalActionAnalysisService;

        #endregion

        #region Instance Members

        public GenerateApplicationCryptogramRequest Create(Kernel2Session session, KernelDatabase database) =>
            throw new NotImplementedException();

        //private static void Kernel2Preprocessing(Kernel2Session session, KernelDatabase database)
        //{
        //    if (!database.IsIdsAndTtrImplemented())
        //        return;

        //    if (!database.IsIntegratedDataStorageReadFlagSet())
        //        return;

        //    var tvr = database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        //    if (tvr.CombinationDataAuthenticationFailed())
        //        return HandleCdaFailed();
        //}

        // A - IDS Write
        private void HandleIdsWrite()
        { }

        // B - No IDS
        //private GenerateApplicationCryptogramRequest HandleNoIds(Kernel2Session session, KernelDatabase database) =>
        //    _TerminalActionAnalysisService.Process(session.GetTransactionSessionId(), database);

        // C - IDS Read Only
        private void HandleIdsReadOnly()
        { }

        // D - CDA Failed
        private static void HandleCdaFailed()
        { }

        #endregion
    }
}