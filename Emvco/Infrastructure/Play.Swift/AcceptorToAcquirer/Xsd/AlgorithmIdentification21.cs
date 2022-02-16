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
public partial class AlgorithmIdentification21
{
    #region Instance Values

    private Algorithm16Code algoField;

    /// <remarks />
    public Algorithm16Code Algo
    {
        get => this.algoField;
        set => this.algoField = value;
    }

    #endregion
}