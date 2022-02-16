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
public partial class Parameter5
{
    #region Instance Values

    private Algorithm11Code dgstAlgoField;
    private bool dgstAlgoFieldSpecified;

    /// <remarks />
    public Algorithm11Code DgstAlgo
    {
        get => this.dgstAlgoField;
        set => this.dgstAlgoField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool DgstAlgoSpecified
    {
        get => this.dgstAlgoFieldSpecified;
        set => this.dgstAlgoFieldSpecified = value;
    }

    #endregion
}