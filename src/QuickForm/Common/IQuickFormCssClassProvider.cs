namespace QuickForm.Common;

/// <summary>
/// Provides the base css classes for the quick form.
/// </summary>
public interface IQuickFormClassProvider
{
    /// <summary>
    /// Gets the base css class for the Input container element.
    /// </summary>
    public string? Editor { get; }

    /// <summary>
    /// Gets the base css class for the Label element.
    /// </summary>
    public string? Label { get; }

    /// <summary>
    /// Gets the base css class for the Input element.
    /// </summary>
    public string? Input { get; }
}