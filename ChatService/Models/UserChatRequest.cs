using System.ComponentModel.DataAnnotations;

namespace ChatService.Models
{
    /// <summary>
    /// Represents a chat request made by a user.
    /// </summary>
    public class UserChatRequest
    {
        /// <summary>
        /// The model to be used for generating responses.
        /// </summary>
        [Required]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// The message sent by the user.
        /// </summary>
        [Required]
        [StringLength(500, ErrorMessage = "User message cannot exceed 500 characters.")]
        public string UserMessage { get; set; } = string.Empty;
    }
}

