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
        displayName ??= prop.Name.Humanize();

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

    internal static string? InputClass(this PropertyInfo prop)
    {
        if (prop.GetCustomAttribute<InputClassAttribute>() is not { Class: var @class })
            return null;

        return @class;
    }

    internal static string? EditorClass(this PropertyInfo prop)
    {
        if (prop.GetCustomAttribute<EditorClassAttribute>() is not { Class: var @class })
            return null;

        return @class;
    }

    internal static string? LabelClass(this PropertyInfo prop)
    {
        if (prop.GetCustomAttribute<LabelClassAttribute>() is not { Class: var @class })
            return null;

        return @class;
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
        { (t, dt) => t == typeof(string) && dt is DataType.MultilineText, typeof(InputTextArea) },
        { (t, _) => t == typeof(string), typeof(InputText) },
        { (t, dt) => t == typeof(DateTime) && dt is DataType.Date, typeof(InputDate<DateTime>) },
        { (t, dt) => t == typeof(DateTime?) && dt is DataType.Date, typeof(InputDate<DateTime?>) },

        // TODO support for DateOnly and TimeOnly through InputText
        { (t, dt) => t == typeof(DateTime) && dt is DataType.Time, typeof(InputText) },
        { (t, dt) => t == typeof(DateTime?) && dt is DataType.Time, typeof(InputText) },
        { (t, _) => t == typeof(DateTime), typeof(InputDateTime<DateTime>) },
        { (t, _) => t == typeof(DateTime?), typeof(InputDateTime<DateTime?>) },
        { (t, _) => t == typeof(DateOnly), typeof(InputText) },
        { (t, _) => t == typeof(DateOnly?), typeof(InputText) },
    };

    internal static Type GetInputComponentType(this PropertyInfo prop)
    {
        var type = prop.PropertyType;
        var dType = prop.GetCustomAttribute<DataTypeAttribute>();

        foreach (var (predicate, componentType) in InputTypes)
            if (predicate(type, dType?.DataType))
                return componentType;

        if (type.IsEnum && !prop.PropertyType.IsDefined(typeof(FlagsAttribute), inherit: true))
            return typeof(InputEnumSelect<>).MakeGenericType(prop.PropertyType);

        return typeof(InputText);
    }

    // TODO support for explicit input type
    internal static string? GetHtmlInputType(this PropertyInfo prop)
    {
        var type = prop.PropertyType;
        var dType = prop.GetCustomAttribute<DataTypeAttribute>();

        if (type == typeof(bool))
            return "checkbox";

        if (type == typeof(string))
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

        if (type == typeof(DateTime) || type == typeof(DateTime?))
            return dType?.DataType switch
            {
                DataType.Date => "date",
                DataType.Time => "time",
                DataType.DateTime => "datetime-local",
                _ => null
            };

        // if (type == typeof(DateOnly) || type == typeof(DateOnly?))
        //     return "date";
        //
        // if (type == typeof(TimeOnly) || type == typeof(TimeOnly?))
        //     return "time";

        return null;
    }

    /// <summary>
    /// Determines if this property is marked as init-only.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>True if the property is init-only, false otherwise.</returns>
    internal static bool IsInitOnly(this PropertyInfo property)
    {
        if (!property.CanWrite)
            return false;

        var setMethod = property.SetMethod;

        // Get the modifiers applied to the return parameter.
        var setMethodReturnParameterModifiers = setMethod?.ReturnParameter.GetRequiredCustomModifiers();

        // Init-only properties are marked with the IsExternalInit type.
        return setMethodReturnParameterModifiers?.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit)) ?? false;
    }
}