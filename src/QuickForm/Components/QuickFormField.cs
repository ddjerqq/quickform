using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Internal;

namespace QuickForm.Components;

internal sealed class QuickFormField<TEntity> : IQuickFormField
    where TEntity : class, new()
{
    private readonly QuickForm<TEntity> _form;

    public string EditorId => $"{_form.BaseEditorId}_{PropertyInfo.Name}";

    public string DisplayName => PropertyInfo.DisplayName();

    public string? Description => PropertyInfo.Description();

    public string? ValidFeedback => PropertyInfo.ValidFeedbackText();

    public RenderFragment<string> InputComponent
    {
        get
        {
            return @class =>
            {
                return builder =>
                {
                    var inputComponentType = PropertyInfo.GetInputComponentType();

                    builder.OpenComponent(0, inputComponentType);
                    builder.AddMultipleAttributes(1, InputComponentAttributes);
                    builder.AddAttribute(2, "class", @class);
                    builder.CloseComponent();
                };
            };
        }
    }

    public RenderFragment<string> ValidationMessages
    {
        get
        {
            return @class =>
            {
                return builder =>
                {
                    var expressionContainer = ValidationMessageExpressionContainer.Create(this);

                    builder.OpenComponent(0, typeof(ValidationMessage<>).MakeGenericType(PropertyType));
                    builder.AddAttribute(1, "For", expressionContainer.Lambda);
                    builder.AddAttribute(2, "class", @class);
                    builder.CloseComponent();
                };
            };
        }
    }

    internal PropertyInfo PropertyInfo { get; }

    internal Type PropertyType => PropertyInfo.PropertyType;

    internal IDictionary<string, object> InputComponentAttributes
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
                { "ValueExpression", expressionContainer.ValueExpression },
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

    private QuickFormField(QuickForm<TEntity> form, PropertyInfo propertyInfo)
    {
        _form = form;
        PropertyInfo = propertyInfo;
    }

    internal event EventHandler? ValueChanged;

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

    internal static IEnumerable<QuickFormField<TEntity>> FromForm(QuickForm<TEntity> form)
    {
        return typeof(TEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(prop => prop.GetCustomAttribute<NotMappedAttribute>() is null)
            .Select(prop => new QuickFormField<TEntity>(form, prop));
    }
}