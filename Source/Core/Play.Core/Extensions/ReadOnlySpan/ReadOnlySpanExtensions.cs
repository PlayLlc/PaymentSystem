namespace Play.Core.Extensions;

public static partial class ReadOnlySpanExtensions
{
    #region Static Metadata

    private const int _CopyThreshold = 12;
    private const int _StackallocThreshold = 256;

    #endregion
}