namespace QuickForm.Attributes;

/// <summary>
/// Adds a class=Class attribute to the underlying input element.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class InputClassAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputClassAttribute"/> class.
    /// </summary>
    /// <param name="class">The css class which will be applied to the Input tag.</param>
    public InputClassAttribute(string @class) => Class = @class;

    /// <summary>
    /// Gets the css class.
    /// </summary>
    public string Class { get; }
}