using Polly;

namespace HttpClientProject.Service
{
    //TODO: Need to work on this
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        //public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
        {
            //_httpClient = httpClient;
            _httpClient = httpClientFactory.CreateClient("ApiHttpClientConfig");
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

        //public void ConfigurePolicies(IServiceCollection services, IHttpClientBuilder builder)
        //{
        //    var retryPolicy = Policy
        //        .Handle<HttpRequestException>() // Handles transient errors such as timeouts and network failures
        //        .OrResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.NotFound) // Handle specific HTTP status codes
        //        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        //            (result, timespan, retryCount, context) =>
        //            {
        //                // Log retry attempt
        //                _logger.LogWarning($"Retrying {retryCount} time after {timespan.TotalSeconds} seconds due to error: {result.Exception?.Message ?? result.Result.ReasonPhrase}");
        //            });

        //    var circuitBreakerPolicy = Policy
        //        .Handle<HttpRequestException>()
        //        .OrResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500) // Handles 5XX server errors
        //        .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30),
        //            onBreak: (result, breakDelay) =>
        //            {
        //                // Log circuit breaker activation
        //                _logger.LogError($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {result.Exception?.Message ?? result.Result.ReasonPhrase}");
        //            },
        //            onReset: () =>
        //            {
        //                // Log circuit breaker reset
        //                _logger.LogInformation("Circuit breaker reset.");
        //            },
        //            onHalfOpen: () =>
        //            {
        //                // Log circuit breaker half-open state
        //                _logger.LogInformation("Circuit breaker is half-open, next call is a trial.");
        //            });

        //    builder.Services.AddHttpClient<IApiService, ApiService>(client =>
        //    {
        //        client.BaseAddress = new Uri("https://api.example.com/");
        //        client.DefaultRequestHeaders.Add("Accept", "application/json");
        //        client.Timeout = TimeSpan.FromSeconds(60); // Optional timeout configuration
        //    })
        //    .AddPolicyHandler(retryPolicy) // Add retry policy
        //    .AddPolicyHandler(circuitBreakerPolicy) // Add circuit breaker policy
        //    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        //    {
        //        MaxConnectionsPerServer = 10  // Optional: limit max connections to server
        //    });
        //}
    }
}
