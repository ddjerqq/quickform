namespace QuickForm.Attributes;

/// <summary>
/// Marks that the property should be rendered as a choice of radio button.
/// Only works on enum properties.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class RadioAttribute : Attribute
{
}