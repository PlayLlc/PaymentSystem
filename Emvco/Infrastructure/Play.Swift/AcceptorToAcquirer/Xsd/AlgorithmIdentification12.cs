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
public partial class AlgorithmIdentification12
{
    #region Instance Values

    private Algorithm8Code algoField;
    private Parameter5 paramField;

    /// <remarks />
    public Algorithm8Code Algo
    {
        get => this.algoField;
        set => this.algoField = value;
    }

    /// <remarks />
    public Parameter5 Param
    {
        get => this.paramField;
        set => this.paramField = value;
    }

    #endregion
}