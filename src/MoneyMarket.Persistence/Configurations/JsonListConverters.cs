using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace MoneyMarket.Persistence.Configurations
{
    internal static class JsonListConverters
    {
        private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

        // IReadOnlyList<string> <-> nvarchar(max) (JSON)
        public static readonly ValueConverter<IReadOnlyList<string>, string> ReadOnlyStringListToJsonConverter
            = new(
                v => JsonSerializer.Serialize(v ?? Array.Empty<string>(), _json),
                v => (IReadOnlyList<string>)(JsonSerializer.Deserialize<List<string>>(v, _json) ?? new List<string>())
              );

        // EF needs expression-tree-safe lambdas (no ?. operators)
        public static readonly ValueComparer<IReadOnlyList<string>> ReadOnlyStringListComparer
            = new(
                // Equals
                (a, b) => (a == null && b == null) || (a != null && b != null && a.SequenceEqual(b)),
                // Hash
                v => v == null
                    ? 0
                    : v.Aggregate(0, (h, s) => HashCode.Combine(h, (s == null ? 0 : s.GetHashCode()))),
                // Snapshot (deep copy to break reference tracking)
                v => (IReadOnlyList<string>)(v != null ? v.ToList().AsReadOnly() : new List<string>().AsReadOnly())
              );
    }
}
