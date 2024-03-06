using System.Linq.Expressions;
using QuickForm.Components;

namespace QuickForm.Internal;

internal record ValidationMessageExpressionContainer(LambdaExpression Lambda)
{
    internal static ValidationMessageExpressionContainer Create<TEntity>(QuickFormField<TEntity> formField)
        where TEntity : class, new()
    {
        // () => Owner.Property
        var access = Expression.Property(Expression.Constant(formField.Owner, typeof(TEntity)), formField.PropertyInfo);
        var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(formField.PropertyType), access);

        return new ValidationMessageExpressionContainer(lambda);
    }
}
