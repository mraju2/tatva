using chat_service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace chatservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendChatRequest([FromBody] UiRequest request)
    {
        try
        {
            var result = await _chatService.SendChatRequestAsync(request);
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
