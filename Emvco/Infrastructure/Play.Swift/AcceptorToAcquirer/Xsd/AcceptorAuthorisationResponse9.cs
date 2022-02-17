using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.002.001.09")]
public partial class AcceptorAuthorisationResponse9
{
    #region Instance Values

    private CardPaymentEnvironment74 envtField;
    private CardPaymentTransaction93 txField;
    private CardPaymentTransaction94 txRspnField;
    private SupplementaryData1[] splmtryDataField;

    /// <remarks />
    public CardPaymentEnvironment74 Envt
    {
        get => envtField;
        set => envtField = value;
    }

    /// <remarks />
    public CardPaymentTransaction93 Tx
    {
        get => txField;
        set => txField = value;
    }

    /// <remarks />
    public CardPaymentTransaction94 TxRspn
    {
        get => txRspnField;
        set => txRspnField = value;
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