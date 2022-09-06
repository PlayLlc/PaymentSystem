using System.Text.Json;

using MockPos.Dtos;
using MockPos.Factories;
using MockPos.Services;

using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Configuration;
using Play.Emv.Display.Configuration;
using Play.Emv.Display.Services;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel2;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Services;
using Play.Emv.Reader;
using Play.Emv.Reader.Configuration;
using Play.Emv.Reader.Services;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Services;
using Play.Emv.Terminal.Services;
using Play.Emv.Terminal.Session;
using Play.Messaging;

namespace MockPos;

public class TestClassz
{
    #region Instance Values

    public List<string> MyStringProperty { get; set; }

    #endregion
}

internal class Program
{
    #region Instance Members

    private static int Test(List<string> hello)
    {
        var a = hello.FirstOrDefault();

        return hello.Count;
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private static void Main(string[] args)
    {
        var b = new TestClassz();

        // Configuration
        Test(b.MyStringProperty);

        PosConfigurationDto posConfigurationDto = GetDto();
        EmvRuntimeCodec emvRuntimeCodec = new();

        TerminalConfiguration terminalConfiguration = posConfigurationDto.GetTerminalConfiguration();
        TransactionProfiles transactionProfiles = posConfigurationDto.GetTransactionProfiles();
        SelectionConfiguration selectionConfiguration = new(transactionProfiles, null);
        CertificateAuthorityDatasets certificateAuthorityDatasets = posConfigurationDto.GetCertificateAuthorityDatasets();
        KernelPersistentConfigurations kernelPersistentConfigurations = posConfigurationDto.GetKernelPersistent(emvRuntimeCodec);
        ReaderPersistentConfiguration readerPersistentConfiguration = posConfigurationDto.GetReaderPersistentConfiguration(emvRuntimeCodec);
        ReaderDatabase readerDatabase = new(kernelPersistentConfigurations, selectionConfiguration.TransactionProfiles, certificateAuthorityDatasets,
            readerPersistentConfiguration);
        DisplayConfigurations displayConfiguration = posConfigurationDto.GetDisplayConfiguration();
        PcdConfiguration pcdConfiguration = posConfigurationDto.GetPcdConfiguration();

        // Services
        MessageBus messageBus = new();
        IEndpointClient endpointClient = messageBus.GetEndpointClient();
        DisplayServiceMock displayServiceMock = new();
        PcdServiceMock pcdServiceMock = new();

        // Endpoint Processes
        TerminalEndpoint terminalEndpoint = TerminalEndpoint.Create(terminalConfiguration, endpointClient);
        MainEndpoint mainEndpoint = ReaderFactory.Create(readerDatabase, endpointClient);
        KernelEndpoint kernelEndpoint = KernelFactory.Create(terminalConfiguration, readerDatabase, endpointClient);
        SelectionEndpoint selectionEndpoint = SelectionEndpoint.Create(selectionConfiguration, endpointClient);

        DisplayEndpoint displayEndpoint = DisplayEndpoint.Create(displayConfiguration, displayServiceMock, displayServiceMock, endpointClient);
        ProximityCouplingDeviceEndpoint pcdEndpoint = ProximityCouplingDeviceEndpoint.Create(pcdConfiguration, pcdServiceMock, endpointClient);
    }

    /// <exception cref="JsonException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    private static PosConfigurationDto GetDto() =>
        JsonSerializer.Deserialize<PosConfigurationDto>(File.ReadAllText(@"C:\Source\PaymentSystem\Source\Emvco\Contactless\Terminal\MockPos\TestJson.json"))!;

    #endregion

    public class TestClass
    {
        #region Instance Values

        public string TestProperty { get; set; }

        #endregion
    }
}