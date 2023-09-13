using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Common;

namespace QuickForm.Components;

/// <summary>
/// A generic component that displays a form for editing / creating a model.
/// </summary>
/// <typeparam name="TEntity">The type of the model to be edited / created by this form.</typeparam>
public partial class QuickForm<TEntity> : ComponentBase, IDisposable
    where TEntity : class, new()
{
    /// <summary>
    /// Gets or sets the model to be edited / created by this form.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public TEntity Model { get; set; } = default!;

    /// <summary>
    /// Gets or sets the CSS class to be applied to the underlying EditForm.
    /// </summary>
    [Parameter]
    public string FormClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the callback to be invoked when the model changes.
    /// This could be used to update the model in the parent component.
    /// </summary>
    [Parameter]
    public EventCallback<TEntity> OnModelChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when the form is submitted and valid.
    /// </summary>
    [Parameter]
    public EventCallback OnValidSubmit { get; set; }

    internal string BaseEditorId { get; } = Guid.NewGuid().ToString();

    internal EditContext EditContext { get; set; } = null!;

    private List<QuickFormField<TEntity>> Fields { get; set; } = new ();

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

        EditContext = BootstrapFormCssProvider.CreateEditContextFor(Model);

        Fields.ForEach(f => f.ValueChanged -= OnValueChanged);
        Fields = QuickFormField<TEntity>.FromForm(this).ToList();
        Fields.ForEach(f => f.ValueChanged += OnValueChanged);
    }

    private void OnValueChanged(object? sender, EventArgs e)
    {
        InvokeAsync(() => OnModelChanged.InvokeAsync(Model));
    }

    private void OnReset()
    {
        EditContext = BootstrapFormCssProvider.CreateEditContextFor(Model);
        StateHasChanged();
    }
}