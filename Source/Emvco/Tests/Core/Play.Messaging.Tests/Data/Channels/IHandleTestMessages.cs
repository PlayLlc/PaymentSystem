using Play.Messaging.Tests.Data.Messages;

namespace Play.Messaging.Tests.Data.Channels;

public interface IHandleTestMessages
{
    #region Instance Members

    public void Request(TestRequestMessage message);

    #endregion
}