using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.DTO.Messages
{
    public enum MessageFolder
    {
        None = 0,

        /// <summary>
        /// Player's inbox
        /// </summary>
        Inbox = 1,

        /// <summary>
        /// Player's sent images
        /// </summary>
        Sent = 2
    }
}
