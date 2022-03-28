using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Kernel2.Databases;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts.SignalOut.Queries
{
    public record PutDataResponse : QueryPcdResponse
    {
        #region Static Metadata

        public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(PutDataResponse));

        #endregion

        #region Constructor

        public PutDataResponse(CorrelationId correlationId, TransactionSessionId transactionSessionId, PutDataRApduSignal rApdu) :
            base(correlationId, MessageTypeId, transactionSessionId, rApdu)
        { }

        #endregion
    }
}