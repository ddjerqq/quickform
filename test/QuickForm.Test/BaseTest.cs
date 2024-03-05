using System.IO;
using AngleSharp.Html.Parser;
using Bunit;
using Bunit.Diffing;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace QuickForm.Test;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class BaseTest : TestContext
{
    private static readonly HtmlParser HtmlParser = new();

    protected const string IdRegex = "[0-9a-f]{8}_[a-zA-Z]+";

    protected void Print(IRenderedFragment cut)
    {
        var doc = HtmlParser.ParseDocument(cut.Markup);
        using var writer = new StringWriter();

        doc.ToHtml(writer, new DiffMarkupFormatter());

        NUnit.Framework.TestContext.Out.WriteLine(writer.ToString());
    }
}