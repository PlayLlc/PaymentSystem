using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Messaging.Tests.Data.Channels;
using Play.Messaging.Tests.Data.Messages;
using Play.Testing.BaseTestClasses;

using Xunit;
using Xunit.Sdk;

namespace Play.Messaging.Tests.Endpoints
{
    public class EndpointClientTests : TestBase
    {
        #region Instance Values

        private readonly TestEndpoint1 _TestEndpoint1;
        private readonly MessageBus _MessageBus;

        #endregion

        #region Constructor

        public EndpointClientTests()
        {
            _MessageBus = new MessageBus();
            _TestEndpoint1 = new TestEndpoint1(_MessageBus);
        }

        #endregion

        #region Instance Members

        [Fact]
        public void Test()
        {
            //int expected = 10;
            //TestRequestMessage message = new TestRequestMessage(expected);
            //_MessageRouter.
        }

        #endregion
    }
}