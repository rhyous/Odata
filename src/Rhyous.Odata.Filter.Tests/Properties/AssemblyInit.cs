using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;

[TestClass]
public class AssemblyInit
{
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        var culture = new CultureInfo("en-US");

        // Set default culture for new threads
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        // Set culture for the current thread (where tests run)
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}