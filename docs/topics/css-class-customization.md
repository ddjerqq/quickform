# Customizing CSS classes

The default %product% component is very plain, it is bare-bones HTML with no css classes attached.

There are many ways to customize the CSS classes that are applied to the form:

- Using Attributes
- Using CSS Class providers

## Before you start

Make sure that:

- %product% package is added to your project.
- %product% is properly imported.

## Attributes

There are three elements in a %product% field: Editor, Label and Input

### To customize each field separately:

1. Import `QuickForm.Attributes`

```C#
using QuickForm.Attributes;
```

2. Give the attributes to your classes

```C#
public class Model
{
   [EditorClass("editor")]
   [LabelClass("label")]
   [InputClass("input")]
   public string Field { get; set; }
}
```

3. The following code will result in the markup shown below

```html

<form>
    <div class="editor">
        <label class="label">...</label>
        <input class="input"/>
    </div>
</form>
```

> Optionally, pass the FormClass parameter to `<QuickForm />`
> ```Razor
> <QuickForm FormClass="form w-50" />
> ```
> Resulting markup:
> ```html
> <form class="form w-50">
>     ...
> </form>
> ```
{style="note"}

## CustomCssClassProviders

You can use the `CustomQuickFormFieldCssClassProvider` in the namespace `QuickForm.Common`
to customize the classes for all fields.

### Example usage:

```C#
<QuickForm Model="model" FieldCssClassProvider="ClassProvider" />

@code 
{
   public IQuickFormFieldCssClassProvider ClassProvider 
      = new CustomQuickFormFieldCssClassProvider("editor", "label", "input");
}
```

> The following code will result in the markup shown below
> ```html
> 
> <form>
>   <div class="editor">
>     <label class="label">...</label>
>     <input class="input"/>
>   </div>
> </form>
> ```

#### There is an alternative way to use the CustomQuickFormCssClassProvider

```C#
<QuickForm Model="model" FieldCssClassProvider="ClassProvider" />

@code 
{
   public IQuickFormFieldCssClassProvider ClassProvider 
       = new CustomQuickFormFieldCssClassProvider
       {
           Editor = f => f.PropertyInfo.PropertyType == typeof(int) ? "editor-int" : "editor",
           Label = f => f.PropertyInfo.PropertyType == typeof(int) ? "label-int" : "label",
           Input = f => f.PropertyInfo.PropertyType == typeof(int) ? "input-int" : "input",
       };
}
```

> The following code will result in the markup shown below
> ```html
> 
> <form>
>   <div class="editor">
>     <label class="label">...</label>
>     <input class="input"/>
>   </div>
> 
>   <div class="editor-int">
>       <label class="label-int">...</label>
>       <input class="input-int"/>
>   </div>
> </form>
> ```
