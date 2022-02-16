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
public partial class AlgorithmIdentification29
{
    #region Instance Values

    private Algorithm24Code algoField;
    private Parameter12 paramField;

    /// <remarks />
    public Algorithm24Code Algo
    {
        get => this.algoField;
        set => this.algoField = value;
    }

    /// <remarks />
    public Parameter12 Param
    {
        get => this.paramField;
        set => this.paramField = value;
    }

    #endregion
}