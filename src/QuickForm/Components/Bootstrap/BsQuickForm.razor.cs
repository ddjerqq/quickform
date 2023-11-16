using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using QuickForm.Common;

// TODO make one for tailwind and place it at the root QuickForm namespace;

namespace QuickForm.Components.Bootstrap;

/// <summary>
/// A quick form with Bootstrap styling.
/// </summary>
public sealed class BsQuickForm<TEntity> : QuickForm<TEntity>
    where TEntity : class, new()
{
    /// <summary>
    /// Set the CSS class provider for the form fields.
    /// </summary>
    protected override void OnParametersSet()
    {
        FieldCssClassProvider = new CustomQuickFormFieldCssClassProvider
        {
            Editor = field => "text-start mb-3" + (field.PropertyInfo.PropertyType == typeof(bool) ? " form-check" : ""),
            Label = field => "text-info fw-bold mb-1" + (field.PropertyInfo.PropertyType == typeof(bool) ? "form-check-label" : ""),
            Input = field => field.PropertyInfo.PropertyType == typeof(bool) ? "form-check-input" : "form-control",
        };

        ValidationCssClassProvider = new CustomValidationCssClassProvider("is-valid", "is-invalid");

        DescriptionTemplate ??= description =>
        {
            return builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "form-text text-muted");
                builder.AddContent(2, description);
                builder.CloseElement();
            };
        };

        ValidFeedbackTemplate ??= validFeedback =>
        {
            return builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "valid-feedback fw-bold");
                builder.AddContent(2, validFeedback);
                builder.CloseElement();
            };
        };

        InValidFeedbackTemplate ??= invalidFeedback =>
        {
            return builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "invalid-feedback fw-bold");
                builder.AddContent(2, invalidFeedback);
                builder.CloseElement();
            };
        };

        SubmitButtonTemplate ??= builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "submit");
            builder.AddAttribute(2, "class", "btn btn-outline-success w-100");
            builder.AddContent(3, "submit");
            builder.CloseElement();
        };

        base.OnParametersSet();

        // TODO test that this throws
        if (InputTemplate is not null)
            throw new InvalidOperationException(
                "InputTemplate is not supported for Bootstrap. " +
                "because it would not work with bootstraps sibling method for validation.");

        FormClass ??= "form w-50 mx-auto";
    }
}