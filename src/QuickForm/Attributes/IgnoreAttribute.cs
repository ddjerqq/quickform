namespace QuickForm.Attributes;

/// <summary>
/// For attributes that don't have NotMapped, but still need to be ignored.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IgnoreAttribute : Attribute
{
}
