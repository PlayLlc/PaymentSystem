using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs;

namespace Play.Interchange.Messages.DataFields.ValueObjects;

public abstract class DataFieldMapper
{
    #region Instance Members

    public abstract DataFieldId GetDataFieldId();
    public abstract PlayEncodingId GetPlayEncodingId();

    #endregion
}