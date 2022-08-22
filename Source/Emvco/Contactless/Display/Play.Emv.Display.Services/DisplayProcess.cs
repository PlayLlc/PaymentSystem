using System.Collections.Immutable;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

using Play.Core;
using Play.Emv.Ber.Enums;
using Play.Emv.Display.Configuration;
using Play.Emv.Display.Contracts;
using Play.Emv.Reader.Configuration;
using Play.Globalization.Language;
using Play.Globalization.Time;

namespace Play.Emv.Display.Services;

/// <summary>
///     a MSG DataExchangeSignal is used as a carrier of the User Interface Request Data.Process D may receive MSG Signals
///     from any other Process.
///     • default language
///     • the currency symbol to display for each currency code and the number of minor units for that currency code
///     • a number of message strings in the default language and potentially other languages
///     • a number of status identifiers(and the corresponding audio and LED Signals)
/// </summary>
public class DisplayProcess : CommandProcessingQueue
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<Alpha2LanguageCode, DisplayMessages> _DisplayMessages;
    private readonly IFormatDisplayMessages _DisplayMessageFormatter;
    private readonly IDisplayLed _LedDisplayService;
    private readonly IDisplayMessages _MessageDisplayService;

    #endregion

    #region Constructor

    public DisplayProcess(
        DisplayConfigurations displayConfiguration, IFormatDisplayMessages messageFormatter, IDisplayMessages messageDisplayService,
        IDisplayLed ledDisplayService) : base(new CancellationTokenSource())
    {
        _DisplayMessages = displayConfiguration.GetDisplayMessages().ToImmutableSortedDictionary(a => a.GetLanguageCode(), b => b);
        _DisplayMessageFormatter = messageFormatter;

        _MessageDisplayService = messageDisplayService;
        _LedDisplayService = ledDisplayService;
    }

    #endregion

    #region Instance Members

    public void Enqueue(DisplayMessageRequest request) => Enqueue((dynamic) request);
    public void Enqueue(StopDisplayRequest request) => Enqueue((dynamic) request);
    protected override async Task Handle(dynamic command) => await Handle(command).ConfigureAwait(false);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NetworkInformationException"></exception>
    public async Task Handle(DisplayMessageRequest request)
    {
        await _LedDisplayService.Display(request.GetStatus()).ConfigureAwait(false);
        await _MessageDisplayService.Display(_DisplayMessageFormatter.Display(request.GetUserInterfaceRequestData())).ConfigureAwait(false);
        Milliseconds holdTime = request.GetHoldTime().AsMilliseconds();
        await Task.WhenAny(Task.Delay(holdTime)).ConfigureAwait(false);
    }

    public async Task Handle(StopDisplayRequest request)
    {
        await _LedDisplayService.Display(DisplayStatuses.Idle).ConfigureAwait(false);
        await _MessageDisplayService.Display(_DisplayMessages.First().Value.GetDisplayMessage(DisplayMessageIdentifiers.ClearDisplay).Display())
            .ConfigureAwait(false);
    }

    #endregion
}