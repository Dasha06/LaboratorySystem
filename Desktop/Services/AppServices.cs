using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Desktop.Services;

public static class AppServices
{
    public static AppSession Session { get; } = new();
    public static LaboratoryApiClient Api { get; private set; } = null!;
    private static readonly string HostFilePath =
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location)!, "host.txt");

    public static string BaseUrl { get; private set; } = "http://localhost:5037";

    public static void Initialize()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly()!.Location)!)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var defaultUrl = config["ApiBaseUrl"] ?? "http://localhost:5037";

        if (File.Exists(HostFilePath))
        {
            var saved = File.ReadAllText(HostFilePath).Trim();
            if (!string.IsNullOrWhiteSpace(saved))
                defaultUrl = saved;
        }

        BaseUrl = defaultUrl;
        BuildApiClient(BaseUrl);
    }

    public static void ReinitializeApi(string baseUrl)
    {
        BaseUrl = baseUrl;
        BuildApiClient(baseUrl);
        File.WriteAllText(HostFilePath, baseUrl);
    }

    private static void BuildApiClient(string baseUrl)
    {
        var http = new HttpClient { BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/") };
        Api = new LaboratoryApiClient(http);
    }
}
