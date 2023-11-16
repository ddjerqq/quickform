using QuickForm.Components;

namespace QuickForm.Common;

/// <summary>
/// Provides the base css classes for the quick form.
/// With custom values provided per instance.
/// </summary>
public class CustomQuickFormFieldCssClassProvider : IQuickFormFieldCssClassProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomQuickFormFieldCssClassProvider"/> class with empty classes
    /// use this constructor for customizing the classes per instance
    /// </summary>
    public CustomQuickFormFieldCssClassProvider()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomQuickFormFieldCssClassProvider"/> class with custom classes
    /// </summary>
    public CustomQuickFormFieldCssClassProvider(string? editor, string? label, string? input)
    {
        Editor = _ => editor;
        Label = _ => label;
        Input = _ => input;
    }

    /// <inheritdoc />
    public Func<IQuickFormField, string?> Editor { get; init; } = _ => default;

    /// <inheritdoc />
    public Func<IQuickFormField, string?> Label { get; init; } = _ => default;

    /// <inheritdoc />
    public Func<IQuickFormField, string?> Input { get; init; } = _ => default;
}