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
public partial class DisplayCapabilities4
{
    #region Instance Values

    private UserInterface4Code[] dstnField;
    private OutputFormat1Code[] avlblFrmtField;
    private decimal nbOfLinesField;
    private bool nbOfLinesFieldSpecified;
    private decimal lineWidthField;
    private bool lineWidthFieldSpecified;
    private string[] avlblLangField;

    /// <remarks />
    [XmlElement("Dstn")]
    public UserInterface4Code[] Dstn
    {
        get => this.dstnField;
        set => this.dstnField = value;
    }

    /// <remarks />
    [XmlElement("AvlblFrmt")]
    public OutputFormat1Code[] AvlblFrmt
    {
        get => this.avlblFrmtField;
        set => this.avlblFrmtField = value;
    }

    /// <remarks />
    public decimal NbOfLines
    {
        get => this.nbOfLinesField;
        set => this.nbOfLinesField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool NbOfLinesSpecified
    {
        get => this.nbOfLinesFieldSpecified;
        set => this.nbOfLinesFieldSpecified = value;
    }

    /// <remarks />
    public decimal LineWidth
    {
        get => this.lineWidthField;
        set => this.lineWidthField = value;
    }

    /// <remarks />
    [XmlIgnore()]
    public bool LineWidthSpecified
    {
        get => this.lineWidthFieldSpecified;
        set => this.lineWidthFieldSpecified = value;
    }

    /// <remarks />
    [XmlElement("AvlblLang")]
    public string[] AvlblLang
    {
        get => this.avlblLangField;
        set => this.avlblLangField = value;
    }

    #endregion
}