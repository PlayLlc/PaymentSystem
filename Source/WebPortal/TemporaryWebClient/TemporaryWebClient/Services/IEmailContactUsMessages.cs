using Play.Core;

using TemporaryWebClient.Models;

namespace TemporaryWebClient.Services;

public interface IEmailContactUsMessages
{
    #region Instance Members

    public Task<Result> Send(ContactUsModel message);

    #endregion
}