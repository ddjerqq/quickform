using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using QuickForm.Common;

namespace QuickForm.Components;

[Obsolete("This component is not ready for production use. it is borderline broken.")]
internal sealed class InputEnumRadio<TEnum> : InputBase<TEnum>
    where TEnum : Enum
{
    [Parameter]
    public IQuickFormField Field { get; set; } = default!;

    [Parameter]
    public IQuickFormFieldCssClassProvider FieldCssClassProvider { get; set; } = default!;

    [Parameter]
    public CustomValidationCssClassProvider ValidationCssClassProvider { get; set; } = default!;

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var enumType = Nullable.GetUnderlyingType(typeof(TEnum)) ?? typeof(TEnum);
        var onchange = EventCallback.Factory.CreateBinder<string>(
            this,
            value =>
            {
                CurrentValueAsString = value;
            },
            CurrentValueAsString!,
            culture: null);

        int i = 0;

        builder.OpenElement(++i, "div");

        var @class = string.Join(' ', CssClass, FieldCssClassProvider.Editor(Field) ?? Field.PropertyInfo.EditorClass());
        builder.AddAttribute(++i, "class", @class);

        builder.OpenElement(++i, "label");
        builder.AddAttribute(++i, "class", FieldCssClassProvider.Label(Field) ?? Field.PropertyInfo.LabelClass());
        builder.AddContent(++i, Field.PropertyInfo.DisplayName());
        builder.CloseElement();

        foreach (TEnum field in Enum.GetValues(enumType))
        {
            builder.OpenComponent<InputRadio<TEnum>>(++i);
            builder.AddAttribute(++i, "Value", field.ToString());
            builder.CloseComponent();

            builder.AddContent(++i, field.GetDisplayName());

            builder.OpenElement(++i, "br");
            builder.CloseElement();
        }

        builder.CloseComponent();
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, out TEnum result, out string validationErrorMessage)
    {
        result = default!;
        validationErrorMessage = string.Empty;

        if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out TEnum? parsedValue))
        {
            // parsedValue is never null here, because TryConvertTo returns false if the conversion fails
            result = parsedValue!;
            return true;
        }

        validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";

        return false;
    }
}