using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.001.001.09")]
public partial class SupplementaryData1
{
    #region Instance Values

    private string plcAndNmField;
    private XmlElement envlpField;

    /// <remarks />
    public string PlcAndNm
    {
        get => this.plcAndNmField;
        set => this.plcAndNmField = value;
    }

    /// <remarks />
    public XmlElement Envlp
    {
        get => this.envlpField;
        set => this.envlpField = value;
    }

    #endregion
}