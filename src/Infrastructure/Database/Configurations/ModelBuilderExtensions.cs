using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace WalmgateIdentity.Infrastructure.Database.Configurations;

public static class ModelBuilderExtensions
{
    public static ModelBuilder HasGlobalQueryFilter<TInterface>(this ModelBuilder modelBuilder,
        Expression<Func<TInterface, bool>> filterExpression)
    {
        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(entity => typeof(TInterface).IsAssignableFrom(entity.ClrType));

        foreach (var entity in entities)
        {
            var parameter = Expression.Parameter(entity.ClrType);
            var expression = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), parameter, filterExpression.Body);
            modelBuilder.Entity(entity.ClrType).HasQueryFilter(Expression.Lambda(expression, parameter));
        }

        return modelBuilder;   
    }
}
