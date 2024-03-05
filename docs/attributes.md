---
title: Attributes
---

# Customizing with attributes

## All attributes

| Attribute       | Description                                                                                         | Input tag modification                                                                                                               |
|-----------------|-----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------|
| `[Required]`    | Specifies that a data field value is required.                                                      | Adds required attribute and a red * to the form.                                                                                     |
| `[Editable]`    | Specifies whether or not the field should be editable.                                              | Adds disabled and readonly attributes to the input, as well as removing the event callbacks for when the value changes               |
| `[NotMapped]`   | Specifies that the property should be skipped during form generation                                | -                                                                                                                                    |
| `[Display]`     | Specifies the DisplayName and the Description from on attribute                                     | Adds the `DisplayName` as the label of the field and `Description` after the field as muted text (if any).                           |
| `[DisplayName]` | Specifies the DisplayName of the field, this is similar to `[Display]`. This also works on Enums    | Overrides the label of the field with the provided value                                                                             |
| `[Description]` | Specifies the Description of the field, this is similar to `[Display]`                              | Adds the provided value after the field as muted text                                                                                |
| `[Placeholder]` | Specifies the Placeholder of the field, default placeholder text is "Please enter {DisplayName}..." | Adds the provided value as the placeholder of the field                                                                              |
| `[DataList]`    | Specifies the DataList for the field, the data list be must be defined in the document              | Adds the provided value as the placeholder of the field                                                                              |
| `[DataType]`    | Specifies the DataType of the field. See supported DataTypes below.                                 | Adds the appropriate value as the type attribute of the input field                                                                  |
| `[DateType]`    | Specifies the DateType of the field. See supported DateTypes below.                                 | Adds the appropriate value as the type attribute of the input field                                                                  |
| `[Range]`       | Specifies the range of values for a numeric field.                                                  | Adds the appropriate value as the min and max attributes of the input field, and transforms this input into a bootstrap Slider Range |

## DataTypes

| DataType                              | HTML type attribute |
|---------------------------------------|---------------------|
| `DataType.Date`                       | `date`              |
| `DataType.Time`                       | `time`              |
| `DataType.DateTime`                   | `datetime-local`    |
| `DataType.EmailAddress`               | `email`             |
| `DataType.Password`                   | `password`          |
| `DataType.PhoneNumber`                | `tel`               |
| `DataType.Url` or `DataType.ImageUrl` | `url`               |
| `DataType.MultilineText`              | `null`              |

## DateTypes

| DateType                      | HTML type attribute |
|-------------------------------|---------------------|
| `InputDateType.Date`          | `date`              |
| `InputDateType.Time`          | `time`              |
| `InputDateType.DateTimeLocal` | `datetime-local`    |
| `InputDateType.Month`         | `month`             |