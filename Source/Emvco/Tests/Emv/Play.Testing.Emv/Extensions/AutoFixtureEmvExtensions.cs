using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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