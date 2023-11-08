namespace QuickForm.Attributes;

/// <summary>
/// Supplies the message for when the input is valid.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ValidFeedbackAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidFeedbackAttribute"/> class.
    /// </summary>
    /// <param name="message">The message for valid feedback.</param>
    public ValidFeedbackAttribute(string message) => Message = message;

    /// <summary>
    /// Gets the css class.
    /// </summary>
    public string Message { get; }
}