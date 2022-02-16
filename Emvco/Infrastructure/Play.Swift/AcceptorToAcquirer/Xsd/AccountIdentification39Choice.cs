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
public partial class AccountIdentification39Choice
{
    #region Instance Values

    private string itemField;
    private ItemChoiceType1 itemElementNameField;

    /// <remarks />
    [XmlElement("BBAN", typeof(string))]
    [XmlElement("Card", typeof(string))]
    [XmlElement("Dmst", typeof(string))]
    [XmlElement("EMail", typeof(string))]
    [XmlElement("IBAN", typeof(string))]
    [XmlElement("MSISDN", typeof(string))]
    [XmlElement("Othr", typeof(string))]
    [XmlElement("UPIC", typeof(string))]
    [XmlChoiceIdentifier("ItemElementName")]
    public string Item
    {
        get => this.itemField;
        set => this.itemField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public ItemChoiceType1 ItemElementName
    {
        get => this.itemElementNameField;
        set => this.itemElementNameField = value;
    }

    #endregion
}