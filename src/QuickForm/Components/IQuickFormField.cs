using Microsoft.AspNetCore.Components;

namespace QuickForm.Components;

/// <summary>
/// Represents a field in a <see cref="QuickForm{TEntity}"/>.
/// </summary>
public interface IQuickFormField
{
    /// <summary>
    /// Gets the id for the input element.
    /// </summary>
    public string EditorId { get; }

    /// <summary>
    /// Gets the display name for the input element.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Gets the description for the input element, if any.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Gets the valid feedback for the input element, if any.
    /// </summary>
    public string? ValidFeedback { get; }

    /// <summary>
    /// Gets the input component template, which is automatically generated.
    /// </summary>
    /// <note>
    /// the string parameter is the class to be applied to the input element.
    /// </note>
    public RenderFragment<string> InputComponent { get; }

    /// <summary>
    /// Gets the validation message element template, which is automatically generated.
    /// </summary>
    /// <note>
    /// the string parameter is the class to be applied to the input element.
    /// </note>
    public RenderFragment<string> ValidationMessages { get; }

    /// <summary>
    /// Gets if the input together with its label and everything else should be hidden.
    /// </summary>
    /// <note>
    /// The input element will still be rendered, but it will be hidden.
    /// </note>
    public bool IsHidden { get; }
}