using System.Text.Json;
using Core.Entities;
using Microsoft.Extensions.Options;
using SguScheduleAPNs.NotificationServer.HttpServices.Interfaces;
using SguScheduleAPNs.NotificationServer.Options;

namespace SguScheduleAPNs.NotificationServer.HttpServices;

public class DeviceManagerHttpService: IDeviceManagerHttpService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public DeviceManagerHttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Device>> GetAllDevicesAsync(CancellationToken token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/DeviceManager/GetAllDevices");
        
        var response = await _httpClient.SendAsync(request, token);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<Device>>(_jsonSerializerOptions, token);
        return result;
    }
}