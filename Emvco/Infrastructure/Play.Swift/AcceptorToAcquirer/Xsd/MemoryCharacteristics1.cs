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
public partial class MemoryCharacteristics1
{
    #region Instance Values

    private string idField;
    private decimal ttlSzField;
    private decimal freeSzField;
    private MemoryUnit1Code unitField;

    /// <remarks />
    public string Id
    {
        get => this.idField;
        set => this.idField = value;
    }

    /// <remarks />
    public decimal TtlSz
    {
        get => this.ttlSzField;
        set => this.ttlSzField = value;
    }

    /// <remarks />
    public decimal FreeSz
    {
        get => this.freeSzField;
        set => this.freeSzField = value;
    }

    /// <remarks />
    public MemoryUnit1Code Unit
    {
        get => this.unitField;
        set => this.unitField = value;
    }

    #endregion
}