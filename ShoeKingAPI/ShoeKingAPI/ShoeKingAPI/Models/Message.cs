using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoeKingAPI.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateSentMessage { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public bool ReadOrUnread { get; set; }
        public string DeletedMessageFirst { get; set; }
        public string DeletedMessageSecond { get; set; }

        public virtual User User { get; set; }
    }
}