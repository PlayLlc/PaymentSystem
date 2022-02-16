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
public partial class IssuerAndSerialNumber1
{
    #region Instance Values

    private RelativeDistinguishedName1[] issrField;
    private byte[] srlNbField;

    /// <remarks />
    [XmlArrayItem("RltvDstngshdNm", IsNullable = false)]
    public RelativeDistinguishedName1[] Issr
    {
        get => this.issrField;
        set => this.issrField = value;
    }

    /// <remarks />
    [XmlElement(DataType = "base64Binary")]
    public byte[] SrlNb
    {
        get => this.srlNbField;
        set => this.srlNbField = value;
    }

    #endregion
}