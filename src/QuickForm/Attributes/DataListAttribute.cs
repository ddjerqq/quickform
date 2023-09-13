namespace QuickForm.Attributes;

/// <summary>
/// Adds a datalist=DataListName attribute to the underlying input element.
/// For autocompletion functionality.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DataListAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataListAttribute"/> class.
    /// </summary>
    /// <param name="dataListName">The name of the datalist.</param>
    public DataListAttribute(string dataListName) => DataListName = dataListName;

    /// <summary>
    /// Gets the name of the datalist.
    /// </summary>
    public string DataListName { get; }
}