using Microsoft.AspNetCore.Components;

namespace API.Endpoints
{
    // Start with this
    // You should see:
    //  - On class:
    //      - AG1 Endpoint 'HelloWorld' must have Route attibute.
    //      - AG2 Endpoint 'HelloWorld' must have Get method.
    public class HelloWorld1Endpoint
    {
    }

    // Start with this
    // You should see:
    //  - On class:
    //      - AG2 Endpoint 'HelloWorld' must have Get method.
    //  - On 'get' method:
    //      - AG3 Endpoint 'HelloWorld2' method 'GET' should be named 'Get'.
    public class HelloWorld2Endpoint
    {
        public string GET()
        {
            return "Hello World!";
        }
    }

    // Result (not exact)
    [Route("/hello-world")]
    public class HelloWorldResultEndpoint
    {
        public string Get()
        {
            return "Hello World!";
        }
    }
}
