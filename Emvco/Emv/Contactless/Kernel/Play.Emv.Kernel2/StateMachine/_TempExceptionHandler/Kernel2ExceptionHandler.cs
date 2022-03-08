using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Extensions;
using Play.Emv.DataElements.Emv;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Databases;

namespace Play.Emv.Kernel2.StateMachine._TempExceptionHandler;

// TODO: This is an idea to consolidate the exception handling for all of the kernel state transitions so they're all in one spot.
// TODO: The idea is that each state uses a builder pattern to construct the exception handling that they'll need when they're
// TODO: constructed. That way we can just call _ExceptionHandler.TryCatch(() => action). If it returns true then we will enqueue
// TODO: a new STOP signal within the state transition. This will work if the exception handling is pretty consistent for each
// TODO: possible exception.... This will also allow us to model our custom internal exception to reflect L1, L2, and L3 errors
// TODO: so that it can be more or less a 1 - 1 mapping for the exception, the ErrorIndication, and how it's handled



/*
 * 
 L1 - Hardware Error (Thrown in PCD Adapter/Driver)
- PCD
    - Play.Icc


L2 - Processing Error
- Card Data Error
- Parsing Error
- Empty Candidate List
- Mag Not Supported
- CamFailed;
- CardDataError;
- CardDataMissing;
- EmptyCandidateList;
- IdsDataError;
- IdsNoMatchingAc;
- IdsReaderError;
- IdsWriterError;
- MagStripeNotSupported;
- MaxLimitExceeded;
- NoPpse;
- Ok;
- ParsingError;
- PpseFault;
- StatusBytes;
- TerminalDataError;


L3 - POS Error
- Amount Not Present (Bad Request)
- Timeout
...
*/

public abstract class ExceptionHandler
{
#region Instance Values

protected readonly IKernelEndpoint _KernelEndpoint;
protected readonly KernelDatabase _KernelDatabase;

#endregion

#region Constructor

protected ExceptionHandler(IKernelEndpoint kernelEndpoint, KernelDatabase kernelDatabase)
{
    _KernelEndpoint = kernelEndpoint;
    _KernelDatabase = kernelDatabase;
}

#endregion

#region Instance Members

public abstract Level1Error GetLevel1Error();
public abstract bool TryCatchError(Action action);

#endregion
}

public class TimeoutExceptionHandler : ExceptionHandler
{
#region Constructor

public TimeoutExceptionHandler(IKernelEndpoint kernelEndpoint, KernelDatabase kernelDatabase) : base(kernelEndpoint, kernelDatabase)
{ }

#endregion

#region Instance Members

public override Level1Error GetLevel1Error() => Level1Error.TimeOutError;
public override bool TryCatchError(Action action) => throw new NotImplementedException();

#endregion
}

// TODO: Actions are expensive to pass around. Let's see if there's a way to compile the action or pass interfaces or something
internal class Kernel2ExceptionHandler
{
#region Instance Values

private readonly ExceptionHandler[] _ExceptionHandlers;

#endregion

#region Constructor

public Kernel2ExceptionHandler(ExceptionHandler[] exceptionHandlers)
{
    _ExceptionHandlers = exceptionHandlers;
}

#endregion

#region Instance Members

public bool TryCatchError(Action action)
{
    for (nint i = 0; i < _ExceptionHandlers.Length; i++)
    {
        if (_ExceptionHandlers[i].TryCatchError(action))
            return true;
    }

    return false;
}

#endregion

public class Builder
{
    #region Instance Values

    private readonly Kernel2Database _KernelDatabase;
    private readonly IKernelEndpoint _KernelEndpoint;
    private readonly List<Level1Error> _Level1Errors = new();
    private readonly List<Level2Error> _Level2Errors = new();
    private readonly List<Level3Error> _Level3Errors = new();

    #endregion

    #region Constructor

    public Builder(Kernel2Database kernelDatabase, IKernelEndpoint kernelEndpoint)
    {
        _KernelDatabase = kernelDatabase;
        _KernelEndpoint = kernelEndpoint;
    }

    #endregion

    #region Instance Members

    public void Set(params Level1Error[] errors)
    {
        for (nint i = 0; i < errors.Length; i++)
            _Level1Errors.Add(errors[i]);
    }

    public void Set(params Level2Error[] errors)
    {
        for (nint i = 0; i < errors.Length; i++)
            _Level2Errors.Add(errors[i]);
    }

    public void Set(params Level3Error[] errors)
    {
        for (nint i = 0; i < errors.Length; i++)
            _Level3Errors.Add(errors[i]);
    }

    public Kernel2ExceptionHandler Complete()
    {
        List<ExceptionHandler> buffer = new();
        if (_Level1Errors.Contains(Level1Error.TimeOutError))
            buffer.Add(new TimeoutExceptionHandler(_KernelEndpoint, _KernelDatabase));

        //....

        return new Kernel2ExceptionHandler(buffer.ToArray());
    }

    #endregion
}
}