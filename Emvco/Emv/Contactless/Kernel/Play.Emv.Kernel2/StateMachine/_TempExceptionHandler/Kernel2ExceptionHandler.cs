using System;
using System.Collections.Generic;

using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Kernel;
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
  ------------------------------------------------------------
 L1 - Hardware Error (Thrown in PCD Adapter/Driver)
------------------------------------------------------------
- PCD
    - Play.Icc

------------------------------------------------------------
L2 - Errors thrown in payment kernel
------------------------------------------------------------


============================================================
Parsing Error
------------------------------------------------------------
    When there's an exception thrown in Play.Ber, Play.Emv.Ber, Play.Emv.DataElements, or Play.Emv.Templates while decoding
------------------------------------------------------------
    Play.Ber, Play.Emv.Ber
============================================================
Card Data Error
------------------------------------------------------------
    When data from the card is an incorrect value or different than expected
------------------------------------------------------------
    Play.Emv.Kernel
============================================================
Card Data Missing
------------------------------------------------------------
    When a Template is missing a required Data Element
------------------------------------------------------------
    Play.Emv.Templates
============================================================ 
Terminal Data Error
------------------------------------------------------------
    If a format error is detected in data received from the Terminal, the Kernel must update the Error Indication data object as follows
------------------------------------------------------------
    Play.Ber, Play.Emv.Ber
============================================================
Status Bytes
------------------------------------------------------------
    When the RAPDU returns with an invalid SW. We can throw in Play.Icc and Play.Emv.Icc
------------------------------------------------------------
    Play.Icc, Play.Emv.Icc
============================================================
Max Limit Exceeded
------------------------------------------------------------
    Amount, Authorized (Numeric) > Reader Contactless Transaction Limit
------------------------------------------------------------
    Play.Emv.Terminal.Common
============================================================
Cryptographic Auth Method Failed
------------------------------------------------------------
    CDA, DDA, SDA fails
------------------------------------------------------------
    Play.Emv.Security
============================================================
Integrated Data Storage Reader Error
------------------------------------------------------------
    DS Summary 1 != DS Summary 2
------------------------------------------------------------
    Play.Emv.Terminal.Common
============================================================
Integrated Data Storage Writer Error
------------------------------------------------------------
    DS Summary 2 != DS Summary 3 &&'Stop if write failed' in DS ODS Info For Reader is set
------------------------------------------------------------
    Play.Emv.Terminal.Common
============================================================
Integrated Data Storage No Matching Application Cryptogram
------------------------------------------------------------
    Usable for AAC' in DS ODS Info For Reader is NOT set && 'Stop if no DS ODS Term' in DS ODS Info For Reader is set
------------------------------------------------------------
    Play.Emv.Security
============================================================
 Empty Candidate List
 ------------------------------------------------------------
    If the Candidate List is empty at Entry Point Start C
------------------------------------------------------------
    Play.Emv.Selection
============================================================
Magstripe Not Supported 
------------------------------------------------------------
    When the kernel and the card do not have a matching operating mode
------------------------------------------------------------
    Play.Emv.Kernel2
============================================================
Integrated Data Storage Error
------------------------------------------------------------
    if false (DS AC Type and DS ODS Info For Reader is not empty)
------------------------------------------------------------
    Play.Emv.Terminal.Common
============================================================
No Proximity Payment System Environment
------------------------------------------------------------
    When the card does not return the requested PPSE
------------------------------------------------------------
    Play.Icc
============================================================

------------------------------

------------------------------------------------------------
L3 - POS Error
------------------------------------------------------------
- Amount Not Present (Bad Request)
- Timeout 
*/

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