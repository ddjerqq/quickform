using System.Linq.Expressions;
using QuickForm.Components;

namespace QuickForm.Common;

internal record ValidationMessageExpressionContainer(LambdaExpression Lambda)
{
    public static ValidationMessageExpressionContainer Create<TEntity>(QuickFormField<TEntity> formField)
    {
        // () => Owner.Property
        var access = Expression.Property(Expression.Constant(formField.Owner, typeof(TEntity)), formField.PropertyInfo);
        var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(formField.PropertyType), access);

        return new ValidationMessageExpressionContainer(lambda);
    }
}
