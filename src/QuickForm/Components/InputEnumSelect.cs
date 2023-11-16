using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

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
        builder.OpenElement(++i, "select");
        builder.AddMultipleAttributes(++i, AdditionalAttributes);
        builder.AddAttribute(++i, "class", CssClass);
        builder.AddAttribute(++i, "value", BindConverter.FormatValue(CurrentValueAsString));
        builder.AddAttribute(++i, "onchange", onchange);

        // Add an option element per enum value
        var enumType = GetEnumType();
        foreach (TEnum value in Enum.GetValues(enumType))
        {
            builder.OpenElement(++i, "option");
            builder.AddAttribute(++i, "value", value.ToString());
            builder.AddContent(++i, GetDisplayName(value));
            builder.CloseElement();
        }

        builder.CloseElement(); // close the select element
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, out TEnum result, out string validationErrorMessage)
    {
        // Let's Blazor convert the value for us 😊
        if (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out TEnum? parsedValue))
        {
            // parsedValue is never null here, because TryConvertTo returns false if the conversion fails
            result = parsedValue!;
            validationErrorMessage = string.Empty;
            return true;
        }

        // The value is invalid => set the error message
        result = default!;
        validationErrorMessage = $"The {FieldIdentifier.FieldName} field is not valid.";
        return false;
    }

    // Get the display text for an enum value:
    // - Use the DisplayAttribute if set on the enum member, or DisplayNameAttribute if set
    // - Fallback on Humanizer to decamelize the enum member name
    private static string GetDisplayName(TEnum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        // Read the Display attribute name
        var member = value.GetType().GetMember(value.ToString()).First();

        var displayName = member.GetCustomAttribute<DisplayAttribute>()?.GetName();
        displayName ??= member.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        displayName ??= value.ToString();

        var regex = new Regex(@"([a-z])([A-Z])");
        displayName = regex.Replace(displayName, "$1 $2");

        return displayName;
    }

    // Get the actual enum type. Unwrap Nullable<T> if needed
    // MyEnum  => MyEnum
    // MyEnum? => MyEnum
    private static Type GetEnumType()
    {
        var nullableType = Nullable.GetUnderlyingType(typeof(TEnum));
        return nullableType ?? typeof(TEnum);
    }
}