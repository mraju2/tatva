using ChatService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ChatService.Services.Interfaces;

namespace ChatService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatMessageService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatMessageService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        /// <summary>
        /// Sends a chat request to the chat service.
        /// </summary>
        /// <param name="request">The chat request object.</param>
        /// <returns>The response from the chat service.</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendChatRequest([FromBody] UserChatRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _chatService.SendChatRequestAsync(request);
                return Ok(new
                {
                    Success = true,
                    Data = result
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An HTTP request error occurred.");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request. Please try again later."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
