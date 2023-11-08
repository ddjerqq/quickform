namespace QuickForm.Attributes;

/// <summary>
/// Adds a class=Class attribute to the container for the input group.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public sealed class EditorClassAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditorClassAttribute"/> class.
    /// </summary>
    /// <param name="class">The css class which will be applied to the container's div tag.</param>
    public EditorClassAttribute(string @class) => Class = @class;

    /// <summary>
    /// Gets the css class.
    /// </summary>
    public string Class { get; }
}