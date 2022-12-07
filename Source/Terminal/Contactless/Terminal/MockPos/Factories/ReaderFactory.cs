using Play.Emv.Reader;
using Play.Emv.Reader.Services;
using Play.Messaging;

namespace MockPos.Factories
{
    internal class ReaderFactory
    {
        #region Instance Members

        public static MainEndpoint Create(ReaderDatabase readerDatabase, IEndpointClient endpointClient) => MainEndpoint.Create(readerDatabase, endpointClient);

        #endregion
    }
}