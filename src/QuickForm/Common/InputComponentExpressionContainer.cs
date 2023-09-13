using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using QuickForm.Components;

namespace QuickForm.Common;

internal record InputComponentExpressionContainer(LambdaExpression ValueExpression, object? ValueChanged)
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo EventCallbackFactoryCreate = GetEventCallbackFactoryCreate();

    private static MethodInfo GetEventCallbackFactoryCreate()
    {
        return typeof(EventCallbackFactory)
            .GetMethods()
            .Single(m =>
            {
                if (m.Name != "Create" || !m.IsPublic || m.IsStatic || !m.IsGenericMethod)
                    return false;

                var generic = m.GetGenericArguments();
                if (generic.Length != 1)
                    return false;

                var args = m.GetParameters();
                return args.Length == 2
                       && args[0].ParameterType == typeof(object)
                       && args[1].ParameterType.IsGenericType
                       && args[1].ParameterType.GetGenericTypeDefinition() == typeof(Action<>);
            });
    }

    public static InputComponentExpressionContainer Create<TEntity>(QuickFormField<TEntity> formField)
        where TEntity : class, new()
    {
        // () => Owner.Property
        var access = Expression.Property(
            Expression.Constant(formField.Owner, typeof(TEntity)),
            formField.PropertyInfo);
        var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(formField.PropertyType), access);

        // Create(object receiver, Action<object>) callback
        var method = EventCallbackFactoryCreate.MakeGenericMethod(formField.PropertyType);

        // value => Field.Value = value;
        var changeHandlerParameter = Expression.Parameter(formField.PropertyType);

        var body = Expression.Assign(
            Expression.Property(Expression.Constant(formField), nameof(formField.Value)),
            Expression.Convert(changeHandlerParameter, typeof(object)));

        var changeHandlerLambda = Expression.Lambda(
            typeof(Action<>).MakeGenericType(formField.PropertyType),
            body,
            changeHandlerParameter);

        var changeHandler = method.Invoke(
            EventCallback.Factory,
            new object[] { formField, changeHandlerLambda.Compile() });

        return new InputComponentExpressionContainer(lambda, changeHandler);
    }
}