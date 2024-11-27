using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using Microsoft.Identity.Client;

namespace MvcClient2.Controllers;

public class ContatoController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ContatoController(ILogger<HomeController> logger)
    {
        _logger = logger;

    }
    public IActionResult Index()
    {
        var query = new QueryBuilder();
        query.Add("client_id", "xxxxxx");
        query.Add("scope", "api://xxxxxx/contato");
        query.Add("response_type", "code");
        query.Add("redirect_uri", "https://localhost:7080/contato/ContatoCallback");
        string url = $"https://login.microsoftonline.com/xxxxxxx/oauth2/v2.0/authorize{query.ToQueryString()}";
        return Redirect(url);
    }


    public async Task<IActionResult> ContatoCallback(string code)
    {
        var app = ConfidentialClientApplicationBuilder.Create("xxxx")
               .WithClientSecret("xxxx")
               .WithRedirectUri("https://localhost:7080/contato/ContatoCallback")
               .WithAuthority(new Uri("https://login.microsoftonline.com/xxxxx"))
               .Build();

        var result = await app.AcquireTokenByAuthorizationCode(new[] { "api://xxxxxx/contato" }, code).ExecuteAsync();

        var contatoClient = new HttpClient();
        contatoClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
        var response = await contatoClient.GetAsync("https://localhost:6001/contato");
        string retorno = await response.Content.ReadAsStringAsync();
        var contatos = JsonSerializer.Deserialize<List<Contato>>(retorno);
        return View("Index", contatos);
    }
}

public class Contato
{
    public string nome { get; set; }
    public string telefone { get; set; }

}