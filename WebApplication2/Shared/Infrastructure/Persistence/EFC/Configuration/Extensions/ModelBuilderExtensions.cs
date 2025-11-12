using Humanizer; // <-- pluralize / underscore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    /// Convierte tablas/columnas/keys/FKs/índices a snake_case y pluraliza nombres de tablas.
    public static void UseSnakeCaseNamingConvention(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            // Tabla (plural + snake_case)
            var tableName = entity.GetTableName();
            if (!string.IsNullOrWhiteSpace(tableName))
                entity.SetTableName(tableName!.Pluralize(inputIsKnownToBeSingular: false).Underscore());

            // Columnas (EF Core 8: requiere StoreObjectIdentifier)
            foreach (var property in entity.GetProperties())
            {
                var store = StoreObjectIdentifier.Create(property.DeclaringEntityType, StoreObjectType.Table);
                if (store.HasValue)
                {
                    var current = property.GetColumnName(store.Value);
                    if (!string.IsNullOrWhiteSpace(current))
                        property.SetColumnName(current!.Underscore());
                }
            }

            // Keys
            foreach (var key in entity.GetKeys())
            {
                var name = key.GetName();
                if (!string.IsNullOrWhiteSpace(name))
                    key.SetName(name!.Underscore());
            }

            // FKs
            foreach (var fk in entity.GetForeignKeys())
            {
                var name = fk.GetConstraintName();
                if (!string.IsNullOrWhiteSpace(name))
                    fk.SetConstraintName(name!.Underscore());
            }

            // Índices
            foreach (var index in entity.GetIndexes())
            {
                var name = index.GetDatabaseName();
                if (!string.IsNullOrWhiteSpace(name))
                    index.SetDatabaseName(name!.Underscore());
            }
        }
    }
}
