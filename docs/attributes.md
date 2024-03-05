# Customizing with attributes

The following chapter lists all the available attributes and their generated markup

##### `[Required]` {collapsible="true"}

> This attribute marks the field as required,
> and shows errors if the field is empty during submit.
> {style="note"}

Code

```C#
[Required]
public string Field { get; set; }
```

Resulting markup

```html
<input class="input" required/>
<div class="invalid-feedback">Field field is required.</div>
```

##### `[NotMapped]` {collapsible="true"}

> This attribute marks the field as ignored.
> {style="note"}

##### `[Display]`, `[DisplayName]` and `[Description]` {collapsible="true"}

> These allow us to specify how the field appears
> {style="note"}

Code

```C#
[Display(Name="name", Description="description")]
// this is the same as doing
[DisplayName("name")]
[Description("description")]
public string Field { get; set; }
```

Resulting markup

```html
<label>name</label>
<input .../>
<div>description</div>
```

## Full list of supported DataAnnotations to customize the generated forms

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
| `[Range]`       | Specifies the range of values for a numeric field.                                                  | Adds the appropriate value as the min and max attributes of the input field, and transforms this input into a bootstrap Slider Range |

### Supported DataTypes and their appropriate html type attributes:

| DataType                              | HTML type attribute |
|---------------------------------------|---------------------|
| `DataType.Date`                       | `"date"`            |
| `DataType.Time`                       | `"time"`            |
| `DataType.DateTime`                   | `"datetime-local"`  |
| `DataType.EmailAddress`               | `"email"`           |
| `DataType.Password`                   | `"password"`        |
| `DataType.PhoneNumber`                | `"tel"`             |
| `DataType.Url` or `DataType.ImageUrl` | `"url"`             |
| `DataType.MultilineText`              | `null`              |