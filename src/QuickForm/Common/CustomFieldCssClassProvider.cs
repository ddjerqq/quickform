using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Common;

/// <summary>
/// A custom <see cref="FieldCssClassProvider"/> that adds custom validation classes to the input elements.
/// </summary>
public sealed class CustomFieldCssClassProvider : FieldCssClassProvider
{
    private readonly string _validClass;

    private readonly string _inValidClass;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFieldCssClassProvider"/> class with custom classes
    /// for when the input is valid or invalid.
    /// </summary>
    /// <param name="validClass">css class to apply when the input is valid</param>
    /// <param name="inValidClass">css class to apply when the input is invalid</param>
    public CustomFieldCssClassProvider(string validClass, string inValidClass)
    {
        _validClass = validClass;
        _inValidClass = inValidClass;
    }

    /// <summary>
    /// Gets or sets a value indicating whether validation has been requested.
    /// This is required to stop the form from showing validation errors before the user has interacted with the form.
    /// </summary>
    public bool ValidationRequested { get; set; }

    /// <inheritdoc />
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        if (!editContext.IsModified(fieldIdentifier) && !ValidationRequested)
            return string.Empty;

        var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();
        if (editContext.IsModified(fieldIdentifier))
            return isValid ? _validClass : _inValidClass;

        return string.Empty;
    }
}