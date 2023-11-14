using NUnit.Framework;

namespace QuickForm.Test;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class BaseTest : Bunit.TestContext
{
    protected const string IdRegex = "[0-9a-f]{8}_[a-zA-Z]+";

    protected static string Id(string id) => $"[0-9a-f]{{8}}_{id}";
}