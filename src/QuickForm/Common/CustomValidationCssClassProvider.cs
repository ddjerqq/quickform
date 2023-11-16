using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Common;

/// <summary>
/// A custom <see cref="FieldCssClassProvider"/> that adds custom validation classes to the input elements.
/// </summary>
public sealed class CustomValidationCssClassProvider : FieldCssClassProvider
{
    private readonly string _validClass;

    private readonly string _inValidClass;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomValidationCssClassProvider"/> class with custom classes
    /// for when the input is valid or invalid.
    /// </summary>
    /// <param name="validClass">css class to apply when the input is valid</param>
    /// <param name="inValidClass">css class to apply when the input is invalid</param>
    public CustomValidationCssClassProvider(string validClass, string inValidClass)
    {
        _validClass = validClass;
        _inValidClass = inValidClass;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the class provider should validate all fields,
    /// regardless of whether they have been modified or not.
    /// </summary>
    public bool ValidateAllFields { get; set; }

    /// <inheritdoc />
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        if (editContext.IsModified(fieldIdentifier) || ValidateAllFields)
            return editContext.GetValidationMessages(fieldIdentifier).Any()
                ? _inValidClass
                : _validClass;

        return string.Empty;
    }
}