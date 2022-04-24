namespace Play.Core.Extensions;

public static partial class ReadOnlySpanExtensions
{
    #region Static Metadata

    private const int CopyThreshold = 12;
    private const int StackallocThreshold = 256;

    #endregion
}