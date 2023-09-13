using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Common;

namespace QuickForm.Components;

internal sealed class QuickFormField<TEntity>
    where TEntity : class, new()
{
    // the parent form
    private readonly QuickForm<TEntity> _form;

    private RenderFragment? _inputComponentTemplate;
    private RenderFragment? _fieldValidationTemplate;

    private QuickFormField(QuickForm<TEntity> form, PropertyInfo propertyInfo)
    {
        _form = form;
        PropertyInfo = propertyInfo;
    }

    public event EventHandler? ValueChanged;

    public string EditorId => _form.BaseEditorId + '_' + PropertyInfo.Name;

    public PropertyInfo PropertyInfo { get; }

    public Type PropertyType => PropertyInfo.PropertyType;

    public TEntity Owner => _form.Model;

    public object? Value
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

    public RenderFragment InputComponentTemplate
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

    public RenderFragment ValidationMessage
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

    private IDictionary<string, object> InputComponentAttributes
    {
        get
        {
            var expressionContainer = InputComponentExpressionContainer.Create(this);

            var attributes = new Dictionary<string, object?>
            {
                { "id", EditorId },
                { "Value", Value },
                { "ValueExpression", expressionContainer.ValueExpression },
                { "autofocus", true },
                { "required", PropertyInfo.IsRequired() },
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
            {
                attributes["list"] = PropertyInfo.DataListName();
            }

            if (!string.IsNullOrEmpty(PropertyInfo.Placeholder()))
            {
                attributes["placeholder"] = PropertyInfo.Placeholder();
            }

            if (PropertyInfo.IsCheckbox())
            {
                attributes["class"] = "form-check-input";
                attributes["role"] = "switch";
            }
            else if (PropertyInfo.RangeAttribute() is not null)
            {
                attributes["class"] = "form-range";

                if (PropertyType == typeof(short)
                    || PropertyType == typeof(int)
                    || PropertyType == typeof(long))
                {
                    attributes["step"] = "1";
                }
                else
                {
                    attributes["step"] = "0.1";
                }

                attributes["min"] = PropertyInfo.RangeAttribute()!.Minimum;
                attributes["max"] = PropertyInfo.RangeAttribute()!.Maximum;
            }
            else
            {
                attributes["class"] = "form-control";
            }

            if (!string.IsNullOrEmpty(PropertyInfo.GetHtmlInputType()))
            {
                attributes["type"] = PropertyInfo.GetHtmlInputType();
            }

            return attributes!;
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