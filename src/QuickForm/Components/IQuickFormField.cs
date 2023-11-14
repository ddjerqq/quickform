using System.Reflection;

namespace QuickForm.Components;

/// <summary>
/// Represents a field in the quick form.
/// </summary>
public interface IQuickFormField
{
    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> of the field.
    /// </summary>
    public PropertyInfo PropertyInfo { get; }
}