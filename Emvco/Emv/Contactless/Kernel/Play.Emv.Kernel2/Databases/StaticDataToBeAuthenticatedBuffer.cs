using System.Collections.Generic;

using Play.Ber.DataObjects;

namespace Play.Emv.Kernel2.Databases;

public class StaticDataToBeAuthenticatedBuffer
{
    #region Instance Values

    private List<TagLengthValue> _Buffer;

    #endregion

    #region Constructor

    public StaticDataToBeAuthenticatedBuffer()
    {
        _Buffer = new List<TagLengthValue>();
    }

    #endregion
}