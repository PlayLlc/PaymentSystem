using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Configuration;
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