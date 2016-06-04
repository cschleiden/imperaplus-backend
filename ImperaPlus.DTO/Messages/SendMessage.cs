using System.ComponentModel.DataAnnotations;
using ImperaPlus.DTO.Users;

namespace ImperaPlus.DTO.Messages
{
    public class SendMessage
    {   
        [Required]
        public UserReference To { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }
    }
}
