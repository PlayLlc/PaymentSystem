using System;
using System.Threading;
using System.Threading.Tasks;

using Play.Core.Threads;
using Play.Emv.Terminal.Contracts.SignalIn;

namespace Play.Emv.Terminal.Services._Temp_DataExchange
{


    internal class DataExchangeProcess : CommandProcessingQueue
    {
        #region Constructor

        public DataExchangeProcess() : base(new CancellationTokenSource())
        { }

        #endregion

        #region Instance Members

        internal void Enqueue(QueryTerminalRequest request)
        {
            Enqueue((dynamic) request);
        }

        private async Task Handle(QueryTerminalRequest request)
        {
            throw new NotImplementedException();

            //await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
        }

        protected override Task Handle(dynamic command)
        {
            return Handle(command);
        }

        #endregion
    }
}