namespace Data.Repositories.Models;

public class MessageModel<T>
{
    public string Id { get; set; }
    public T? Data { get; set; }

    public MessageModel()
    {
        Id = String.Empty;
    }
    public MessageModel(string id, T data)
    {
        Id = id;
        Data = data;
    }
}