using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Desktop.Services;

public static class AppServices
{
    public static AppSession Session { get; } = new();
    public static LaboratoryApiClient Api { get; private set; } = null!;

    public static void Initialize()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location)!)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var baseUrl = config["ApiBaseUrl"] ?? "http://localhost:5037";
        var http = new HttpClient { BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/") };
        Api = new LaboratoryApiClient(http);
    }
}
