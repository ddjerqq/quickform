using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Common;

/// <summary>
/// A custom <see cref="FieldCssClassProvider"/> that adds custom validation classes to the input elements.
/// </summary>
public sealed class CustomValidationCssClassProvider : FieldCssClassProvider
{
    private readonly string? _modifiedClass;

    private readonly string? _validClass;

    private readonly string? _inValidClass;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomValidationCssClassProvider"/> class with custom classes
    /// for when the input is valid or invalid.
    /// </summary>
    /// <param name="modifiedClass">css class to apply when the input is modified</param>
    /// <param name="validClass">css class to apply when the input is valid</param>
    /// <param name="inValidClass">css class to apply when the input is invalid</param>
    public CustomValidationCssClassProvider(string? modifiedClass, string? validClass, string? inValidClass)
    {
        _modifiedClass = modifiedClass;
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
        bool isModified = editContext.IsModified(fieldIdentifier);
        bool isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

        var sb = new StringBuilder();

        if (isModified)
        {
            sb.Append(_modifiedClass);
            sb.Append(' ');
        }

        if (isModified || ValidateAllFields)
        {
            var result = isValid
                ? _validClass
                : _inValidClass;

            if (!string.IsNullOrEmpty(result))
                sb.Append(result);
        }

        return sb.ToString();
    }
}