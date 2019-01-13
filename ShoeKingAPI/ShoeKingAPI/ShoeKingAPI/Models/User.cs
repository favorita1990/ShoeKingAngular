using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoeKingAPI.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Galleries = new HashSet<Gallery>();
            this.Messages = new HashSet<Message>();
            this.Orders = new HashSet<Order>();
            this.Comments = new HashSet<Comment>();
            this.Ratings = new HashSet<Rating>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImageUrl { get; set; }
        public string CoverUrl { get; set; }
        public string ConnectionId { get; set; }
        public bool OnlineOrOffline { get; set; }
        public string ChattingWithUserId { get; set; }
        public bool? ProfileMainStatus { get; set; }
        public bool? ProfilePhotosStatus { get; set; }

        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}