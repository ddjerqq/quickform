using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Internal;

internal sealed class CustomValidationCssClassProvider : FieldCssClassProvider
{
    private readonly string? _modifiedClass;

    private readonly string? _validClass;

    private readonly string? _inValidClass;

    /// <summary>
    /// Gets or sets a value indicating whether the class provider should validate all fields,
    /// regardless of whether they have been modified or not.
    /// </summary>
    public bool ValidateAllFields { get; set; }

    public CustomValidationCssClassProvider(string? modifiedClass, string? validClass, string? inValidClass)
    {
        _modifiedClass = modifiedClass;
        _validClass = validClass;
        _inValidClass = inValidClass;
    }

    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var isModified = editContext.IsModified(fieldIdentifier);
        var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

        var sb = new StringBuilder();

        if (isModified)
        {
            sb.Append(_modifiedClass);
            sb.Append(' ');
        }

        if (isModified || ValidateAllFields)
        {
            var result = isValid ? _validClass : _inValidClass;

            if (!string.IsNullOrEmpty(result))
                sb.Append(result);
        }

        return sb.ToString();
    }
}