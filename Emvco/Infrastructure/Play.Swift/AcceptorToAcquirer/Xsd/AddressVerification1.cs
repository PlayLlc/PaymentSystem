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
public partial class AddressVerification1
{
    #region Instance Values

    private string adrDgtsField;
    private string pstlCdDgtsField;

    /// <remarks />
    public string AdrDgts
    {
        get => this.adrDgtsField;
        set => this.adrDgtsField = value;
    }

    /// <remarks />
    public string PstlCdDgts
    {
        get => this.pstlCdDgtsField;
        set => this.pstlCdDgtsField = value;
    }

    #endregion
}