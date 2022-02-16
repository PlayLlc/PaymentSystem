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
public partial class Acquirer10
{
    #region Instance Values

    private GenericIdentification177 idField;
    private string paramsVrsnField;

    /// <remarks />
    public GenericIdentification177 Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public string ParamsVrsn
    {
        get => this.paramsVrsnField;
        set => this.paramsVrsnField = value;
    }

    #endregion
}