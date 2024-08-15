namespace QuickForm.Attributes;

/// <summary>
/// A custom attribute that directly applies the attribute to the underlying input html element
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class HtmlAttributeAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the name of the attribute
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the attribute
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="HtmlAttributeAttribute"/>
    /// </summary>
    public HtmlAttributeAttribute(string name, object value)
    {
        Name = name;
        Value = value;
    }
}