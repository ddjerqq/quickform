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
}
