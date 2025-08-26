using Microsoft.Extensions.Hosting;
using MoneyMarket.Application.Common.Abstractions;

namespace MoneyMarket.Infrastructure.Files
{
    public sealed class LocalFileStorage : IFileStorage
    {
        private readonly IHostEnvironment _env;
        public LocalFileStorage(IHostEnvironment env) => _env = env;

        public async Task<string> UploadAsync(string container, string path, Stream content, string contentType, CancellationToken ct)
        {
            var root = Path.Combine(_env.ContentRootPath, container);
            Directory.CreateDirectory(root);
            var fullPath = Path.Combine(root, path.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await content.CopyToAsync(fs, ct);
            // Return relative path like "images/profiles/uid/photo.jpg"
            return Path.Combine(container, path).Replace("\\", "/");
        }
    }
}

