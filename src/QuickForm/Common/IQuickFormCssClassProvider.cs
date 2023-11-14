using QuickForm.Components;

namespace QuickForm.Common;

/// <summary>
/// Provides the base css classes for the quick form.
/// </summary>
public interface IQuickFormClassProvider
{
    /// <summary>
    /// Gets the base css class for the Input container element.
    /// </summary>
    public Func<IQuickFormField, string?> Editor { get; }

    /// <summary>
    /// Gets the base css class for the Label element.
    /// </summary>
    public Func<IQuickFormField, string?> Label { get; }

    /// <summary>
    /// Gets the base css class for the Input element.
    /// </summary>
    public Func<IQuickFormField, string?> Input { get; }
}