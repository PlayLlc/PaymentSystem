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
public partial class Header58
{
    #region Instance Values

    private MessageFunction41Code msgFctnField;
    private string prtcolVrsnField;
    private decimal xchgIdField;
    private string reTrnsmssnCntrField;
    private DateTime creDtTmField;
    private GenericIdentification176 initgPtyField;
    private GenericIdentification177 rcptPtyField;
    private Traceability8[] tracbltField;

    /// <remarks />
    public MessageFunction41Code MsgFctn
    {
        get => this.msgFctnField;
        set => this.msgFctnField = value;
    }

    /// <remarks />
    public string PrtcolVrsn
    {
        get => this.prtcolVrsnField;
        set => this.prtcolVrsnField = value;
    }

    /// <remarks />
    public decimal XchgId
    {
        get => this.xchgIdField;
        set => this.xchgIdField = value;
    }

    /// <remarks />
    public string ReTrnsmssnCntr
    {
        get => this.reTrnsmssnCntrField;
        set => this.reTrnsmssnCntrField = value;
    }

    /// <remarks />
    public DateTime CreDtTm
    {
        get => this.creDtTmField;
        set => this.creDtTmField = value;
    }

    /// <remarks />
    public GenericIdentification176 InitgPty
    {
        get => this.initgPtyField;
        set => this.initgPtyField = value;
    }

    /// <remarks />
    public GenericIdentification177 RcptPty
    {
        get => this.rcptPtyField;
        set => this.rcptPtyField = value;
    }

    /// <remarks />
    [XmlElement("Tracblt")]
    public Traceability8[] Tracblt
    {
        get => this.tracbltField;
        set => this.tracbltField = value;
    }

    #endregion
}