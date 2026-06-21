using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Desktop.Services;

public static class AppServices
{
    public static AppSession Session { get; } = new();
    public static LaboratoryApiClient Api { get; private set; } = null!;
    private static readonly string HostFilePath =
        Path.Combine(AppContext.BaseDirectory, "host.txt");

    public static string BaseUrl { get; private set; } = "http://localhost:5037";

    public static void Initialize()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
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
