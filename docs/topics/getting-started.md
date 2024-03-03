# About %product%

%product% is an ASP.NET Blazor component library project, used to create complex
input forms effortlessly!

## Example usage

<procedure>
<step>
<p>In your <code>@_Imports.razor</code> file, or your component file, import the QuickForm library with <code>@using QuickForm.Components</code></p>
</step>
<step>
<p>Add the <code>&lt;QuickForm /&gt;</code> component in your markup</p>
</step>
<step>
<p>Define the model for which you want to use the QuickForm for</p>
<code-block lang="c#">
<![CDATA[
public class LoginCommand
{
    public string Email { get; set; }
    public string Password { get; set; }
}
]]>
</code-block>
</step>
<step>
    <p>Define the model in your component</p>
<code-block lang="C#">
<![CDATA[
@code {
    public LoginCommand Model { get; set; } = new();

        public void OnValidSubmit()
        {
            // your logic here
        }
    }

]]>
</code-block>
</step>
<step>
<p>Pass the model to QuickForm</p>
<code-block lang="razor">
&lt;QuickForm Model="Model" OnValidSubmit="OnValidSubmit" />
</code-block>
</step>
<step>
<p>The component will look something like this:</p>
<code-block lang="razor">
<![CDATA[
    @using QuickForm.Components

    <QuickForm Model="Model" OnValidSubmit="OnValidSubmit" />
    
    @code {
        public LoginCommand Model { get; set; } = new();

        public void OnValidSubmit()
        {
            // your logic here
        }
    }

]]>
</code-block>
</step>
<step>
<p>The output will look like this:</p>
<img src="simple-form.png" alt="simple form" />
<p>Not really stylish, is it? but its bare bones!</p>
</step>
<step>
<p>If you are using Bootstrap in your project, you can use the <code>&lt;BsQuickForm /></code></p>
<code-block lang="razor">
    &lt;BsQuickForm ... />
</code-block>
<warning><b>Make sure to also import</b> <code>QuickForm.Components.Bootstrap</code></warning>
<p>The output will look like this:</p>
<img src="simple-form-bs.png" alt="simple form bootstrap version" />
</step>
</procedure>
