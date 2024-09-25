using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace HttpClientProject.Service
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        //public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
        {
            //_httpClient = httpClient;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var fullUrl = _httpClient.BaseAddress + url;
                var response = await _httpClient.GetAsync(fullUrl);

                response.EnsureSuccessStatusCode();

                //return await response.Content.ReadFromJsonAsync<T>();
                var result = await response.Content.ReadFromJsonAsync<T>();
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize the response content.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing GET request");
                throw;
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestData)
        {
            try
            {
                var fullUrl = _httpClient.BaseAddress + url;
                var response = await _httpClient.PostAsJsonAsync(fullUrl, requestData);
                response.EnsureSuccessStatusCode();

                //return await response.Content.ReadFromJsonAsync<TResponse>();
                var result = await response.Content.ReadFromJsonAsync<TResponse>();
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize the response content.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing POST request");
                throw;
            }
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest requestData)
        {
            try
            {
                var fullUrl = _httpClient.BaseAddress + url;
                var response = await _httpClient.PutAsJsonAsync(fullUrl, requestData);
                response.EnsureSuccessStatusCode();

                //return await response.Content.ReadFromJsonAsync<TResponse>();
                var result = await response.Content.ReadFromJsonAsync<TResponse>();
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize the response content.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing PUT request");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string url)
        {
            try
            {
                var fullUrl = _httpClient.BaseAddress + url;
                var response = await _httpClient.DeleteAsync(fullUrl);
                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing DELETE request");
                throw;
            }
        }
    }
}
