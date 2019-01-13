using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoeKingAPI.Models.ViewModels
{
    public class AboutVm
    {
        public string FirstPic { get; set; }
        public string FirstPicUpdatedby { get; set; }
        public string FirstPicUpdatedAt { get; set; }
        public string TextFirstHeader { get; set; }
        public string TextFirst { get; set; }
        public string UpdatedFirstTextBy { get; set; }
        public string UpdatedFirstTextAt { get; set; }
        public string SecondPic { get; set; }
        public string SecondPicUpdatedby { get; set; }
        public string SecondPicUpdatedAt { get; set; }
        public string TextSecondHeader { get; set; }
        public string TextSecond { get; set; }
        public string TextSecondUpdatedBy { get; set; }
        public string TextSecondUpdatedAt { get; set; }
    }
}