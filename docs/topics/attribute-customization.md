# Customizing with attributes

The following chapter lists all the available attributes and their generated markup

##### `[Required]` {collapsible="true"}

> This attribute marks the field as required, 
> and shows errors if the field is empty during submit.
{style="note"}

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
{style="note"}

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
<input ... />
<div>description</div>
```