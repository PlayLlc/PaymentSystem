using AutoFixture;

using Play.Ber.DataObjects;

namespace Play.Testing.Emv.Extensions
{
    public static class AutoFixtureEmvExtensions
    {
        #region Instance Members

        public static void RegisterSetOf<T>(this IFixture fixture) where T : IEncodeBerDataObjects, IRetrieveBerDataObjectMetadata
        {
            fixture.Register(() => new SetOf<T>(fixture.CreateMany<T>().ToArray()));
        }

        #endregion
    }
}