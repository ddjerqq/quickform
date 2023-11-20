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

    internal static RadioAttribute? RadioAttribute(this PropertyInfo prop)
    {
        return prop.GetCustomAttribute<RadioAttribute>();
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
            var isRadio = prop.GetCustomAttribute<RadioAttribute>() is not null;
            if (isRadio)
            {
                // TODO implement this one day
                // return typeof(InputEnumRadio<>).MakeGenericType(prop.PropertyType);
                throw new InvalidOperationException("RadioAttribute is not implemented yet.");
            }

            var isFlagsEnum = prop.PropertyType.IsDefined(typeof(FlagsAttribute), inherit: true);
            if (isFlagsEnum)
            {
                // TODO flags multi choice select
            }

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
        foreach ((var pred, object? htmlInputType) in HtmlTypeAttributes)
            if (pred(type, dataTypeAttribute?.DataType))
                return htmlInputType;

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