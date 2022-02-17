using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <remarks />
[GeneratedCode("xsd", "4.8.3928.0")]
[Serializable()]
[DebuggerStepThrough()]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:iso:std:iso:20022:tech:xsd:caaa.002.001.09")]
public partial class Action10
{
    #region Instance Values

    private ActionType9Code actnTpField;
    private ActionMessage7 msgToPresField;

    /// <remarks />
    public ActionType9Code ActnTp
    {
        get => this.actnTpField;
        set => this.actnTpField = value;
    }

    /// <remarks />
    public ActionMessage7 MsgToPres
    {
        get => this.msgToPresField;
        set => this.msgToPresField = value;
    }

    #endregion
}