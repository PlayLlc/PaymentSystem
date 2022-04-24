using AutoFixture.Kernel;

using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;

namespace Play.Testing.Emv
{
    public class DirectoryEntryBuilder : SpecimenBuilder
    {
        #region Static Metadata

        public static readonly SpecimenBuilderId Id = new(nameof(DirectoryEntryBuilder));

        private static readonly List<byte[]> _RawDirectoryEntries = new()
        {
            new byte[]
            {
                0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x03, 0x10,
                0x10, 0x87, 0x01, 0x01, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03,
                0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53
            },
            new byte[]
            {
                0x61, 0x1A, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08,
                0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03, 0x42, 0x03,
                0x40, 0x81, 0x38, 0x5F, 0x55, 0x02, 0x55, 0x53
            }
        };

        #endregion

        #region Instance Members

        public override SpecimenBuilderId GetId() => Id;

        /// <exception cref="DataElementParsingException"></exception>
        public override object Create(object request, ISpecimenContext context)
        {
            Type? type = request as Type;

            if (type == null)
                return new NoSpecimen();

            if (type != typeof(DirectoryEntry))
                return new NoSpecimen();

            return DirectoryEntry.Decode(_RawDirectoryEntries.ElementAt(_Random.Next(0, _RawDirectoryEntries.Count - 1)));
        }

        #endregion
    }
}