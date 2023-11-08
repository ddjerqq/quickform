namespace QuickForm.Common;

/// <summary>
/// Provides the base css classes for the quick form.
/// With custom values provided per instance.
/// </summary>
public class CustomQuickFormClassProvider : IQuickFormClassProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomQuickFormClassProvider"/> class with custom classes
    /// </summary>
    public CustomQuickFormClassProvider(string? editor, string? label, string? input)
    {
        Editor = editor;
        Label = label;
        Input = input;
    }

    /// <inheritdoc />
    public string? Editor { get; }

    /// <inheritdoc />
    public string? Label { get; }

    /// <inheritdoc />
    public string? Input { get; }
}