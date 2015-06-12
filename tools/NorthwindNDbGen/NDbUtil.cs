using System.Linq;
using NDatabase.Api;

namespace NDatabase.Northwind.Generator
{
    internal static class NDbUtil
    {
        public static IObjectSet<TItem> GetAllInstances<TItem>(IOdb odb)
        {
            var query = odb.Query<TItem>();
            
            return query.Execute<TItem>();
        }

        public static TItem GetByStringID<TItem>(IOdb odb, string idField, string id)
        {
            var query = odb.Query<TItem>();
            query.Descend(idField).Constrain(id);
            var result = query.Execute<TItem>();

            return result.FirstOrDefault();
        }

        public static TItem GetByNumericalID<TItem>(IOdb odb, string idField, long id)
        {
            var query = odb.Query<TItem>();
            query.Descend(idField).Constrain(id);
            var result = query.Execute<TItem>();
            
            return result.FirstOrDefault();
        }
    }
}
