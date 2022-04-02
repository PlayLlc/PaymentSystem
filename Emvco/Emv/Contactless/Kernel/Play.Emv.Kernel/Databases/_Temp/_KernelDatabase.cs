using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Emv.Kernel.Databases._Temp;
// TODO: The Kernel Database is getting pretty fucking crowded. It represents too many concepts so break it down into smaller components

public class _KernelDatabase
{
    #region Instance Values

    public __ConfigurationDatabase Configuration;
    public _TransactionDatabase Transaction;

    #endregion
}

public class _TlvDatabase
{ }

public class __ConfigurationDatabase
{
    #region Instance Values

    private readonly _TlvDatabase _TlvDatabase;

    #endregion
}

public class _TransactionDatabase
{
    #region Instance Values

    private readonly _TlvDatabase _TlvDatabase;

    #endregion
}