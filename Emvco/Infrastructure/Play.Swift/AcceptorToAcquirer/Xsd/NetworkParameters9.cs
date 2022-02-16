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
public partial class NetworkParameters9
{
    #region Instance Values

    private NetworkType1Code ntwkTpField;
    private string adrValField;

    /// <remarks />
    public NetworkType1Code NtwkTp
    {
        get => this.ntwkTpField;
        set => this.ntwkTpField = value;
    }

    /// <remarks />
    public string AdrVal
    {
        get => this.adrValField;
        set => this.adrValField = value;
    }

    #endregion
}