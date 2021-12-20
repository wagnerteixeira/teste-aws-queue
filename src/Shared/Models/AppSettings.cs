#nullable disable
namespace Shared.Models
{
    public record QueueSettings
    {
        public string Url { get; init; } = String.Empty;

        public string UrlDlq { get; init; } = String.Empty;

        public bool Fifo { get; set; }
    }

    public record AppSettings
    {
        public QueueSettings Queue { get; set; }
        public string DynamoDbTable { get; init; } = String.Empty;
    }
}