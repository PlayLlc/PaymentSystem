﻿using System;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.PrepareGenerateAc;

public partial class PrepareGenerateAcService
{
    private class WriteIntegratedDataStorage
    {
        #region Instance Values

        private readonly KernelDatabase _Database;
        private readonly IEndpointClient _EndpointClient;
        private readonly Owhf2 _Owhf2;
        private readonly Owhf2Aes _Owhf2Aes;

        #endregion

        #region Constructor

        public WriteIntegratedDataStorage(KernelDatabase database, IEndpointClient endpointClient)
        {
            _Database = database;
            _EndpointClient = endpointClient;
            _Owhf2 = new Owhf2(); // TODO: this will be singleton.
            _Owhf2Aes = new Owhf2Aes();
        }

        public WriteIntegratedDataStorage(KernelDatabase database, IEndpointClient endpointClient, Owhf2 owhf2, Owhf2Aes owhf2Aes)
        {
            _Database = database;
            _EndpointClient = endpointClient;
            _Owhf2 = owhf2; // TODO: this will be singleton.
            _Owhf2Aes = owhf2Aes;
        }

        #endregion

        #region Instance Members

        public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
        {
            DataStorageDataObjectList dataStorageDataObjectList = _Database.Get<DataStorageDataObjectList>(DataStorageDataObjectList.Tag);

            if (!IsDataStorageDigestHashPresent(dataStorageDataObjectList))
            {
                HandleGenerateAcCapdu(session);

                return currentStateIdRetriever.GetStateId();
            }

            if (!IsDataStorageInputTermPresent())
            {
                HandleGenerateAcCapdu(session);

                return currentStateIdRetriever.GetStateId();
            }

            UpdateDataStorageDigestHash();

            HandleGenerateAcCapdu(session);

            return currentStateIdRetriever.GetStateId();
        }

        #region GAC.40

        /// <remarks>Book C-2 Section GAC.40</remarks>
        private bool IsDataStorageDigestHashPresent(DataStorageDataObjectList dsdol) => dsdol.Exists(DataStorageDigestHash.Tag);

        #endregion

        #region GAC.41

        /// <remarks>Book C-2 Section GAC.41</remarks>
        private bool IsDataStorageInputTermPresent() => _Database.IsPresent(DataStorageInputTerminal.Tag);

        #endregion

        #region GAC.42 - GAC.44

        /// <remarks>Book C-2 Section GAC.42- GAC.44</remarks>
        private void UpdateDataStorageDigestHash()
        {
            ApplicationCapabilitiesInformation? applicationCapabilitiesInformation =
                _Database.Get<ApplicationCapabilitiesInformation>(ApplicationCapabilitiesInformation.Tag);

            DataStorageInputTerminal input = _Database.Get<DataStorageInputTerminal>(DataStorageInputTerminal.Tag);

            if (applicationCapabilitiesInformation.GetDataStorageVersionNumber() == DataStorageVersionNumbers.Version1)
                _Owhf2.Hash(_Database, input.EncodeValue());
            else
                _Owhf2Aes.Hash(_Database, input.EncodeValue());
        }

        #endregion

        #region GAC.45 - GAC.48

        /// <remarks>Book C-2 Section GAC.45 - GAC.48</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="BerParsingException"></exception>
        private void HandleGenerateAcCapdu(Kernel2Session session)
        {
            CryptogramType acType = session.GetApplicationCryptogramType();
            ReferenceControlParameter referenceControlParam = new(acType, true);

            _Database.Update(referenceControlParam);

            CardRiskManagementDataObjectList1RelatedData cdol1RelatedData =
                _Database.Get<CardRiskManagementDataObjectList1RelatedData>(CardRiskManagementDataObjectList1RelatedData.Tag);

            DataStorageDataObjectList dsDol = _Database.Get<DataStorageDataObjectList>(DataStorageDataObjectList.Tag);

            GenerateApplicationCryptogramRequest? capdu = GenerateApplicationCryptogramRequest.Create(session.GetTransactionSessionId(), referenceControlParam,
                cdol1RelatedData, dsDol.AsDataObjectListResult(_Database));

            _EndpointClient.Send(capdu);
        }

        #endregion

        #endregion
    }
}