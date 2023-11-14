using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Common;

namespace QuickForm.Components;

internal class QuickFormField<TEntity> : IQuickFormField
    where TEntity : class, new()
{
    private readonly QuickForm<TEntity> _form;

    private RenderFragment? _inputComponentTemplate;
    private RenderFragment? _fieldValidationTemplate;

    public PropertyInfo PropertyInfo { get; }

    internal Type PropertyType => PropertyInfo.PropertyType;

    public IDictionary<string, object> InputComponentAttributes
    {
        get
        {
            var expressionContainer = InputComponentExpressionContainer.Create(this);

            var attributes = new Dictionary<string, object?>
            {
                { "id", EditorId },
                { "Value", Value },
                { "autofocus", true },
                { "required", PropertyInfo.IsRequired() },
                { "class", PropertyInfo.InputClass() ?? _form.CssClassProvider?.Input(this) },
                { "ValueExpression", expressionContainer.ValueExpression }
            };

            if (PropertyInfo.IsEditable())
            {
                attributes["ValueChanged"] = expressionContainer.ValueChanged;
            }
            else
            {
                attributes["disabled"] = "true";
                attributes["readonly"] = "true";
            }

            if (!string.IsNullOrEmpty(PropertyInfo.DataListName()))
                attributes["list"] = PropertyInfo.DataListName();

            if (!string.IsNullOrEmpty(PropertyInfo.Placeholder()))
                attributes["placeholder"] = PropertyInfo.Placeholder();

            if (PropertyInfo.IsCheckbox())
            {
                attributes["role"] = "switch";
            }
            else if (PropertyInfo.RangeAttribute() is { Minimum: var min, Maximum: var max })
            {
                attributes["min"] = min;
                attributes["max"] = max;
            }

            if (PropertyInfo.GetHtmlInputType() is { } htmlInputType)
                attributes["type"] = htmlInputType;

            return attributes!;
        }
    }

    protected QuickFormField(QuickForm<TEntity> form, PropertyInfo propertyInfo)
    {
        _form = form;
        PropertyInfo = propertyInfo;
    }

    internal event EventHandler? ValueChanged;

    internal string EditorId => _form.BaseEditorId + '_' + PropertyInfo.Name;

    internal TEntity? Owner => _form.Model;

    internal object? Value
    {
        get => PropertyInfo.GetValue(Owner);
        set
        {
            if (PropertyInfo.SetMethod is not null && !Equals(Value, value))
            {
                PropertyInfo.SetValue(Owner, value);
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    internal RenderFragment InputComponentTemplate
    {
        get
        {
            return _inputComponentTemplate ??= builder =>
            {
                var inputComponentType = PropertyInfo.GetInputComponentType();

                builder.OpenComponent(0, inputComponentType);
                builder.AddMultipleAttributes(1, InputComponentAttributes);
                builder.CloseComponent();
            };
        }
    }

    internal RenderFragment ValidationMessage
    {
        get
        {
            return _fieldValidationTemplate ??= builder =>
            {
                var expressionContainer = ValidationMessageExpressionContainer.Create(this);

                builder.OpenComponent(0, typeof(ValidationMessage<>).MakeGenericType(PropertyType));
                builder.AddAttribute(1, "For", expressionContainer.Lambda);
                builder.CloseComponent();
            };
        }
    }

    internal static IEnumerable<QuickFormField<TEntity>> FromForm(QuickForm<TEntity> form)
    {
        return typeof(TEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(prop => prop.GetCustomAttribute<NotMappedAttribute>() is null)
            .Select(prop => new QuickFormField<TEntity>(form, prop));
    }
}