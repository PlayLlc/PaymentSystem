using System.CodeDom.Compiler;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.001.001.09", IncludeInSchema = false)]
public enum ItemChoiceType
{
    /// <remarks />
    BBAN,

    /// <remarks />
    DmstAcct,

    /// <remarks />
    IBAN,

    /// <remarks />
    UPIC
}