# ASP.NET Core Blazor `QuickForm` component V2.0.0

The QuickForm component is a generic Razor component for quickly and efficiently generating HTML forms.
QuickForm provides a simple and convenient form component for common form rendering scenarios,
while having plenty of customization options with data annotation attributes. It has two validation schemes:
Fluent (using [Blazored.FluentValidation](https://github.com/Blazored/FluentValidation))
and DataAnnotations (using the built-in `DataAnnotationValidator` component).

---

## Package

Add a package reference for the [`QuickForm`](https://www.nuget.org/packages/QuickForm) package.
You can also use the .NET CLI to add the package reference
with: [`dotnet add package` command](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package).

[//]: # (---)

[//]: # (## Implementation)

[//]: # ()
[//]: # (To implement the `QuickForm` component:)

[//]: # ()
[//]: # (1. Specify tag for the `QuickForm` component in Razor markup &#40;`<QuickForm />`&#41;.)

[//]: # (2. Use the following Parameters:)

[//]: # (    * `T Model`: supply a value for which you want to generate the form for.)

[//]: # (    * `OnModelChange?`: An optional event callback that's invoked when the model changes.)

[//]: # (    * `OnValidSubmit?`: An optional event callback that's invoked when the form is submitted and passes validation.)

[//]: # (3. ***Done***! your quick form is set up in ***one line of code***!)

[//]: # (---)

[//]: # (## Basic usage)

[//]: # ()
[//]: # (Start by add the following using statement to your root `_Imports.razor` or anywhere inside your component)

[//]: # ()
[//]: # (```razor)

[//]: # (@using QuickForm.Components)

[//]: # (```)

[//]: # ()
[//]: # (Here is a very basic form:)

[//]: # ()
[//]: # ([BasicForm.razor]&#40;./samples/BasicForm.razor&#41;:)

[//]: # ()
[//]: # (```razor)

[//]: # (@using QuickForm.Components)

[//]: # ()
[//]: # (<QuickForm Model="Model" />)

[//]: # ()
[//]: # (@code {)

[//]: # (    )
[//]: # (    public LoginCommand Model { get; set; } = new LoginCommand&#40;&#41;;)

[//]: # (    )
[//]: # (})

[//]: # (```)

[//]: # ()
[//]: # (```csharp)

[//]: # (public class LoginCommand)

[//]: # ({)

[//]: # (    public string Email { get; set; })

[//]: # ()
[//]: # (    public string Password { get; set; })

[//]: # (})

[//]: # (```)

[//]: # ()
[//]: # (![BasicForm.png]&#40;https://raw.githubusercontent.com/ddjerqq/QuickForm/master/assets/BasicForm.razor.png&#41;)

[//]: # ()
[//]: # (---)

[//]: # ()
[//]: # (We can customize the properties and the generated fields, with data annotation attributes:)

[//]: # ()
[//]: # ([CustomizedBasicForm.razor]&#40;./samples/CustomizedBasicForm.razor&#41;:)

[//]: # ()
[//]: # (```razor)

[//]: # (@using QuickForm.Components)

[//]: # ()
[//]: # (<QuickForm Model="Model" />)

[//]: # ()
[//]: # (@code {)

[//]: # ()
[//]: # (    public LoginCommand Model { get; set; } = new LoginCommand&#40;&#41;;)

[//]: # ()
[//]: # (})

[//]: # (```)

[//]: # ()
[//]: # (```csharp)

[//]: # (using QuickForm.Attributes;)

[//]: # ()
[//]: # (public class LoginCommand)

[//]: # ({)

[//]: # (    [Required])

[//]: # (    [Placeholder])

[//]: # (    [DisplayName&#40;"Email Address"&#41;])

[//]: # (    [EmailAddress])

[//]: # (    public string Email { get; set; })

[//]: # ()
[//]: # (    [Required])

[//]: # (    [Placeholder&#40;"Enter password..."&#41;])

[//]: # (    [DataType&#40;DataType.Password&#41;])

[//]: # (    [Description&#40;"Password must have at least 8 characters, one uppercase letter, one lowercase letter ..."&#41;])

[//]: # (    [RegularExpression&#40;@"^&#40;?=.*[a-z]&#41;&#40;?=.*[A-Z]&#41;.{8,32}$", ErrorMessage = "Password must have at least 8 characters, one uppercase letter, one lowercase letter ..."&#41;])

[//]: # (    public string Password { get; set; })

[//]: # (})

[//]: # (```)

[//]: # ()
[//]: # (![CustomizedBasicForm_before_validation.png]&#40;https://raw.githubusercontent.com/ddjerqq/QuickForm/master/assets/CustomizedBasicForm.razor_before_validation.png&#41;)

[//]: # ()
[//]: # (![CustomizedBasicForm_after_validation.png]&#40;https://raw.githubusercontent.com/ddjerqq/QuickForm/master/assets/CustomizedBasicForm.razor_after_validation.png&#41;)

[//]: # ()
[//]: # (---)

[//]: # ()
[//]: # (## Full list of supported DataAnnotations to customize the generated forms)

[//]: # ()
[//]: # (| Attribute       | Description                                                                                         | Input tag modification                                                                                                               |)

[//]: # (|-----------------|-----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------|)

[//]: # (| `[Required]`    | Specifies that a data field value is required.                                                      | Adds required attribute and a red * to the form.                                                                                     |)

[//]: # (| `[Editable]`    | Specifies whether or not the field should be editable.                                              | Adds disabled and readonly attributes to the input, as well as removing the event callbacks for when the value changes               |)

[//]: # (| `[NotMapped]`   | Specifies that the property should be skipped during form generation                                | -                                                                                                                                    |)

[//]: # (| `[Display]`     | Specifies the DisplayName and the Description from on attribute                                     | Adds the `DisplayName` as the label of the field and `Description` after the field as muted text &#40;if any&#41;.                           |)

[//]: # (| `[DisplayName]` | Specifies the DisplayName of the field, this is similar to `[Display]`. This also works on Enums    | Overrides the label of the field with the provided value                                                                             |)

[//]: # (| `[Description]` | Specifies the Description of the field, this is similar to `[Display]`                              | Adds the provided value after the field as muted text                                                                                |)

[//]: # (| `[Placeholder]` | Specifies the Placeholder of the field, default placeholder text is "Please enter {DisplayName}..." | Adds the provided value as the placeholder of the field                                                                              |)

[//]: # (| `[DataList]`    | Specifies the DataList for the field, the data list be must be defined in the document              | Adds the provided value as the placeholder of the field                                                                              |)

[//]: # (| `[DataType]`    | Specifies the DataType of the field. See supported DataTypes below.                                 | Adds the appropriate value as the type attribute of the input field                                                                  |)

[//]: # (| `[Range]`       | Specifies the range of values for a numeric field.                                                  | Adds the appropriate value as the min and max attributes of the input field, and transforms this input into a bootstrap Slider Range |)

[//]: # ()
[//]: # (### Supported DataTypes and their appropriate html type attributes:)

[//]: # ()
[//]: # (* `DataType.Date` => `"date"`)

[//]: # (* `DataType.Time` => `"time"`)

[//]: # (* `DataType.DateTime` => `"datetime-local"`)

[//]: # (* `DataType.EmailAddress` => `"email"`)

[//]: # (* `DataType.Password` => `"password"`)

[//]: # (* `DataType.PhoneNumber` => `"tel"`)

[//]: # (* `DataType.Url` or `DataType.ImageUrl` => `"url"`)

[//]: # (* `DataType.MultilineText` => `null`)

[//]: # ()
[//]: # (---)

[//]: # ()
[//]: # (There aren't current plans to extend `QuickForm` with features that full-blown commercial form generators tend to offer,)

[//]: # (for example, bootstrap input groups, color pickers, hierarchical dropdowns, etc.)

[//]: # (However, if you think that a certain feature would make sense in this component, feel free to open an issue on github)

[//]: # (or even a Pull Request, it would be massively appreciated!)

[//]: # ()
[//]: # (---)

[//]: # ()
[//]: # (Thank you for using QuickForm!)

[//]: # ()
[//]: # (Thank you Anton from [Raw Coding]&#40;https://www.youtube.com/@RawCoding&#41; for inspiration about expressions.)

[//]: # ()
[//]: # (Thank you [meziantou]&#40;https://github.com/meziantou&#41; for [this amazing article]&#40;https://www.meziantou.net/automatically-generate-a-form-from-an-object-in-blazor.htm&#41; which inspired to me create this repo.)

[//]: # ( )