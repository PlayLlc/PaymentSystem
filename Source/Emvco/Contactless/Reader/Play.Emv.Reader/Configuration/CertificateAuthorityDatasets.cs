using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Reader.Configuration
{
    public class CertificateAuthorityDatasets
    {
        #region Instance Values

        private readonly CertificateAuthorityDataset[] _CertificateAuthorityDatasets;

        #endregion

        #region Constructor

        public CertificateAuthorityDatasets(CertificateAuthorityDataset[] certificateAuthorityDatasets)
        {
            _CertificateAuthorityDatasets = certificateAuthorityDatasets;
        }

        #endregion

        #region Instance Members

        public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets() => _CertificateAuthorityDatasets;

        public void PurgeRevokedCertificates()
        {
            foreach (var value in _CertificateAuthorityDatasets)
                value.PurgeRevokedCertificates();
        }

        #endregion
    }
}