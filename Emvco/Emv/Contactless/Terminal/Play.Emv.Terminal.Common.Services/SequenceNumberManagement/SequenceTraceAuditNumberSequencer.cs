using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Emv.Terminal.Common.Services.SequenceNumberManagement;

internal class SequenceTraceAuditNumberSequencer : IGenerateSequenceTraceAuditNumbers
{
    #region Instance Values

    private readonly ushort _SequenceNumber;

    #endregion

    #region Constructor

    public SequenceTraceAuditNumberSequencer(ushort previousSequence)
    {
        _SequenceNumber = previousSequence;
    }

    #endregion

    #region Instance Members

    public ushort Generate() => throw new NotImplementedException();

    #endregion
}