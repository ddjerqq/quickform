using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using QuickForm.Common;

namespace QuickForm.Components;

internal sealed class InputEnumSelect<TEnum> : InputBase<TEnum>
    where TEnum : Enum
{
    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var onchange = EventCallback.Factory.CreateBinder<string>(
            this,
            value => CurrentValueAsString = value,
            CurrentValueAsString!,
            culture: null);

        int i = 0;
        builder.OpenElement(i++, "select");
        builder.AddMultipleAttributes(i++, AdditionalAttributes);
        builder.AddAttribute(i++, "class", CssClass);
        builder.AddAttribute(i++, "value", BindConverter.FormatValue(CurrentValueAsString));
        builder.AddAttribute(i++, "onchange", onchange);

        var enumType = Nullable.GetUnderlyingType(typeof(TEnum)) ?? typeof(TEnum);
        foreach (TEnum field in Enum.GetValues(enumType))
        {
            builder.OpenElement(i++, "option");
            builder.AddAttribute(i++, "value", field.ToString());
            builder.AddContent(i++, field.GetDisplayName());
            builder.CloseElement();
        }

        builder.CloseElement();
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