using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoeKingAPI.Models
{
    public class HomePage
    {
        [Key]
        public int Id { get; set; }
        public string TextHeader { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
    }
}