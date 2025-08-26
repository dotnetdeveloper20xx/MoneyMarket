namespace MoneyMarket.Application.Common.Abstractions
{
    public interface IFileStorage
    {
        Task<string> UploadAsync(string container, string path, 
            Stream content, string contentType, CancellationToken ct);
    }
}
