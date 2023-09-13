using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Common;

/// <summary>
/// A custom <see cref="FieldCssClassProvider"/> that adds Bootstrap validation classes to the input elements.
/// These classes are `is-valid` and `is-invalid`, instead of the default aspnet `valid` and `invalid`, which DO NOT
/// work with Bootstrap, which is built-into any Blazor / ASPNET core web APP template.
/// </summary>
internal sealed class BootstrapFormCssProvider : FieldCssClassProvider
{
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
            return isValid ? "is-valid" : "is-invalid";

        return string.Empty;
    }

    /// <summary>
    /// Creates an <see cref="EditContext"/> for the given entity,
    /// and sets up the <see cref="BootstrapFormCssProvider"/> as the <see cref="FieldCssClassProvider"/>.
    /// This method is best called in the `OnInitialized` method of a component.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <returns>The <see cref="EditContext"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
    public static EditContext CreateEditContextFor<T>([NotNull] T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        var provider = new BootstrapFormCssProvider();
        var editContext = new EditContext(entity);

        editContext.SetFieldCssClassProvider(provider);
        editContext.OnValidationRequested += (_, _) => provider.ValidationRequested = true;

        return editContext;
    }
}