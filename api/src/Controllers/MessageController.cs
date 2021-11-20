using api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

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

    [HttpPost("create-messages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMessages()
    {
        var messagesTaks = Enumerable.Range(0, 100).Select(async _ => await GetRamdomGuid());
        var messages = await Task.WhenAll(messagesTaks);
        var parts = messages.Chunk(10);

        foreach (var part in parts)
        {
            await _awsRepository.SendMessagesAsync(part);
        }
        return Ok($"Messages sent {messages.Count()}");
    }

    private async Task<string> GetRamdomGuid()
    {
        return await Task.FromResult(Guid.NewGuid().ToString());
    }
}
