using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace FunPart1;

public class Function1(ILogger<Function1> logger)
{

    static ILogger<Function1> slogger;


    [Function("Blaat")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route=@"blah/{name}")] HttpRequest req, string name)
    {
        logger.LogInformation($"C# HTTP trigger function processed a request. [{name}]");
        return new OkObjectResult($"Welcome to Azure {name} Functions!");
    }

    [Function("Blaat2")]
    public IActionResult Run2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = @"blah2/{name}")] HttpRequest req, string name)
    {
        logger.LogInformation($"C# HTTP trigger function processed a request. [{name}]");
        return new OkObjectResult($"Welcome to Azure {name} Functions!");
    }
    [Function("Blaat3")]
    public static IActionResult Run3(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = @"blah3/{name}")] HttpRequest req, string name)
    {
        //logger.LogInformation($"C# HTTP trigger function processed a request. [{name}]");
        return new OkObjectResult($"Welcome to Azure {name} Functions!");
    }
}
