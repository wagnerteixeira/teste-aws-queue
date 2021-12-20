namespace BusinessLogic.Models;

public class MessageModel
{
    public string Guid { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public DateTime ProcessTime { get; set; }
}