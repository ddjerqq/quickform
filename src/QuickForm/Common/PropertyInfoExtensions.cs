using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Humanizer;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Attributes;
using QuickForm.Components;

namespace QuickForm.Common;

internal static class PropertyInfoExtensions
{
    public static bool IsRequired(this PropertyInfo prop)
    {
        return prop.GetCustomAttribute<RequiredAttribute>() is not null;
    }

    public static bool IsEditable(this PropertyInfo prop)
    {
        return prop.SetMethod is not null &&
               prop.GetCustomAttribute<EditableAttribute>() is null or { AllowEdit: true };
    }

    public static bool IsCheckbox(this PropertyInfo prop)
    {
        return prop.PropertyType == typeof(bool);
    }

    public static string DisplayName(this PropertyInfo prop)
    {
        string? displayName = null;

        displayName ??= prop.GetCustomAttribute<DisplayAttribute>()?.GetName();
        displayName ??= prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        displayName ??= prop.Name.Humanize();

        return displayName;
    }

    public static string? Description(this PropertyInfo prop)
    {
        string? description = null;

        description ??= prop.GetCustomAttribute<DisplayAttribute>()?.GetDescription();
        description ??= prop.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return description;
    }

    public static string? DataListName(this PropertyInfo prop)
    {
        return prop.GetCustomAttribute<DataListAttribute>()?.DataListName;
    }

    public static string? Placeholder(this PropertyInfo prop)
    {
        if (prop.GetCustomAttribute<PlaceholderAttribute>() is not { } placeholder)
        {
            return null;
        }

        return placeholder.PlaceholderText ?? $"Enter {prop.DisplayName()}...";
    }

    public static RangeAttribute? RangeAttribute(this PropertyInfo prop)
    {
        return prop.GetCustomAttribute<RangeAttribute>();
    }

    // this could be written better, but I cba, and I admire yandere developer so...
    public static Type GetInputComponentType(this PropertyInfo prop)
    {
        var type = prop.PropertyType;
        var dType = prop.GetCustomAttribute<DataTypeAttribute>();

        if (type == typeof(bool))
            return typeof(InputCheckbox);

        if (type == typeof(short))
            return typeof(InputNumber<short>);

        if (type == typeof(int))
            return typeof(InputNumber<int>);

        if (type == typeof(long))
            return typeof(InputNumber<long>);

        if (type == typeof(float))
            return typeof(InputNumber<float>);

        if (type == typeof(double))
            return typeof(InputNumber<double>);

        if (type == typeof(decimal))
            return typeof(InputNumber<decimal>);

        if (type == typeof(string) && dType is { DataType: DataType.MultilineText })
            return typeof(InputTextArea);

        if (type == typeof(string))
            return typeof(InputText);

        if (type == typeof(DateTime) && dType is { DataType: DataType.Date })
            return typeof(InputDate<DateTime>);

        if (type == typeof(DateTime?) && dType is { DataType: DataType.Date })
            return typeof(InputDate<DateTime?>);

        if (type == typeof(DateTime))
            return typeof(InputDateTime<DateTime>);

        if (type == typeof(DateTime?))
            return typeof(InputDateTime<DateTime?>);

        if (type == typeof(DateTimeOffset) && dType is { DataType: DataType.Date })
            return typeof(InputDate<DateTimeOffset>);

        if (type == typeof(DateTimeOffset?) && dType is { DataType: DataType.Date })
            return typeof(InputDate<DateTimeOffset?>);

        if (type == typeof(DateTimeOffset))
            return typeof(InputDateTime<DateTimeOffset>);

        if (type == typeof(DateTimeOffset))
            return typeof(InputDateTime<DateTimeOffset>);

        // we currently do not support multiple selection enums
        if (type.IsEnum && !prop.PropertyType.IsDefined(typeof(FlagsAttribute), inherit: true))
            return typeof(InputEnumSelect<>).MakeGenericType(prop.PropertyType);

        return typeof(InputText);
    }

    public static string? GetHtmlInputType(this PropertyInfo prop)
    {
        var type = prop.PropertyType;
        var dType = prop.GetCustomAttribute<DataTypeAttribute>();

        if (type == typeof(bool))
            return "checkbox";

        if (prop.RangeAttribute() is not null)
            return "range";

        if (type == typeof(string))
        {
            return dType?.DataType switch
            {
                DataType.Date => "date",
                DataType.Time => "time",
                DataType.DateTime => "datetime-local",
                DataType.EmailAddress => "email",
                DataType.Password => "password",
                DataType.PhoneNumber => "tel",
                DataType.Url or DataType.ImageUrl => "url",
                DataType.MultilineText => null,
                _ => null
            };
        }

        return null;
    }
}