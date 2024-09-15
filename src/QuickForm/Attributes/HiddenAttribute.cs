namespace QuickForm.Attributes;

/// <summary>
/// For attributes that have an input field, but should not be displayed.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HiddenAttribute : Attribute
{
}
