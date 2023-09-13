namespace QuickForm.Attributes;

/// <summary>
/// Adds a placeholder=PlaceholderValue attribute to the underlying input element.
/// For placeholder text.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PlaceholderAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceholderAttribute"/> class.
    /// </summary>
    /// <param name="placeholderText">The placeholder text</param>
    public PlaceholderAttribute(string placeholderText) => PlaceholderText = placeholderText;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceholderAttribute"/> class.
    /// When no value is passed, the default placeholder text -
    /// "Please enter {Name}..." will be used.
    /// </summary>
    public PlaceholderAttribute() => PlaceholderText = null;

    /// <summary>
    /// Gets the placeholder text, if any.
    /// </summary>
    public string? PlaceholderText { get; }
}