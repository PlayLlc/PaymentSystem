namespace Play.Core.Specifications;

public partial class Specs
{
    public static class ByteArray
    {
        #region Static Metadata

        /// <summary>
        ///     The max byte count that would be acceptable to use stacalloc. If the byte count is greater
        ///     than this then a shared array pool of some sort should be used
        /// </summary>
        public const int StackAllocateCeiling = 256;

        #endregion
    }
}