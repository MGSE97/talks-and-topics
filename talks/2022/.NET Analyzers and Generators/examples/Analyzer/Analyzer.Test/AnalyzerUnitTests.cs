using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Analyzer.Test.CSharpCodeFixVerifier<
    Analyzer.EndpointAnalyzer,
    Analyzer.EndpointCodeFixProvider>;

namespace Analyzer.Test
{
    [TestClass]
    public class AnalyzerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task Nothing()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task NothingIfEndpointIsValid()
        {
            var test = @"
using System;

class Route : Attribute { public Route(string pattern) { } }

namespace API.Handlers
{
    [Route(""/kenobi"")]
    public class {|#0:HelloEndpoint|}
    {
        public string Get()
        {
            return ""Hello there!"";
        }
    }
}
";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task DiagnosticsIfRouteAttributeMissing()
        {
            var test = @"
namespace API.Handlers
{
    public class {|#0:HelloEndpoint|}
    {
        public string Get(string name)
        {
            return ""Hello there!"";
        }
    }
}
";

            var expected = VerifyCS.Diagnostic(EndpointAnalyzer.RouteRule.Id).WithLocation(0).WithArguments("Hello");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task DiagnosticsIfRouteParameterIsMissing()
        {
            var test = @"
using System;

class Route : Attribute { public Route(string pattern) { } }

namespace API.Handlers
{
    [{|#0:Route(""/kenobi"")|}]
    public class HelloEndpoint
    {
        public string Get({|#1:string miss|})
        {
            return ""Hello there!"";
        }
    }
}
";

            var expected = VerifyCS.Diagnostic(EndpointAnalyzer.ArgumentsRule.Id).WithLocation(1).WithLocation(0).WithArguments("Hello", "Get", "miss", "/kenobi");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task DiagnosticsIfRouteParameterMismatch()
        {
            var test = @"
using System;

class Route : Attribute { public Route(string pattern) { } }

namespace API.Handlers
{
    [{|#0:Route(""/kenobi/{hit}"")|}]
    public class HelloEndpoint
    {
        public string Get({|#1:string miss|})
        {
            return ""Hello there!"";
        }
    }
}
";

            var expected = VerifyCS.Diagnostic(EndpointAnalyzer.ArgumentsRule.Id).WithLocation(1).WithLocation(0).WithArguments("Hello", "Get", "miss", "/kenobi/{hit}");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task DiagnosticsIfWrongMethodName()
        {
            var test = @"
using System;

class Route : Attribute { public Route(string pattern) { } }

namespace API.Handlers
{
    [Route(""/kenobi"")]
    public class HelloEndpoint
    {
        public string {|#2:get|}()
        {
            return ""Hello there!"";
        }
    }
}
";

            var expected = VerifyCS.Diagnostic(EndpointAnalyzer.MethodNameRule.Id).WithLocation(2).WithArguments("Hello", "get", "Get");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task DiagnosticsIfMissingMethod()
        {
            var test = @"
using System;

class Route : Attribute { public Route(string pattern) { } }

namespace API.Handlers
{
    [Route(""/kenobi"")]
    public class {|#0:HelloEndpoint|}
    {
    }
}
";

            var expected = VerifyCS.Diagnostic(EndpointAnalyzer.MethodMissingRule.Id).WithLocation(0).WithArguments("Hello");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task DiagnosticsIfDifferentMethodName()
        {
            var test = @"
using System;

class Route : Attribute { public Route(string pattern) { } }

namespace API.Handlers
{
    [Route(""/kenobi"")]
    public class {|#0:HelloEndpoint|}
    {
        public string {|#2:Hash|}()
        {
            return ""Hello there!"";
        }
    }
}
";

            var expected = VerifyCS.Diagnostic(EndpointAnalyzer.MethodMissingRule.Id).WithLocation(0).WithArguments("Hello", "get");
            await VerifyCS.VerifyAnalyzerAsync(test, expected);
        }
    }
}
