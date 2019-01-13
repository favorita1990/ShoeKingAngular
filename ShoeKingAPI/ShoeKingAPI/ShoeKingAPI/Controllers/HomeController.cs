using Microsoft.AspNet.Identity;
using ShoeKingAPI.Context;
using ShoeKingAPI.Models;
using ShoeKingAPI.Models.ViewModels;
using ShoeKingAPI.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ShoeKingAPI.Controllers
{
    public class HomeController : ApiController
    {
        private readonly ContextDB db = new ContextDB();
        private readonly ShoppingCart shoppingCartRepository = new ShoppingCart();
        private readonly ProductRepository productRepository = new ProductRepository();
        private readonly BreadCrumbRepository breadCrumbRepository = new BreadCrumbRepository();
        private readonly string AboutFolder = "../../src/assets/images/about/";
        private readonly string AboutFolderPath = "F:/favorita/Angular/onlineshoeking/ShoeKingAngular/src/assets/images/about/";
        private readonly string HomeFolder = "../../src/assets/images/home-page/";
        private readonly string HomeFolderPath = "F:/favorita/Angular/onlineshoeking/ShoeKingAngular/src/assets/images/home-page/";
        //private readonly string profilePicWoman = "../../images/profilePicWoman.jpg";
        //private readonly string directoryToProfilePicture = "../../assets/UsersImages/";
        //private string directoryOfProductIco = "../../assets/images/picProducts.ico";
        //private string getCustomersPicPath = "../../assets/images/picPeople.png";

        [HttpGet]
        [Route("api/GetHomePage")]
        public HomePageViewModel GetHomePage()
        {
            var homePageDb = this.db.HomePage.FirstOrDefault();
            var homePage = new HomePageViewModel()
            {
                Text = homePageDb.Text,
                TextHeader = homePageDb.TextHeader,
                ImageUrl = homePageDb.ImageUrl != "" ? HomeFolder + homePageDb.ImageUrl : ""
            };

            return homePage;
        }

        [HttpGet]
        [Route("api/GetNewArrivals")]
        public List<HomeNewArrivalsViewModel> GetNewArrivals()
        {
            var newArrivals = this.productRepository.GetNewArrivals();

            return newArrivals;
        }

        [HttpGet]
        [Route("api/GetMostBought")]
        public List<HomeMostBoughtViewModel> GetMostBought()
        {
            var discounts = productRepository.GetMostBought();

            return discounts;
        }

        [HttpGet]
        [Route("api/IsValidEmail")]
        public bool IsValidEmail(string source)
        {
            bool isvalid = new EmailAddressAttribute().IsValid(source.Trim());

            if (!isvalid)
            {
                return false;
            }

            return true;
        }

        [HttpGet]
        [Route("api/SendEmail")]
        public bool SendEmail(string name, string email, string subject, string text)
        {

            if ((name == "") || (email == "") || (subject == "") || (text == ""))
            {
                return false;
            }

            bool isvalid = new EmailAddressAttribute().IsValid(email.Trim());

            if (!isvalid)
            {
                return false;
            }

            var body = "<p>From Name: {0}</p><p>From Email: {1}</p><p>Text: {2}</p>";
            MailMessage mailMessage = new MailMessage();
            MailAddress fromAddress = new MailAddress(email);
            mailMessage.From = fromAddress;
            mailMessage.To.Add("wearenbu@gmail.com");
            mailMessage.Body = string.Format(body, name, email, text);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;

            SmtpClient mailSender = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("wearenbu@gmail.com", "wearenbu1")
            };

            mailSender.Send(mailMessage);

            return true;
        }


        [HttpGet]
        [Route("api/About")]
        public AboutVm About()
        {
            var about = this.db.About.FirstOrDefault();

            var updatedFirstImageBy = "";
            if (about.UpdatedFirstImageBy != null)
            {
                var updatedUser = this.db.Users.Find(about.UpdatedFirstImageBy);
                updatedFirstImageBy = $"{updatedUser.FirstName} {updatedUser.LastName}";
            }

            var updatedFirstTextBy = "";
            if (about.UpdatedFirstTextBy != null)
            {
                var updatedUser = this.db.Users.Find(about.UpdatedFirstTextBy);
                updatedFirstTextBy = $"{updatedUser.FirstName} {updatedUser.LastName}";
            }

            var updatedSecondImageBy = "";
            if (about.UpdatedSecondImageBy != null)
            {
                var updatedUser = this.db.Users.Find(about.UpdatedSecondImageBy);
                updatedSecondImageBy = $"{updatedUser.FirstName} {updatedUser.LastName}";
            }

            var updatedSecondTextBy = "";
            if (about.UpdatedSecondTextBy != null)
            {
                var updatedUser = this.db.Users.Find(about.UpdatedSecondTextBy);
                updatedSecondTextBy = $"{updatedUser.FirstName} {updatedUser.LastName}";
            }

            var aboutVm = new AboutVm
            {
                FirstPic = about.FirstImage != null ? AboutFolder + about.FirstImage : "",
                FirstPicUpdatedby = updatedFirstImageBy,
                FirstPicUpdatedAt = about.UpdatedFirstImageAt.HasValue ? about.UpdatedFirstImageAt.Value.ToString("dd.MM.yy HH:mm") : "",
                TextFirstHeader = about.FirstTextHeader,
                TextFirst = about.FirstText,
                UpdatedFirstTextBy = updatedFirstTextBy,
                UpdatedFirstTextAt = about.UpdatedFirstTextAt.HasValue ? about.UpdatedFirstTextAt.Value.ToString("dd.MM.yy HH:mm") : "",
                SecondPic = about.SecondImage != null ? AboutFolder + about.SecondImage : "",
                SecondPicUpdatedby = updatedSecondImageBy,
                SecondPicUpdatedAt = about.UpdatedSecondImageAt.HasValue ? about.UpdatedSecondImageAt.Value.ToString("dd.MM.yy HH:mm") : "",
                TextSecondHeader = about.SecondTextHeader,
                TextSecond = about.SecondText,
                TextSecondUpdatedBy = updatedSecondTextBy,
                TextSecondUpdatedAt = about.UpdatedSecondTextAt.HasValue ? about.UpdatedSecondTextAt.Value.ToString("dd.MM.yy HH:mm") : ""
            };

            return aboutVm;
        }

        [HttpPost]
        [Route("api/EditPictureFirst")]
        public bool EditPictureFirst()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var httpRequest = HttpContext.Current.Request;
                var file = httpRequest.Files["pictureFirst"];
                var format = Path.GetExtension(file.FileName);
                var fileName = Utilities.GenerateRandomString() + format;
                string filePath = Path.Combine(AboutFolderPath, fileName);
                file.SaveAs(filePath);

                var timeT = DateTime.Now.Year + DateTime.Now.Month +
                DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second.ToString();
                var filenameT = timeT + "_T" + Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(AboutFolderPath, filenameT);
                file.SaveAs(fullPath);

                using (Image myImage = Image.FromStream(file.InputStream))
                {
                    Bitmap imageMedium = ResizeImage(myImage, 1200, 800);

                    SaveJpeg(filePath, imageMedium, 90);

                    imageMedium.Dispose();
                }

                Image img = Image.FromFile(fullPath);
                if (img.PropertyIdList.Contains(0x0112))
                {
                    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
                    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                    if (orientation == 6)
                    {
                        Image imgFull = Image.FromFile(filePath);

                        imgFull.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        imgFull.Save(filePath, ImageFormat.Jpeg);
                        imgFull.Dispose();
                    }
                    else if (orientation == 8)
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(filePath, ImageFormat.Jpeg);
                    }
                    else
                    {
                        // Do nothing
                    }
                }

                img.Dispose();

                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }
                var dateTimeNow = DateTime.Now;

                var email = identityClaims.FindFirst("Email").Value;
                var userId = db.Users.FirstOrDefault(f => f.Email == email)?.Id;
                var currentUser = this.db.Users.Find(userId);
                var fullName = $"{currentUser.FirstName} {currentUser.LastName}";
                var about = this.db.About.FirstOrDefault();
                fullPath = AboutFolderPath + about.FirstImage;
                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }
                about.FirstImage = fileName;
                about.UpdatedFirstImageBy = userId;
                about.UpdatedFirstImageAt = dateTimeNow;
                db.SaveChanges();

                return true;
            }

            return false;
        }

        [HttpPost]
        [Route("api/DeletePictureFirst")]
        public bool DeletePictureFirst()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var dateTimeNow = DateTime.Now;
                var about = this.db.About.FirstOrDefault();
                string fullPath = Path.Combine(AboutFolderPath, about.FirstImage);
                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }

                var email = identityClaims.FindFirst("Email").Value;
                var userId = db.Users.FirstOrDefault(f => f.Email == email)?.Id;
                var currentUser = this.db.Users.Find(userId);
                about.FirstImage = null;
                var fullName = $"{currentUser.FirstName} {currentUser.LastName}";
                about.UpdatedFirstImageBy = userId;
                about.UpdatedFirstImageAt = dateTimeNow;
                db.SaveChanges();

                return true;
            }

            return false;
        }


        [HttpPost]
        [Route("api/EditWhoFirst")]
        public bool EditWhoFirst(string textHeader, string textBody)
        {
            var dateTimeNow = DateTime.Now;
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var email = identityClaims.FindFirst("Email").Value;
                var userId = db.Users.FirstOrDefault(f => f.Email == email)?.Id;
                var currentUser = this.db.Users.Find(userId);

                var about = this.db.About.FirstOrDefault();
                about.UpdatedFirstTextBy = currentUser.Id;
                about.UpdatedFirstTextAt = dateTimeNow;
                about.FirstTextHeader = textHeader;
                about.FirstText = textBody;
                this.db.SaveChanges();

                return true;
            }

            return false;
        }

        [HttpPost]
        [Route("api/EditPictureSecond")]
        public bool EditPictureSecond()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var httpRequest = HttpContext.Current.Request;
                var file = httpRequest.Files["pictureSecond"];
                var format = Path.GetExtension(file.FileName);
                var fileName = Utilities.GenerateRandomString() + format;
                string filePath = Path.Combine(AboutFolderPath, fileName);
                file.SaveAs(filePath);

                var timeT = DateTime.Now.Year + DateTime.Now.Month +
                DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second.ToString();
                var filenameT = timeT + "_T" + Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(AboutFolderPath, filenameT);
                file.SaveAs(fullPath);

                using (Image myImage = Image.FromStream(file.InputStream))
                {
                    Bitmap imageMedium = ResizeImage(myImage, 1200, 800);

                    SaveJpeg(filePath, imageMedium, 90);

                    imageMedium.Dispose();
                }

                Image img = Image.FromFile(fullPath);
                if (img.PropertyIdList.Contains(0x0112))
                {
                    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
                    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                    if (orientation == 6)
                    {
                        Image imgFull = Image.FromFile(filePath);

                        imgFull.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        imgFull.Save(filePath, ImageFormat.Jpeg);
                        imgFull.Dispose();
                    }
                    else if (orientation == 8)
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(filePath, ImageFormat.Jpeg);
                    }
                    else
                    {
                        // Do nothing
                    }
                }

                img.Dispose();

                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }
                var dateTimeNow = DateTime.Now;

                var email = identityClaims.FindFirst("Email").Value;
                var userId = db.Users.FirstOrDefault(f => f.Email == email)?.Id;
                var currentUser = this.db.Users.Find(userId);
                var fullName = $"{currentUser.FirstName} {currentUser.LastName}";
                var about = this.db.About.FirstOrDefault();
                fullPath = AboutFolderPath + about.SecondImage;
                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }
                about.SecondImage = fileName;
                about.UpdatedSecondImageBy = userId;
                about.UpdatedSecondImageAt = dateTimeNow;
                db.SaveChanges();

                return true;
            }

            return false;
        }

        [HttpPost]
        [Route("api/DeletePictureSecond")]
        public bool DeletePictureSecond()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var dateTimeNow = DateTime.Now;
                var about = this.db.About.FirstOrDefault();
                string fullPath = Path.Combine(AboutFolderPath, about.SecondImage);
                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }

                var email = identityClaims.FindFirst("Email").Value;
                var userId = db.Users.FirstOrDefault(f => f.Email == email)?.Id;
                var currentUser = this.db.Users.Find(userId);
                about.SecondImage = null;
                var fullName = $"{currentUser.FirstName} {currentUser.LastName}";
                about.UpdatedSecondImageBy = userId;
                about.UpdatedSecondImageAt = dateTimeNow;
                db.SaveChanges();

                return true;
            }

            return false;
        }


        [HttpPost]
        [Route("api/EditWhoSecond")]
        public bool EditWhoSecond(string textHeader, string textBody)
        {
            var dateTimeNow = DateTime.Now;
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var email = identityClaims.FindFirst("Email").Value;
                var userId = db.Users.FirstOrDefault(f => f.Email == email)?.Id;
                var currentUser = this.db.Users.Find(userId);

                var about = this.db.About.FirstOrDefault();
                about.UpdatedSecondTextBy = currentUser.Id;
                about.UpdatedSecondTextAt = dateTimeNow;
                about.SecondTextHeader = textHeader;
                about.SecondText = textBody;
                this.db.SaveChanges();

                return true;
            }

            return false;
        }

        [HttpPost]
        [Route("api/EditHomePagePicture")]
        public bool EditHomePagePicture()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;

            if (claims.Count() > 0)
            {
                var httpRequest = HttpContext.Current.Request;
                var file = httpRequest.Files["homeChangeImg"];

                var format = Path.GetExtension(file.FileName);
                var fileName = Utilities.GenerateRandomString() + format;
                string filePath = Path.Combine(HomeFolderPath, fileName);
                file.SaveAs(filePath);

                var timeT = DateTime.Now.Year + DateTime.Now.Month +
                DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second.ToString();
                var filenameT = timeT + "_T" + Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(HomeFolderPath, filenameT);
                file.SaveAs(fullPath);

                using (Image myImage = Image.FromStream(file.InputStream))
                {
                    Bitmap imageMedium = ResizeImage(myImage, 1600, 800);

                    SaveJpeg(filePath, imageMedium, 90);

                    imageMedium.Dispose();
                }

                Image img = Image.FromFile(fullPath);
                if (img.PropertyIdList.Contains(0x0112))
                {
                    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
                    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                    if (orientation == 6)
                    {
                        Image imgFull = Image.FromFile(filePath);

                        imgFull.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        imgFull.Save(filePath, ImageFormat.Jpeg);
                        imgFull.Dispose();
                    }
                    else if (orientation == 8)
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(filePath, ImageFormat.Jpeg);
                    }
                    else
                    {
                        // Do nothing
                    }
                }

                img.Dispose();

                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }
                var homePage = this.db.HomePage.FirstOrDefault();
                fullPath = Path.Combine(HomeFolderPath, homePage.ImageUrl);
                if (System.IO.File.Exists(fullPath))
                {

                    System.IO.File.Delete(fullPath);
                }
                homePage.ImageUrl = fileName;
                db.SaveChanges();

                return true;
            }

            return false;
        }

        [HttpPost]
        [Route("api/DeleteHomePageImg")]
        public bool DeleteHomePageImg()
        {
            var homePageDb = this.db.HomePage.FirstOrDefault();
            homePageDb.ImageUrl = "";
            this.db.SaveChanges();

            return true;
        }

        [HttpPost]
        [Route("api/EditHomeTextHeader")]
        public bool EditHomeTextHeader(string textHeader)
        {
            var homePage = this.db.HomePage.FirstOrDefault();
            homePage.TextHeader = textHeader;
            this.db.SaveChanges();
            return true;
        }


        [HttpPost]
        [Route("api/EditHomeText")]
        public bool EditHomeText(string text)
        {
            var homePage = this.db.HomePage.FirstOrDefault();
            homePage.Text = text;
            this.db.SaveChanges();
            return true;
        }

        public static Bitmap ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioWidth = (double)maxWidth / (double)image.Width;
            double ratioHeight = (double)maxHeight / (double)image.Height;

            double ratio = ratioWidth < ratioHeight ? ratioWidth : ratioHeight;

            int newWidth = Convert.ToInt32(image.Width * ratio);
            int newHeight = Convert.ToInt32(image.Height * ratio);

            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var destImage = new Bitmap(newWidth, newHeight);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);


            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public void SaveJpeg(string path, Bitmap img, int quality)
        {
            if (quality < 0 || quality > 100)
            {
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");
            }

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }

        //public ActionResult Index()
        //{
        //    ViewBag.Title = "Home Page";

        //    return View();
        //}
    }
}
