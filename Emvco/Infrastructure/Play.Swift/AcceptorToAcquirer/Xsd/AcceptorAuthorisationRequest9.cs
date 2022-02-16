using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.001.001.09")]
public partial class AcceptorAuthorisationRequest9
{
    #region Instance Values

    private CardPaymentEnvironment74 envtField;
    private CardPaymentContext28 cntxtField;
    private CardPaymentTransaction92 txField;
    private SupplementaryData1[] splmtryDataField;

    /// <remarks />
    public CardPaymentEnvironment74 Envt
    {
        get => envtField;
        set => envtField = value;
    }

    /// <remarks />
    public CardPaymentContext28 Cntxt
    {
        get => cntxtField;
        set => cntxtField = value;
    }

    /// <remarks />
    public CardPaymentTransaction92 Tx
    {
        get => txField;
        set => txField = value;
    }

    /// <remarks />
    [XmlElement("SplmtryData")]
    public SupplementaryData1[] SplmtryData
    {
        get => splmtryDataField;
        set => splmtryDataField = value;
    }

    #endregion
}