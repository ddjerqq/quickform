---
title: Attributes
---

# Customizing with attributes

## All attributes

| Attribute       | Description                                                                                         | Input tag modification                                                                                                                                                                                   |
|-----------------|-----------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `[Required]`    | Mark a field as required.                                                                           | Adds `required` attribute to the underlying HTML input element, and validations.                                                                                                                         |
| `[Editable]`    | Specifies whether or not the field should be editable.                                              | Adds `disabled` and `readonly` attributes to the underlying HTML input element, as well as removing the event callbacks for when the value changes, so that users cant edit the HTML and bypass security |
| `[NotMapped]`   | Specifies that the property should be skipped during form generation                                | -                                                                                                                                                                                                        |
| `[Display]`     | Specifies the Display Name and the Description for the field                                        | Adds the `DisplayName` as the label of the field and `Description` after the field as muted text (if any).                                                                                               |
| `[DisplayName]` | Specifies the DisplayName of the field, this is similar to `[Display]`. This also works on Enums    | Overrides the label of the field with the provided value                                                                                                                                                 |
| `[Description]` | Specifies the Description of the field, this is similar to `[Display]`                              | Adds the provided value after the field as muted text                                                                                                                                                    |
| `[Placeholder]` | Specifies the Placeholder of the field, default placeholder text is "Please enter {DisplayName}..." | Adds the provided value as the placeholder of the field                                                                                                                                                  |
| `[DataList]`    | Specifies the DataList for the field, the data list be must be defined in the document              | Adds the provided value as the placeholder of the field                                                                                                                                                  |
| `[DataType]`    | Specifies the DataType of the field. See supported DataTypes below.                                 | Adds the appropriate value as the type attribute of the input field                                                                                                                                      |
| `[DateType]`    | Specifies the DateType of the field. See supported DateTypes below.                                 | Adds the appropriate value as the type attribute of the input field                                                                                                                                      |
| `[Range]`       | Specifies the range of values for a numeric field.                                                  | Adds the appropriate value as the min and max attributes of the input field, and transforms this input into a bootstrap Slider Range                                                                     |

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
| `DataType("custom")`                  | `custom`            |

!!! info "Custom data types"

    Custom data types are useful for `hidden`, `search`, or other relatively-rare input types.

## DateTypes

| DateType                      | HTML type attribute |
|-------------------------------|---------------------|
| `InputDateType.Date`          | `date`              |
| `InputDateType.Time`          | `time`              |
| `InputDateType.DateTimeLocal` | `datetime-local`    |
| `InputDateType.Month`         | `month`             |