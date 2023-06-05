using System.Linq.Expressions;

namespace Pattern.Utility
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> OrderByCustom<TEntity>(this IQueryable<TEntity> query, string sortBy, string sortOrder)
        {
            var type = typeof(TEntity);
            var expression2 = Expression.Parameter(type, "t");
            var property = type.GetProperty(sortBy);
            var expression1 = Expression.MakeMemberAccess(expression2, property);
            var lambda = Expression.Lambda(expression1, expression2);

            var result = Expression.Call(
                typeof(IQueryable),
                sortOrder == "desc" ? "OrderByDesc" : "OrderBy",
                new Type[] {type, property.PropertyType},
                query.Expression,
                Expression.Quote(lambda)
                );

            return query.Provider.CreateQuery<TEntity>(result);
        }
    }
}
