using System;

using Play.Emv.Identifiers;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private StateId ProcessWithoutCda(
            IGetKernelStateId currentStateIdRetriever, Kernel2Session session, GenerateApplicationCryptogramResponse rapdu) =>
            throw new NotImplementedException();
    }
}