using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Common;

namespace QuickForm.Components;

internal sealed class QuickFormField<TEntity>
{
    // the parent form
    private readonly QuickForm<TEntity> form;

    private RenderFragment? inputComponentTemplate;
    private RenderFragment? fieldValidationTemplate;

    private QuickFormField(QuickForm<TEntity> form, PropertyInfo propertyInfo)
    {
        this.form = form;
        this.PropertyInfo = propertyInfo;
    }

    public event EventHandler? ValueChanged;

    public string EditorId => this.form.BaseEditorId + '_' + this.PropertyInfo.Name;

    public PropertyInfo PropertyInfo { get; }

    public Type PropertyType => this.PropertyInfo.PropertyType;

    public TEntity Owner => this.form.Model;

    public object? Value
    {
        get => this.PropertyInfo.GetValue(this.Owner);
        set
        {
            if (this.PropertyInfo.SetMethod is not null && !Equals(this.Value, value))
            {
                this.PropertyInfo.SetValue(this.Owner, value);
                this.ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public RenderFragment InputComponentTemplate
    {
        get
        {
            return this.inputComponentTemplate ??= builder =>
            {
                var inputComponentType = this.PropertyInfo.GetInputComponentType();

                builder.OpenComponent(0, inputComponentType);
                builder.AddMultipleAttributes(1, this.InputComponentAttributes);
                builder.CloseComponent();
            };
        }
    }

    public RenderFragment ValidationMessage
    {
        get
        {
            return this.fieldValidationTemplate ??= builder =>
            {
                var expressionContainer = ValidationMessageExpressionContainer.Create(this);

                builder.OpenComponent(0, typeof(ValidationMessage<>).MakeGenericType(this.PropertyType));
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
                { "id", this.EditorId },
                { "Value", this.Value },
                { "ValueExpression", expressionContainer.ValueExpression },
                { "autofocus", true },
                { "required", this.PropertyInfo.IsRequired() },
            };

            if (this.PropertyInfo.IsEditable())
            {
                attributes["ValueChanged"] = expressionContainer.ValueChanged;
            }
            else
            {
                attributes["disabled"] = "true";
                attributes["readonly"] = "true";
            }

            if (!string.IsNullOrEmpty(this.PropertyInfo.DataListName()))
            {
                attributes["list"] = this.PropertyInfo.DataListName();
            }

            if (!string.IsNullOrEmpty(this.PropertyInfo.Placeholder()))
            {
                attributes["placeholder"] = this.PropertyInfo.Placeholder();
            }

            if (this.PropertyInfo.IsCheckbox())
            {
                attributes["class"] = "form-check-input";
                attributes["role"] = "switch";
            }
            else if (this.PropertyInfo.RangeAttribute() is not null)
            {
                attributes["class"] = "form-range";

                if (this.PropertyType == typeof(short)
                    || this.PropertyType == typeof(int)
                    || this.PropertyType == typeof(long))
                {
                    attributes["step"] = "1";
                }
                else
                {
                    attributes["step"] = "0.1";
                }

                attributes["min"] = this.PropertyInfo.RangeAttribute()!.Minimum;
                attributes["max"] = this.PropertyInfo.RangeAttribute()!.Maximum;
            }
            else
            {
                attributes["class"] = "form-control";
            }

            if (!string.IsNullOrEmpty(this.PropertyInfo.GetHtmlInputType()))
            {
                attributes["type"] = this.PropertyInfo.GetHtmlInputType();
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