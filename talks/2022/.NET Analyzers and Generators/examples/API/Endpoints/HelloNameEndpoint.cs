using Microsoft.AspNetCore.Components;

namespace API.Endpoints
{
    // Start with this
    // You should see:
    //  - On class: AG1 Endpoint 'HelloNameFromFn' must have Route attibute.
    //  - On attribute after fix: AG4 Endpoint 'HelloNameFromFn' method 'Get' argument 'name' should be in route '/hellonamefromfn'.
    public class HelloNameFromFnEndpoint
    {
        public string Get(string name)
        {
            return $"Hello {name}!";
        }
    }

    // Start with this
    // You should see:
    //  - On class: AG2 Endpoint 'HelloNameFromRoute' must have Get method.
    [Route("/hello/{name}")]
    public class HelloNameFromRouteEndpoint
    {
    }

    // Result (not exact)
    [Route("/hello/{name}")]
    public class HelloNameResultEndpoint
    {
        public string Get(string name)
        {
           return $"Hello {name}!";
        }
    }
}
