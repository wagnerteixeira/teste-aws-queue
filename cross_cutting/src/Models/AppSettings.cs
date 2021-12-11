#nullable disable
namespace CrossCutting.Models
{
    public record QueueSettings
    {
        public string Url { get; init; } = String.Empty;
        public bool Fifo { get; set; }
    }

    public record AppSettings
    {
        public QueueSettings Queue { get; set; }
    }
}