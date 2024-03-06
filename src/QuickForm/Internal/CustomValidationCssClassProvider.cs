using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Internal;

internal sealed class CustomValidationCssClassProvider : FieldCssClassProvider
{
    private readonly string? _modifiedClass;

    private readonly string? _validClass;

    private readonly string? _inValidClass;

    public CustomValidationCssClassProvider(string? modifiedClass, string? validClass, string? inValidClass)
    {
        _modifiedClass = modifiedClass;
        _validClass = validClass;
        _inValidClass = inValidClass;
    }

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

        if (isModified)
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