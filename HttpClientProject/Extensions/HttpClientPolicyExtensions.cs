using HttpClientProject.Service;
using Polly;

namespace HttpClientProject.Extensions
{
    //public static class HttpClientPolicyExtensions
    //{

    //    public static IHttpClientBuilder AddPolicies(this IHttpClientBuilder builder, ILogger<ApiService> logger)
    //    {
    //        var retryPolicy = Policy
    //            .Handle<HttpRequestException>()
    //            .OrResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.NotFound)
    //            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
    //                (result, timespan, retryCount, context) =>
    //                {
    //                    logger.LogWarning($"Retrying {retryCount} time after {timespan.TotalSeconds} seconds due to error: {result.Exception?.Message ?? result.Result.ReasonPhrase}");
    //                });

    //        var circuitBreakerPolicy = Policy
    //            .Handle<HttpRequestException>()
    //            .OrResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500)
    //            .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30),
    //                onBreak: (result, breakDelay) =>
    //                {
    //                    logger.LogError($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {result.Exception?.Message ?? result.Result.ReasonPhrase}");
    //                },
    //                onReset: () => logger.LogInformation("Circuit breaker reset."),
    //                onHalfOpen: () => logger.LogInformation("Circuit breaker is half-open, next call is a trial."));

    //        return builder
    //            .AddPolicyHandler(retryPolicy)
    //            .AddPolicyHandler(circuitBreakerPolicy);
    //    }
    //}
    public static class HttpClientPolicyExtensions
    {
        public static IHttpClientBuilder AddPolicies(this IHttpClientBuilder builder, Func<IServiceProvider, ILogger<ApiService>> loggerFactory)
        {
            builder.Services.AddTransient(provider =>
            {
                var logger = loggerFactory(provider);

                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .OrResult<HttpResponseMessage>(r => r.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (result, timespan, retryCount, context) =>
                        {
                            logger.LogWarning($"Retrying {retryCount} time after {timespan.TotalSeconds} seconds due to error: {result.Exception?.Message ?? result.Result.ReasonPhrase}");
                        });

                var circuitBreakerPolicy = Policy
                    .Handle<HttpRequestException>()
                    .OrResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500)
                    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30),
                        onBreak: (result, breakDelay) =>
                        {
                            logger.LogError($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {result.Exception?.Message ?? result.Result.ReasonPhrase}");
                        },
                        onReset: () => logger.LogInformation("Circuit breaker reset."),
                        onHalfOpen: () => logger.LogInformation("Circuit breaker is half-open, next call is a trial."));

                builder.AddPolicyHandler(retryPolicy).AddPolicyHandler(circuitBreakerPolicy);

                return builder;
            });

            return builder;
        }
    }
}
