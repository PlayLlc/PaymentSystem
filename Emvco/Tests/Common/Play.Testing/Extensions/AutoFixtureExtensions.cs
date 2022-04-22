using System.Collections.Immutable;

using AutoFixture;

namespace Play.Testing.Extensions
{
    public static class AutoFixtureExtensions
    {
        #region Instance Members

        public static void RegisterCollections<T, K>(this IFixture fixture) where T : notnull
        {
            List<KeyValuePair<T, K>> keyValuePairs = fixture.CreateMany<KeyValuePair<T, K>>().ToList();
            fixture.Register(() => keyValuePairs.ToDictionary(a => a.Key, b => b.Value));
            fixture.Register(() => keyValuePairs.ToImmutableDictionary(a => a.Key, b => b.Value));

            if (!typeof(T).IsAssignableFrom(typeof(IComparable<T>)))
            {
                fixture.Register(() => keyValuePairs.ToImmutableSortedDictionary(a => a.Key, b => b.Value));
                fixture.Register(() => new SortedDictionary<T, K>(keyValuePairs!.ToDictionary(a => a.Key, b => b.Value)));
                fixture.Register(() => new SortedList<T, K>(keyValuePairs!.ToDictionary(a => a.Key, b => b.Value)));
            }
        }

        public static void RegisterCollections<T>(this IFixture fixture)
        {
            fixture.Register(() => fixture.CreateMany<T>().ToArray());
            fixture.Register(() => fixture.CreateMany<T>().ToList());
            fixture.Register(() => fixture.CreateMany<T>().AsEnumerable());
            fixture.Register(() => fixture.CreateMany<T>().ToHashSet());
            fixture.Register(fixture.CreateMany<T>);

            if (!typeof(T).IsAssignableFrom(typeof(IComparable<T>)))
                fixture.Register(fixture.CreateMany<T>);
        }

        #endregion
    }
}