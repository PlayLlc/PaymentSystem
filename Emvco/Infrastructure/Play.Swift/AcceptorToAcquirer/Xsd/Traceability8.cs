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
public partial class Traceability8
{
    #region Instance Values

    private GenericIdentification177 rlayIdField;
    private string prtcolNmField;
    private string prtcolVrsnField;
    private DateTime tracDtTmInField;
    private DateTime tracDtTmOutField;

    /// <remarks />
    public GenericIdentification177 RlayId
    {
        get => this.rlayIdField;
        set => this.rlayIdField = value;
    }

    /// <remarks />
    public string PrtcolNm
    {
        get => this.prtcolNmField;
        set => this.prtcolNmField = value;
    }

    /// <remarks />
    public string PrtcolVrsn
    {
        get => this.prtcolVrsnField;
        set => this.prtcolVrsnField = value;
    }

    /// <remarks />
    public DateTime TracDtTmIn
    {
        get => this.tracDtTmInField;
        set => this.tracDtTmInField = value;
    }

    /// <remarks />
    public DateTime TracDtTmOut
    {
        get => this.tracDtTmOutField;
        set => this.tracDtTmOutField = value;
    }

    #endregion
}