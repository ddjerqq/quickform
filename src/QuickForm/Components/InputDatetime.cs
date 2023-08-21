using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace QuickForm.Components;

internal class InputDateTime<TValue> : InputDate<TValue>
{
    private const string DateFormat = "yyyy-MM-ddTHH:mm";

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var onchange = EventCallback.Factory.CreateBinder<string>(
            this,
            value => this.CurrentValueAsString = value,
            this.CurrentValueAsString!);

        builder.OpenElement(0, "input");
        builder.AddMultipleAttributes(1, this.AdditionalAttributes);
        builder.AddAttribute(2, "type", "datetime-local");
        builder.AddAttribute(3, "class", this.CssClass);
        builder.AddAttribute(4, "value", BindConverter.FormatValue(this.CurrentValueAsString));
        builder.AddAttribute(5, "onchange", onchange);
        builder.CloseElement();
    }

    /// <inheritdoc />
    protected override string FormatValueAsString(TValue? value)
    {
        return value switch
        {
            DateTime dateTimeValue => BindConverter.FormatValue(
                dateTimeValue,
                DateFormat,
                CultureInfo.InvariantCulture),

            DateTimeOffset dateTimeOffsetValue => BindConverter.FormatValue(
                dateTimeOffsetValue,
                DateFormat,
                CultureInfo.InvariantCulture),

            _ => string.Empty, // Handles null for Nullable<DateTime>, etc.
        };
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out TValue result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        // Unwrap nullable types. We don't have to deal with receiving empty values for nullable
        // types here, because the underlying InputBase already covers that.
        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

        bool success;
        if (targetType == typeof(DateTime))
        {
            success = TryParseDateTime(value!, out result);
        }
        else if (targetType == typeof(DateTimeOffset))
        {
            success = TryParseDateTimeOffset(value!, out result);
        }
        else
        {
            throw new InvalidOperationException($"The type '{targetType}' is not a supported date type.");
        }

        if (success)
        {
            Debug.Assert(result != null, "result is null, but the conversion was successfull.");
            validationErrorMessage = null;
            return true;
        }

        validationErrorMessage = string.Format(
            CultureInfo.CurrentCulture,
            this.ParsingErrorMessage,
            this.FieldIdentifier.FieldName);

        return false;
    }

    private static bool TryParseDateTime(string value, out TValue? result)
    {
        var success = BindConverter.TryConvertToDateTime(
            value,
            CultureInfo.InvariantCulture,
            DateFormat,
            out var parsedValue);

        if (success)
        {
            result = (TValue)(object)parsedValue;
            return true;
        }

        result = default;
        return false;
    }

    private static bool TryParseDateTimeOffset(string value, out TValue result)
    {
        var success = BindConverter.TryConvertToDateTimeOffset(
            value,
            CultureInfo.InvariantCulture,
            DateFormat,
            out var parsedValue);

        if (success)
        {
            result = (TValue)(object)parsedValue;
            return true;
        }

        result = default!;
        return false;
    }
}