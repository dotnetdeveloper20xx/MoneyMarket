using Azure.Storage.Blobs;
using MoneyMarket.Application.Common.Abstractions;
namespace MoneyMarket.Infrastructure.Files
{

    public sealed class AzureBlobStorage : IFileStorage
    {
        private readonly BlobServiceClient _svc;
        public AzureBlobStorage(BlobServiceClient svc) => _svc = svc;

        public async Task<string> UploadAsync(string container, string path, Stream content, string contentType, CancellationToken ct)
        {
            var c = _svc.GetBlobContainerClient(container);
            await c.CreateIfNotExistsAsync(cancellationToken: ct);
            var blob = c.GetBlobClient(path);
            await blob.UploadAsync(content, overwrite: true, cancellationToken: ct);
            await blob.SetHttpHeadersAsync(new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
            return blob.Uri.ToString();
        }
    }
}
