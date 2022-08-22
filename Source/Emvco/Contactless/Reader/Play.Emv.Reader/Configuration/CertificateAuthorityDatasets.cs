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

        private readonly Dictionary<RegisteredApplicationProviderIndicator, CertificateAuthorityDataset[]> _CertificateAuthorityDatasets = new();

        #endregion

        #region Constructor

        public CertificateAuthorityDatasets(CertificateAuthorityDataset[] certificateAuthorityDatasets)
        {
            IEnumerable<RegisteredApplicationProviderIndicator> rids = certificateAuthorityDatasets.Select(a => a.GetRid()).Distinct();

            foreach (var rid in rids)
                _CertificateAuthorityDatasets.Add(rid, certificateAuthorityDatasets.Where(a => a.GetRid() == rid).ToArray());
        }

        #endregion

        #region Instance Members

        public CertificateAuthorityDataset[] GetCertificateAuthorityDatasets(RegisteredApplicationProviderIndicator rid) => _CertificateAuthorityDatasets[rid];

        public void PurgeRevokedCertificates()
        {
            foreach (var keyValue in _CertificateAuthorityDatasets)
            {
                foreach (var dataset in keyValue.Value)
                    dataset.PurgeRevokedCertificates();
            }
        }

        public void PurgeRevokedCertificates(RegisteredApplicationProviderIndicator rid)
        {
            if (!_CertificateAuthorityDatasets.TryGetValue(rid, out CertificateAuthorityDataset[]? values))
                return;

            foreach (var keyValue in values)
                keyValue.PurgeRevokedCertificates();
        }

        #endregion
    }
}