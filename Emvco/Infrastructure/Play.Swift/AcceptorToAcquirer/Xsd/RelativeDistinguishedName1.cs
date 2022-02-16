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
public partial class RelativeDistinguishedName1
{
    #region Instance Values

    private AttributeType1Code attrTpField;
    private string attrValField;

    /// <remarks />
    public AttributeType1Code AttrTp
    {
        get => this.attrTpField;
        set => this.attrTpField = value;
    }

    /// <remarks />
    public string AttrVal
    {
        get => this.attrValField;
        set => this.attrValField = value;
    }

    #endregion
}