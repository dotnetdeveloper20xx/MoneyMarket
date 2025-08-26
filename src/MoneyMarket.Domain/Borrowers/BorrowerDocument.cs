namespace MoneyMarket.Domain.Borrowers
{
    public sealed class BorrowerDocument
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DocumentType Type { get; private set; }
        public string FileName { get; private set; } = default!;
        public string StoragePath { get; private set; } = default!; // URL or local relative path
        public DateTime UploadedAtUtc { get; private set; }

        private BorrowerDocument() { }
        public BorrowerDocument(DocumentType type, string fileName, string storagePath, DateTime now)
        { Type = type; FileName = fileName; StoragePath = storagePath; UploadedAtUtc = now; }
    }
}
