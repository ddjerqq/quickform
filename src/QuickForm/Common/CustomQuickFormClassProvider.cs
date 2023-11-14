using QuickForm.Components;

namespace QuickForm.Common;

/// <summary>
/// Provides the base css classes for the quick form.
/// With custom values provided per instance.
/// </summary>
public class CustomQuickFormClassProvider : IQuickFormClassProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomQuickFormClassProvider"/> class with empty classes
    /// use this constructor for customizing the classes per instance
    /// </summary>
    public CustomQuickFormClassProvider()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomQuickFormClassProvider"/> class with custom classes
    /// </summary>
    public CustomQuickFormClassProvider(string? editor, string? label, string? input)
    {
        Editor = _ => editor;
        Label = _ => label;
        Input = _ => input;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomQuickFormClassProvider"/> class with custom classes
    /// </summary>
    public CustomQuickFormClassProvider(
        Func<IQuickFormField, string?> editor,
        Func<IQuickFormField, string?> label,
        Func<IQuickFormField, string?> input)
    {
        Editor = editor;
        Label = label;
        Input = input;
    }

    /// <inheritdoc />
    public Func<IQuickFormField, string?> Editor { get; init; } = _ => default;

    /// <inheritdoc />
    public Func<IQuickFormField, string?> Label { get; init; } = _ => default;

    /// <inheritdoc />
    public Func<IQuickFormField, string?> Input { get; init; } = _ => default;
}