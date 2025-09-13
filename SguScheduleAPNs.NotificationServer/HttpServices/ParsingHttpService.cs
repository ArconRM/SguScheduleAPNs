using System.Text.Json;
using Microsoft.Extensions.Options;
using SguScheduleAPNs.NotificationServer.Entities;
using SguScheduleAPNs.NotificationServer.HttpServices.Interfaces;
using SguScheduleAPNs.NotificationServer.Options;

namespace SguScheduleAPNs.NotificationServer.HttpServices;

public class ParsingHttpService: IParsingHttpService
{
    private readonly string _baseSiteUrl;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public ParsingHttpService(IOptions<ParsingServiceOptions> options, HttpClient httpClient)
    {
        _baseSiteUrl = options.Value.BaseSiteUrl;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Lesson>> ParseLessonsInGroupAsync(string groupUrl, CancellationToken token)
    {
        string fullUrl = $"{_baseSiteUrl}/{groupUrl}";
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/schedule/");
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(fullUrl), "url");
        request.Content = content;
        
        var response = await _httpClient.SendAsync(request, token);
        response.EnsureSuccessStatusCode();
        
        var lessons = await response.Content.ReadFromJsonAsync<IEnumerable<Lesson>>(_jsonSerializerOptions, token);
        return lessons;
    }
}