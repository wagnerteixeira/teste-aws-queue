using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;

    private readonly IAwsRepository _awsRepository;

    public MessageController(ILogger<MessageController> logger, IAwsRepository awsRepository)
    {
        _logger = logger;
        _awsRepository = awsRepository;
    }

    [HttpPost("send-messages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendMessages(IList<string> messages)
    {
        var parts = messages.Chunk(10);
        foreach (var part in parts)
        {
            await _awsRepository.SendMessagesAsync(part);
        }

        return Ok();
    }

    [HttpPost]
    [Route("create-messages/{quantidade}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMessages(int quantidade)
    {
        Console.Write(quantidade);
        var messagesTaks = Enumerable.Range(0, quantidade).Select(_ => Task.Run(() => GetRamdomGuid()));
        var messages = await Task.WhenAll(messagesTaks);
        var parts = messages.Chunk(10);

        foreach (var part in parts)
        {
            await _awsRepository.SendMessagesAsync(part);
        }
        return Ok($"Messages sent {messages.Count()}");
    }

    private string GetRamdomGuid()
    {
        return Guid.NewGuid().ToString();
    }
}
