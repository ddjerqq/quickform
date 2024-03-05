using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace QuickForm.Components;

/// <summary>
/// A TailwindCSS styled generic component that displays a form for editing / creating a model.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public partial class TwQuickForm<TEntity> : ComponentBase, IDisposable
    where TEntity : class, new()
{
    private QuickForm<TEntity> QuickFormRef { get; set; } = default!;

    /// <inheritdoc cref="QuickForm{TEntity}.Model"/>
    [Parameter, EditorRequired]
    public TEntity? Model { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the model changes.
    /// This could be used to update the model in the parent component.
    /// </summary>
    [Parameter]
    public EventCallback<TEntity> OnModelChanged { get; set; }

    /// <summary>
    /// A callback that will be invoked when the form is submitted.
    ///
    /// If using this parameter, you are responsible for triggering any validation
    /// manually, e.g., by calling <see cref="EditContext.Validate"/>.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> OnSubmit { get; set; }

    /// <summary>
    /// A callback that will be invoked when the form is submitted and the
    /// <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/> is determined to be valid.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> OnValidSubmit { get; set; }

    /// <summary>
    /// A callback that will be invoked when the form is submitted and the
    /// <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/> is determined to be invalid.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created <c>form</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
        QuickFormRef.Dispose();
    }
}