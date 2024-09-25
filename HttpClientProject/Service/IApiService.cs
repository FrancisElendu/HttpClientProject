namespace HttpClientProject.Service
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string url);
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestData);
        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest requestData);
        Task<bool> DeleteAsync(string url);
    }
}
