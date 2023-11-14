using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Common;
using Microsoft.AspNetCore.Components.Web;

namespace QuickForm.Components;

/// <summary>
/// A generic component that displays a form for editing / creating a model.
/// </summary>
/// <typeparam name="TEntity">The type of the model to be edited / created by this form.</typeparam>
public partial class QuickForm<TEntity> : ComponentBase, IDisposable
    where TEntity : class, new()
{
    /// <summary>
    /// the model to be edited / created by this form.
    /// </summary>
    protected TEntity? InternalModel;

    /// <summary>
    /// Indicates if the model has been set explicitly.
    /// </summary>
    protected bool HasSetModelExplicitly;

    /// <summary>
    /// The <see cref="EditContext"/> instance explicitly associated with this model
    /// </summary>
    protected EditContext? InternalEditContext;

    /// <summary>
    /// Indicates if the <see cref="EditContext"/> has been set explicitly.
    /// </summary>
    protected bool HasSetEditContextExplicitly;

    /// <summary>
    /// Gets or sets the title template of the form.
    /// </summary>
    [Parameter]
    public RenderFragment<RenderFragment?>? InputTemplate { get; set; }

    /// <summary>
    /// Gets or sets the title template of the form.
    /// </summary>
    [Parameter]
    public RenderFragment<string?>? DescriptionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the valid feedback template of the form.
    /// </summary>
    [Parameter]
    public RenderFragment<string?>? ValidFeedbackTemplate { get; set; }

    /// <summary>
    /// Gets or sets the invalid feedback template of the form.
    /// </summary>
    [Parameter]
    public RenderFragment<RenderFragment?>? InValidFeedbackTemplate { get; set; }

    /// <summary>
    /// Gets or sets the submit button template of the form.
    /// </summary>
    /// <note>
    /// Make sure to give this button type="submit" to make it work.
    /// </note>
    [Parameter]
    public RenderFragment? SubmitButtonTemplate { get; set; }

    /// <summary>
    /// Gets or sets the model to be edited / created by this form.
    /// </summary>
    [Parameter]
    public TEntity? Model
    {
        get => InternalModel;
        set
        {
            InternalModel = value;
            HasSetModelExplicitly = value != null;
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="EditContext"/> instance explicitly associated with this model
    /// </summary>
    [Parameter]
    public EditContext EditContext
    {
        get => InternalEditContext!;
        set
        {
            InternalEditContext = value;
            HasSetEditContextExplicitly = value != null!;
        }
    }

    /// <summary>
    /// Gets or sets the CSS class to be applied to the underlying EditForm.
    /// </summary>
    [Parameter]
    public string? FormClass { get; set; }

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
    /// <see cref="EditContext"/> is determined to be valid.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> OnValidSubmit { get; set; }

    /// <summary>
    /// A callback that will be invoked when the form is submitted and the
    /// <see cref="EditContext"/> is determined to be invalid.
    /// </summary>
    [Parameter]
    public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IQuickFormClassProvider"/> that is used to determine the CSS class
    /// for the elements on the form.
    /// </summary>
    [Parameter]
    public IQuickFormClassProvider? CssClassProvider { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ValidationCssClassProvider"/> that is used to determine the CSS class,
    /// for valid and invalid fields.
    /// </summary>
    [Parameter]
    public FieldCssClassProvider ValidationCssClassProvider { get; set; } = new();

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created <c>form</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    internal string BaseEditorId { get; } = Guid.NewGuid().ToString()[..8];

    internal List<QuickFormField<TEntity>> Fields { get; set; } = new();

    /// <summary>
    /// Triggers a validation of the form.
    /// </summary>
    public void Revalidate()
    {
        InternalEditContext?.Validate();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Fields.ForEach(f => f.ValueChanged -= OnValueChanged);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        switch (HasSetEditContextExplicitly)
        {
            case true when Model is not null:
                throw new InvalidOperationException(
                    $"{nameof(EditForm)} requires a {nameof(Model)} " +
                    $"parameter, or an {nameof(EditContext)} parameter, but not both.");

            case false when Model is null:
                throw new InvalidOperationException(
                    $"{nameof(EditForm)} requires either a {nameof(Model)} " +
                    $"parameter, or an {nameof(EditContext)} parameter, please provide one of these.");
        }

        // copied from EditForm.cs
        // If you're using OnSubmit, it becomes your responsibility to trigger validation manually
        // (e.g., so you can display a "pending" state in the UI). In that case you don't want the
        // system to trigger a second validation implicitly, so don't combine it with the simplified
        // OnValidSubmit/OnInvalidSubmit handlers.
        if (OnSubmit.HasDelegate && (OnValidSubmit.HasDelegate || OnInvalidSubmit.HasDelegate))
            throw new InvalidOperationException(
                $"When supplying an {nameof(OnSubmit)} parameter to " +
                $"{nameof(EditForm)}, do not also supply {nameof(OnValidSubmit)} or {nameof(OnInvalidSubmit)}.");

        // Update _editContext if we don't have one yet, or if they are supplying a
        // potentially new EditContext, or if they are supplying a different Model
        if (Model is not null && Model != InternalEditContext?.Model)
        {
            InternalEditContext = new EditContext(Model!);
            InternalEditContext.SetFieldCssClassProvider(ValidationCssClassProvider);
        }

        // set the model from EditContext.
        Model ??= EditContext.Model as TEntity;

        Fields.ForEach(f => f.ValueChanged -= OnValueChanged);
        Fields = QuickFormField<TEntity>.FromForm(this).ToList();
        Fields.ForEach(f => f.ValueChanged += OnValueChanged);
    }

    /// <summary>
    /// Invoked when the value of a field changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnValueChanged(object? sender, EventArgs e)
    {
        InternalEditContext?.Validate();
        InvokeAsync(() => OnModelChanged.InvokeAsync(Model));
    }
}