namespace QuickForm.Attributes;

/// <summary>
/// Adds a class=Class attribute to the underlying label element.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class LabelClassAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LabelClassAttribute"/> class.
    /// </summary>
    /// <param name="class">The css class which will be applied to the Input tag.</param>
    public LabelClassAttribute(string @class) => Class = @class;

    /// <summary>
    /// Gets the css class.
    /// </summary>
    public string Class { get; }
}