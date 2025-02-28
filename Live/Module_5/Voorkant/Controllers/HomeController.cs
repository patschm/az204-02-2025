using System.Diagnostics;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Voorkant.Models;

namespace Voorkant.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //private readonly IHttpClientFactory _clientFactory;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        //_clientFactory = clientFactory;
    }

    public async Task<IActionResult> Index()
    {
        // var client = _clientFactory.CreateClient("weer");
        // var response = await client.GetAsync("weatherforecast");
                
        var client = DaprClient.CreateInvokeHttpClient(appId: "backend");
        var cts = new CancellationTokenSource();

        var response = await client.GetAsync("weatherforecast", cts.Token);
        //var response = await client.PostAsJsonAsync("/orders", order, cts.Token);
        //Console.WriteLine("Order passed: " + order);

        _logger.LogInformation(client.BaseAddress.ToString());
        _logger.LogInformation("Response: {0}", response);
        _logger.LogError(response.Content.ReadAsStringAsync().Result);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            return View("Index", content);
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
