using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Attributes;

/// <summary>
/// Adds a DateInputType attribute to the underlying date input element.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class DateTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DateTypeAttribute"/> class.
    /// </summary>
    /// <param name="inputDateType">
    /// The type of the date input.
    /// this determines the underlying HTML type attribute on the input tag.
    /// </param>
    public DateTypeAttribute(InputDateType inputDateType) => InputDateType = inputDateType;

    /// <summary>
    /// Gets the type of the date input.
    /// </summary>
    public InputDateType InputDateType { get; }

    /// <summary>
    /// Gets the HTML input type.
    /// </summary>
    public string HtmlInputType => InputDateType switch
    {
        InputDateType.Date => "date",
        InputDateType.DateTimeLocal => "datetime-local",
        InputDateType.Month => "month",
        InputDateType.Time => "time",
        _ => throw new ArgumentOutOfRangeException(),
    };
}
