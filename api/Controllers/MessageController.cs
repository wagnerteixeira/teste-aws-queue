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
}
