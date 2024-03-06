using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Attributes;
using QuickForm.Components;

namespace QuickForm.Internal;

internal static class PropertyInfoExtensions
{
    internal static bool IsRequired(this PropertyInfo prop)
    {
        return prop.GetCustomAttribute<RequiredAttribute>() is not null;
    }

    internal static bool IsEditable(this PropertyInfo prop)
    {
        var isSetMethodNull = prop.SetMethod is null;
        var isInitOnly = prop.IsInitOnly();

        if (isSetMethodNull || isInitOnly)
            return false;

        var editableAttribute = prop.GetCustomAttribute<EditableAttribute>();
        return editableAttribute?.AllowEdit ?? true;
    }

    internal static bool IsCheckbox(this PropertyInfo prop)
    {
        return prop.PropertyType == typeof(bool);
    }

    internal static string DisplayName(this PropertyInfo prop)
    {
        string? displayName = null;

        displayName ??= prop.GetCustomAttribute<DisplayAttribute>()?.GetName();
        displayName ??= prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        displayName ??= prop.Name;

        return displayName;
    }

    internal static string? Description(this PropertyInfo prop)
    {
        string? description = null;

        description ??= prop.GetCustomAttribute<DisplayAttribute>()?.GetDescription();
        description ??= prop.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return description;
    }

    internal static string? DataListName(this PropertyInfo prop)
    {
        return prop.GetCustomAttribute<DataListAttribute>()?.DataListName;
    }

    internal static string? Placeholder(this PropertyInfo prop)
    {
        if (prop.GetCustomAttribute<PlaceholderAttribute>() is not { } placeholder)
            return null;

        return placeholder.PlaceholderText ?? $"Enter {prop.DisplayName()}...";
    }

    internal static string? ValidFeedbackText(this PropertyInfo prop)
    {
        if (prop.GetCustomAttribute<ValidFeedbackAttribute>() is not { Message: var message })
            return null;

        return message;
    }

    internal static RangeAttribute? RangeAttribute(this PropertyInfo prop)
    {
        return prop.GetCustomAttribute<RangeAttribute>();
    }

    private static readonly Dictionary<Func<Type, DataType?, bool>, Type> InputTypes = new()
    {
        { (t, _) => t == typeof(bool), typeof(InputCheckbox) },

        { (t, _) => t == typeof(short), typeof(InputNumber<short>) },
        { (t, _) => t == typeof(int), typeof(InputNumber<int>) },
        { (t, _) => t == typeof(long), typeof(InputNumber<long>) },
        { (t, _) => t == typeof(float), typeof(InputNumber<float>) },
        { (t, _) => t == typeof(double), typeof(InputNumber<double>) },
        { (t, _) => t == typeof(decimal), typeof(InputNumber<decimal>) },

        { (t, _) => t == typeof(short?), typeof(InputNumber<short?>) },
        { (t, _) => t == typeof(int?), typeof(InputNumber<int?>) },
        { (t, _) => t == typeof(long?), typeof(InputNumber<long?>) },
        { (t, _) => t == typeof(float?), typeof(InputNumber<float?>) },
        { (t, _) => t == typeof(double?), typeof(InputNumber<double?>) },
        { (t, _) => t == typeof(decimal?), typeof(InputNumber<decimal?>) },

        { (t, dt) => t == typeof(string) && dt is DataType.MultilineText, typeof(InputTextArea) },

        { (t, _) => t == typeof(DateTime), typeof(InputDate<DateTime>) },
        { (t, _) => t == typeof(DateTimeOffset), typeof(InputDate<DateTimeOffset>) },
        { (t, _) => t == typeof(DateOnly), typeof(InputDate<DateOnly>) },
        { (t, _) => t == typeof(TimeOnly), typeof(InputDate<TimeOnly>) },

        { (t, _) => t == typeof(DateTime?), typeof(InputDate<DateTime?>) },
        { (t, _) => t == typeof(DateTimeOffset?), typeof(InputDate<DateTimeOffset?>) },
        { (t, _) => t == typeof(DateOnly?), typeof(InputDate<DateOnly?>) },
        { (t, _) => t == typeof(TimeOnly?), typeof(InputDate<TimeOnly?>) },
    };

    internal static Type GetInputComponentType(this PropertyInfo prop)
    {
        var type = prop.PropertyType;
        var dType = prop.GetCustomAttribute<DataTypeAttribute>();

        if (type == typeof(bool?))
            throw new InvalidOperationException("Nullable bools are not supported, please just use a regular bool");

        if (type is { IsEnum: true, IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            throw new InvalidOperationException("Nullable enums are not supported, please just use a regular enum");

        foreach (var (predicate, componentType) in InputTypes)
            if (predicate(type, dType?.DataType))
                return componentType;

        if (type.IsEnum)
        {
            if (prop.PropertyType.IsDefined(typeof(FlagsAttribute), inherit: true))
                throw new InvalidOperationException("Flags enums are not supported yet");

            return typeof(InputEnumSelect<>).MakeGenericType(prop.PropertyType);
        }

        return typeof(InputText);
    }

    private static readonly Dictionary<Func<Type, DataType?, bool>, object?> HtmlTypeAttributes = new()
    {
        { (t, _) => t == typeof(bool), "checkbox" },

        { (t, dt) => t == typeof(string) && dt is DataType.Date, "date" },
        { (t, dt) => t == typeof(string) && dt is DataType.Time, "time" },
        { (t, dt) => t == typeof(string) && dt is DataType.DateTime, "datetime-local" },
        { (t, dt) => t == typeof(string) && dt is DataType.EmailAddress, "email" },
        { (t, dt) => t == typeof(string) && dt is DataType.Password, "password" },
        { (t, dt) => t == typeof(string) && dt is DataType.PhoneNumber, "tel" },
        { (t, dt) => t == typeof(string) && dt is DataType.Url or DataType.ImageUrl, "url" },

        { (t, _) => t == typeof(DateTime), InputDateType.DateTimeLocal },
        { (t, _) => t == typeof(DateTimeOffset), InputDateType.DateTimeLocal },
        { (t, _) => t == typeof(DateOnly), InputDateType.Date },
        { (t, _) => t == typeof(TimeOnly), InputDateType.Time },
    };

    internal static object? GetHtmlInputType(this PropertyInfo prop)
    {
        var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
        var dataTypeAttribute = prop.GetCustomAttribute<DataTypeAttribute>();

        // handle custom DataTypes
        if (dataTypeAttribute?.DataType == DataType.Custom
            && !string.IsNullOrEmpty(dataTypeAttribute.CustomDataType))
            return dataTypeAttribute.CustomDataType;

        // handle dates
        if (type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(DateOnly) || type == typeof(TimeOnly))
            if (prop.GetCustomAttribute<DateTypeAttribute>() is { InputDateType: var inputDateType })
                return inputDateType;

        // handle other types
        foreach (var (pred, htmlInputType) in HtmlTypeAttributes)
            if (pred(type, dataTypeAttribute?.DataType))
                return htmlInputType;

        return null;
    }

    private static bool IsInitOnly(this PropertyInfo property)
    {
        if (!property.CanWrite)
            return false;

        var setMethod = property.SetMethod;
        var setMethodReturnParameterModifiers = setMethod?.ReturnParameter.GetRequiredCustomModifiers();
        return setMethodReturnParameterModifiers?.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit)) ?? false;
    }
}