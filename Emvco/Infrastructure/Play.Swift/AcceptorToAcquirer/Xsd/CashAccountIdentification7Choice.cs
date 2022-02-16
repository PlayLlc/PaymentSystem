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
public partial class CashAccountIdentification7Choice
{
    #region Instance Values

    private object itemField;
    private ItemChoiceType itemElementNameField;

    /// <remarks />
    [XmlElement("BBAN", typeof(string))]
    [XmlElement("DmstAcct", typeof(SimpleIdentificationInformation4))]
    [XmlElement("IBAN", typeof(string))]
    [XmlElement("UPIC", typeof(string))]
    [XmlChoiceIdentifier("ItemElementName")]
    public object Item
    {
        get => this.itemField;
        set => this.itemField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public ItemChoiceType ItemElementName
    {
        get => this.itemElementNameField;
        set => this.itemElementNameField = value;
    }

    #endregion
}