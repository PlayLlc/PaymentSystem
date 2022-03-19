using System;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Outcomes;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Kernel.Databases
{
    public abstract partial class KernelDatabase
    {
        #region Outcome

        /// <summary>
        ///     CreateEmvDiscretionaryData
        /// </summary>
        /// <param name="dataExchanger"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void CreateEmvDiscretionaryData(DataExchangeKernelService dataExchanger)
        {
            KernelOutcome.CreateEmvDiscretionaryData(this, dataExchanger);
        }

        /// <summary>
        ///     CreateMagstripeDiscretionaryData
        /// </summary>
        /// <param name="dataExchanger"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void CreateMagstripeDiscretionaryData(DataExchangeKernelService dataExchanger)
        {
            KernelOutcome.CreateMagstripeDiscretionaryData(this, dataExchanger);
        }

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        public ErrorIndication GetErrorIndication() => (ErrorIndication)Get(ErrorIndication.Tag);  

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        private OutcomeParameterSet GetOutcomeParameterSet() => (OutcomeParameterSet) Get(OutcomeParameterSet.Tag); 

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        private TerminalVerificationResults GetTerminalVerificationResults() =>
            (TerminalVerificationResults)Get(TerminalVerificationResults.Tag); 

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private UserInterfaceRequestData GetUserInterfaceRequestData()
        {
            if (IsPresentAndNotEmpty(UserInterfaceRequestData.Tag))
            {
                UserInterfaceRequestData.Builder builder = UserInterfaceRequestData.GetBuilder();

                return builder.Complete();
            }
            return (UserInterfaceRequestData)Get(UserInterfaceRequestData.Tag); 
        }

        /// <summary>
        ///     GetDataRecord
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private DataRecord? GetDataRecord()
        {
            if (IsPresentAndNotEmpty(DataRecord.Tag))
                return null;


            return (DataRecord)Get(DataRecord.Tag); 
        }

        /// <summary>
        ///     GetDiscretionaryData
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private DiscretionaryData? GetDiscretionaryData()
        {
            if (IsPresentAndNotEmpty(DiscretionaryData.Tag))
                return null;

            return (DiscretionaryData)Get(DiscretionaryData.Tag); 
        }

        #region Write Outcome

        /// <exception cref="TerminalDataException"></exception>
        public void Update(MessageIdentifier value)
        {
            try
            {
                _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
                _UserInterfaceRequestDataBuilder.Set(value);
                Update(_UserInterfaceRequestDataBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(Status value)
        {
            try
            {
                _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
                _UserInterfaceRequestDataBuilder.Set(value);
                Update(_UserInterfaceRequestDataBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(MessageHoldTime value)
        {
            try
            {
                _UserInterfaceRequestDataBuilder.Reset(GetUserInterfaceRequestData());
                _UserInterfaceRequestDataBuilder.Set(value);
                Update(_UserInterfaceRequestDataBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(UserInterfaceRequestData)}",
                                                exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Set(TerminalVerificationResult value)
        {
            try
            {
                _TerminalVerificationResultBuilder.Reset(GetTerminalVerificationResults());
                _TerminalVerificationResultBuilder.Set(value);
                Update(_TerminalVerificationResultBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(TerminalVerificationResults)}",
                                                exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(TerminalVerificationResults)}",
                                                exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(TerminalVerificationResults)}",
                                                exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(StatusOutcome value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.Set(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(CvmPerformedOutcome value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.Set(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(OnlineResponseOutcome value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.Set(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(FieldOffRequestOutcome value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.Set(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(StartOutcome value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.Set(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void SetUiRequestOnRestartPresent(bool value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void SetIsDataRecordPresent(bool value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.SetIsDataRecordPresent(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void SetIsDiscretionaryDataPresent(bool value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.SetIsDiscretionaryDataPresent(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void SetIsReceiptPresent(bool value)
        {
            try
            {
                _OutcomeParameterSetBuilder.Reset(GetOutcomeParameterSet());
                _OutcomeParameterSetBuilder.SetIsUiRequestOnRestartPresent(value);
                Update(_OutcomeParameterSetBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(OutcomeParameterSet)}", exception);
            }
        }

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Update(Level1Error value)
        {
            try
            {
                _ErrorIndicationBuilder.Reset(GetErrorIndication());
                _ErrorIndicationBuilder.Set(value);
                Update(_ErrorIndicationBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
        }

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Update(Level2Error value)
        {
            try
            {
                _ErrorIndicationBuilder.Reset(GetErrorIndication());
                _ErrorIndicationBuilder.Set(value);
                Update(_ErrorIndicationBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
        }

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Update(Level3Error value)
        {
            try
            {
                _ErrorIndicationBuilder.Reset(GetErrorIndication());
                _ErrorIndicationBuilder.Set(value);
                Update(_ErrorIndicationBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
        }

        /// <exception cref="TerminalDataException"></exception>
        public void Update(StatusWords value)
        {
            try
            {
                _ErrorIndicationBuilder.Reset(GetErrorIndication());
                _ErrorIndicationBuilder.Set(value);
                Update(_ErrorIndicationBuilder.Complete());
            }
            catch (DataElementParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (CodecParsingException exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
            catch (Exception exception)
            {
                throw new TerminalDataException($"An error occurred while writing a value to the {nameof(ErrorIndication)}", exception);
            }
        }

        /// <summary>
        ///     Reset
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Reset(OutcomeParameterSet value)
        {
            Update(value);
        }

        /// <summary>
        ///     Reset
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Reset(UserInterfaceRequestData value)
        {
            Update(value);
        }

        /// <summary>
        ///     Reset
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="TerminalDataException"></exception>
        public void Reset(ErrorIndication value)
        {
            Update(value);
        }

        #endregion

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        public Outcome GetOutcome() =>
            new(GetErrorIndication(), GetOutcomeParameterSet(), GetDataRecord(), GetDiscretionaryData(), GetUserInterfaceRequestData());

        #endregion
    }
}