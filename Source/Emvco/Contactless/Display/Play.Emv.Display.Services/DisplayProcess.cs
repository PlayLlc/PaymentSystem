using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

using Play.Emv.Display.Contracts;
using Play.Messaging;
using Play.Messaging.Threads;

namespace Play.Emv.Display.Services;

/// <summary>
///     a MSG DataExchangeSignal is used as a carrier of the User Interface Request Data.Process D may receive MSG Signals
///     from
///     any other Process.
///     • default language
///     • the currency symbol to display for each currency code and the number of minor units for that currency code
///     • a number of message strings in the default language and potentially other languages
///     • a number of status identifiers(and the corresponding audio and LED Signals)
/// </summary>
public class DisplayProcess : CommandProcessingQueue<Message>
{
    #region Instance Values

    private readonly IDisplayMessageRepository _DisplayMessageRepository;
    private readonly IDisplayLed _LedDisplayService;
    private readonly IDisplayMessages _MessageDisplayService;

    #endregion

    #region Constructor

    public DisplayProcess(
        IDisplayMessages messageDisplayService, IDisplayLed ledDisplayService, IDisplayMessageRepository displayMessageRepository) : base(
        new CancellationTokenSource())
    {
        _MessageDisplayService = messageDisplayService;
        _LedDisplayService = ledDisplayService;
        _DisplayMessageRepository = displayMessageRepository;
    }

    #endregion

    #region Instance Members

    protected override async Task Handle(Message command)
    {
        if (command is DisplayMessageRequest displayMessageRequestCommand)
        {
            await Handle(displayMessageRequestCommand).ConfigureAwait(false);
            return;
        }

        if (command is StopDisplayRequest stopDisplayRequestCommand)
        {
            await Handle(stopDisplayRequestCommand).ConfigureAwait(false);
            return;
        }
            
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NetworkInformationException"></exception>
    private async Task Handle(DisplayMessageRequest request)
    {
        DisplayMessage displayMessage = _DisplayMessageRepository.Get(request.GetLanguagePreference().GetPreferredLanguage(), request.GetMessageIdentifier());

        // Hack: I don't remember if 'Status' is what you're supposed to use for LED, but i sincerely doubt it
        await _LedDisplayService.Display(request.GetStatus()).ConfigureAwait(false);
        await _MessageDisplayService.Display(displayMessage).ConfigureAwait(false);

        throw new NetworkInformationException();
    }

    public async Task Handle(StopDisplayRequest request) =>

        // TODO: Implement STOP signal logic
        _CancellationTokenSource.Cancel();

    #endregion
}