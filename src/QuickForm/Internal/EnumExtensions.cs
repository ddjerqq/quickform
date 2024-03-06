using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace QuickForm.Internal;

internal static class EnumExtensions
{
    internal static string GetDisplayName(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var member = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault();

        // get the name from attributes
        var displayName =
            member?.GetCustomAttribute<DisplayAttribute>()?.GetName()
            ?? member?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
            ?? value.ToString();

        // make it human readable
        var regex = new Regex(@"([a-z])([A-Z])");
        displayName = regex.Replace(displayName, "$1 $2");

        return displayName;
    }
}