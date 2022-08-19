using Play.Emv.Reader;
using Play.Emv.Reader.Services;
using Play.Messaging;

namespace MockPos
{
    internal class ReaderFactory
    {
        public static MainEndpoint Create(ReaderConfiguration readerConfiguration, IEndpointClient endpointClient) =>
            MainEndpoint.Create(readerConfiguration, endpointClient);
    }
}