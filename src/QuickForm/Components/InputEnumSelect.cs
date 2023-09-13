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
{
    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var onchange = EventCallback.Factory.CreateBinder<string>(
            this,
            value => CurrentValueAsString = value,
            CurrentValueAsString!,
            culture: null);

        builder.OpenElement(0, "select");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "class", CssClass);
        builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValueAsString));
        builder.AddAttribute(4, "onchange", onchange);

        // Add an option element per enum value
        var enumType = GetEnumType();
        foreach (TEnum value in Enum.GetValues(enumType))
        {
            builder.OpenElement(5, "option");
            builder.AddAttribute(6, "value", value.ToString());
            builder.AddContent(7, GetDisplayName(value));
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

        // Map null/empty value to null if the bound object is nullable
        if (string.IsNullOrEmpty(value))
        {
            var nullableType = Nullable.GetUnderlyingType(typeof(TEnum));
            if (nullableType != null)
            {
                result = default!;
                validationErrorMessage = string.Empty;
                return true;
            }
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
        if (value is null) return string.Empty;

        // Read the Display attribute name
        var member = value.GetType().GetMember(value.ToString()!).First();

        var displayName = member.GetCustomAttribute<DisplayAttribute>()?.GetName();
        displayName ??= member.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        displayName ??= value.ToString()!;

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

        if (nullableType != null)
            return nullableType;

        return typeof(TEnum);
    }
}