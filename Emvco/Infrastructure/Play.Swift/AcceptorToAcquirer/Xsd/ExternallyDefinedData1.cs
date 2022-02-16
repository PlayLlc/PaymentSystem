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
public partial class ExternallyDefinedData1
{
    #region Instance Values

    private string idField;
    private byte[] valField;
    private ContentInformationType23 prtctdValField;
    private string tpField;

    /// <remarks />
    public string Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] Val
    {
        get => this.valField;
        set => this.valField = value;
    }

    /// <remarks />
    public ContentInformationType23 PrtctdVal
    {
        get => this.prtctdValField;
        set => this.prtctdValField = value;
    }

    /// <remarks />
    public string Tp
    {
        get => this.tpField;
        set => this.tpField = value;
    }

    #endregion
}