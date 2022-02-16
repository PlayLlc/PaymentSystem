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
public partial class Recipient8Choice
{
    #region Instance Values

    private object itemField;

    /// <remarks />
    [XmlElement("KEK", typeof(KEK7))]
    [XmlElement("KeyIdr", typeof(KEKIdentifier2))]
    [XmlElement("KeyTrnsprt", typeof(KeyTransport5))]
    public object Item
    {
        get => this.itemField;
        set => this.itemField = value;
    }

    #endregion
}