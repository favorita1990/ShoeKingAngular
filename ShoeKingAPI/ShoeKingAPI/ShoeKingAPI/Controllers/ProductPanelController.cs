using ShoeKingAPI.Context;
using ShoeKingAPI.Models;
using ShoeKingAPI.Models.ViewModels;
using ShoeKingAPI.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ShoeKingAPI.Controllers
{
    public class ProductPanelController : ApiController
    {
        private readonly ContextDB db = new ContextDB();
        private readonly ProductRepository productRepository = new ProductRepository();
        private readonly CategoryRepository categoryRepository = new CategoryRepository();
        
        private readonly string productsFolder = "../../src/assets/images/products/";
        private readonly string productsFolderPath = "F:/favorita/Angular/onlineshoeking/ShoeKingAngular/src/assets/images/products/";


        [HttpPost]
        [Route("api/DeleteProductById")]
        public bool DeleteProductById(string productId)
        {
            productRepository.DeleteProductById(int.Parse(productId));

            return true;
        }


        [HttpPost]
        [Route("api/DeleteCategoryById")]
        public bool DeleteCategoryById(string categoryId)
        {
            categoryRepository.DeleteCategoryById(int.Parse(categoryId));

            return true;
        }

        [HttpGet]
        [Route("api/GetAllCategories")]
        public List<CategoryViewModel> GetAllCategories()
        {
            var allCategories = categoryRepository.GetAllCategories();

            return allCategories;
        }

        [HttpGet]
        [Route("api/GetAllProductsGrid")]
        public List<ProductViewModel> GetAllProductsGrid()
        {
            var allProducts = productRepository.GetAllProductsGrid();

            return allProducts;
        }

        [HttpPost]
        [Route("api/CreateEditProduct")]
        public bool CreateEditProduct()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var httpRequest = HttpContext.Current.Request;

                string productId = httpRequest.Form["inputProductId"];

                string productName = httpRequest.Form["inputName"];
                string productBgName = httpRequest.Form["inputBgName"];
                HttpPostedFile productMainImg = httpRequest.Files["productMainImg"];
                var productBodyImg = httpRequest.Files["productBodyImg0"];
                List<HttpPostedFile> productBodyImgs = new List<HttpPostedFile>();
                for (int i = 0; i < int.Parse(productBodyImg.FileName); i++)
                {
                    var productBodyImgTemp = httpRequest.Files["productBodyImg" + i];
                    productBodyImgs.Add(productBodyImgTemp);
                }

                string productPriceReplaced = httpRequest.Form["inputPrice"];
                productPriceReplaced = productPriceReplaced.Replace(".", ",");
                decimal productPrice = decimal.Parse(productPriceReplaced);
                int productCategory = int.Parse(httpRequest.Form["inputCategories"]);
                string productCategoryText = productCategory != 0 ? "" : httpRequest.Form["inputCategoryText"];
                string productCategoryBgText = productCategory != 0 ? "" : httpRequest.Form["inputCategoryBgText"];
                HttpPostedFile productCategoryImg = httpRequest.Files["productCategoryImg"];
                bool productSpecials = int.Parse(httpRequest.Form["inputSpecials"]) == 1 ? true : false;
                string productPromotionPercentReplaced = httpRequest.Form["inputPromotionPercent"];
                bool productStatus = int.Parse(httpRequest.Form["inputStatus"]) == 1 ? true : false;
                bool categoryGender = productCategory != 0 ? false : int.Parse(httpRequest.Form["inputCategoryGender"]) == 1 ? true : false;
                bool categoryStatus = productCategory != 0 ? false : int.Parse(httpRequest.Form["inputCategoryStatus"].ToString()) == 1 ? true : false;
                productPromotionPercentReplaced = productPromotionPercentReplaced.Replace(".", ",");
                decimal productPromotionPercent = productSpecials == false ? 0 : decimal.Parse(productPromotionPercentReplaced);
                var productSize = httpRequest.Form["inputListSize"].Split(',');
                var productQuantity = httpRequest.Form["inputListQuantity"].Split(',');

                List<double> productListSize = new List<double>();
                List<int> productListQuantity = new List<int>();
                foreach (var sizeItem in productSize)
                {
                    string sizeReplaced = sizeItem;
                    sizeReplaced = sizeReplaced.Replace(".", ",");
                    double tempSize = double.Parse(sizeReplaced);
                    if (tempSize < 10 || tempSize > 55)
                    {
                        return false;
                    }
                    productListSize.Add(tempSize);
                }

                foreach (var quantityItem in productQuantity)
                {
                    int quantityParse = int.Parse(quantityItem);
                    productListQuantity.Add(quantityParse);
                }

                string productCategoryImgName = null;

                if (productPromotionPercent < 0 || productPromotionPercent > 100)
                {
                    return false;
                }

                string productDescription = httpRequest.Form["inputDescription"];
                string productBgDescription = httpRequest.Form["inputBgDescription"];

                if (string.IsNullOrEmpty(productName))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(productBgName))
                {
                    return false;
                }

                if (productCategory == 0)
                {
                    if (string.IsNullOrEmpty(productCategoryText))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(productCategoryBgText))
                    {
                        return false;
                    }

                    //string time = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    //  DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    //productCategoryImgName = time + "_" + Path.GetFileName(productCategoryImg.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath(directoryOfProductMainBody), productCategoryImgName);
                    //productCategoryImg.SaveAs(physicalPath);

                    string time = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    productCategoryImgName = time + "_" + Path.GetFileName(productCategoryImg.FileName);
                    string filePath = Path.Combine(productsFolderPath, productCategoryImgName);
                    productCategoryImg.SaveAs(filePath);

                    var timeT = DateTime.Now.Year + DateTime.Now.Month +
                    DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second.ToString();
                    var filenameT = timeT + "_T" + Path.GetFileName(productCategoryImg.FileName);
                    var fullPath = Path.Combine(productsFolderPath, filenameT);
                    productCategoryImg.SaveAs(fullPath);

                    using (Image myImage = Image.FromStream(productCategoryImg.InputStream))
                    {
                        Bitmap imageMedium = ResizeImage(myImage, 832, 350);

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

                }
                if (string.IsNullOrEmpty(productDescription))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(productBgDescription))
                {
                    return false;
                }


                string productMainImgName = null;

                if (productMainImg != null)
                {
                    //string productImgTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    //DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    //productMainImgName = productImgTime + "_" + Path.GetFileName(productMainImg.FileName);
                    //var physicalPathMainProduct = Path.Combine(Server.MapPath(directoryOfProductMainBody), productMainImgName);
                    //productMainImg.SaveAs(physicalPathMainProduct);

                    string productImgTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                    DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    productMainImgName = productImgTime + "_" + Path.GetFileName(productMainImg.FileName);
                    string filePath = Path.Combine(productsFolderPath, productMainImgName);
                    productMainImg.SaveAs(filePath);

                    var timeT = DateTime.Now.Year + DateTime.Now.Month +
                    DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second.ToString();
                    var filenameT = timeT + "_T" + Path.GetFileName(productMainImg.FileName);
                    var fullPath = Path.Combine(productsFolderPath, filenameT);
                    productMainImg.SaveAs(fullPath);

                    using (Image myImage = Image.FromStream(productMainImg.InputStream))
                    {
                        Bitmap imageMedium = ResizeImage(myImage, 832, 350);

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

                    if (productId == null)
                    {
                        productId = "0";
                    }
                    int? productIdInt = int.Parse(productId);
                    var productToEdit = db.Products.FirstOrDefault(w => w.Id == productIdInt);

                    if (productToEdit != null)
                    {
                        fullPath = productsFolderPath + productToEdit.PhotoHeader;
                        if (System.IO.File.Exists(fullPath))
                        {

                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                List<string> listProductBodyImg = new List<string>();

                if (productBodyImg != null)
                {
                    foreach (var bodyImg in productBodyImgs)
                    {
                        string productBodyImgTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                        DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                        string productBodyImgName = productBodyImgTime + "_" + Path.GetFileName(bodyImg.FileName);
                        string filePath = Path.Combine(productsFolderPath, productBodyImgName);
                        bodyImg.SaveAs(filePath);

                        var timeT = DateTime.Now.Year + DateTime.Now.Month +
                        DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second.ToString();
                        var filenameT = timeT + "_T" + Path.GetFileName(bodyImg.FileName);
                        var fullPath = Path.Combine(productsFolderPath, filenameT);
                        bodyImg.SaveAs(fullPath);

                        using (Image myImage = Image.FromStream(bodyImg.InputStream))
                        {
                            Bitmap imageMedium = ResizeImage(myImage, 832, 350);

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

                        listProductBodyImg.Add(productBodyImgName);
                    }
                }

                var email = identityClaims.FindFirst("Email").Value;

                string success = productRepository.CreateNewProduct(productId, productName, productBgName, productMainImgName, listProductBodyImg, productPrice, productCategory,
                productCategoryText, productCategoryBgText, productCategoryImgName, productSpecials, productPromotionPercent, productStatus, categoryGender, categoryStatus,
                productListSize, productListQuantity, productDescription, productBgDescription, email);

                return true;
            }

            return false;
        }


        [HttpPost]
        [Route("api/CreateEditCategory")]
        public bool CreateEditCategory()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {

                var httpRequest = HttpContext.Current.Request;

                string categoryId = httpRequest.Form["inputCategoryId"];

                string categoryText = httpRequest.Form["inputCategoryText"];
                string categoryBgText = httpRequest.Form["inputCategoryBgText"];
                HttpPostedFile categoryImg = httpRequest.Files["productCategoryImg"];
                bool categoryStatus = int.Parse(httpRequest.Form["inputCategoryStatus"]) == 1 ? true : false;
                bool categoryGender = int.Parse(httpRequest.Form["inputCategoryGender"]) == 1 ? true : false;

                if (string.IsNullOrEmpty(categoryText))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(categoryBgText))
                {
                    return false;
                }

                string categoryImgName = null;

                if (categoryImg != null)
                {
                    string time = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                                    DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

                    categoryImgName = time + "_" + Path.GetFileName(categoryImg.FileName);
                    string filePath = Path.Combine(productsFolderPath, categoryImgName);
                    categoryImg.SaveAs(filePath);

                    var timeT = DateTime.Now.Year + DateTime.Now.Month +
                    DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second.ToString();
                    var filenameT = timeT + "_T" + Path.GetFileName(categoryImg.FileName);
                    var fullPath = Path.Combine(productsFolderPath, filenameT);
                    categoryImg.SaveAs(fullPath);

                    using (Image myImage = Image.FromStream(categoryImg.InputStream))
                    {
                        Bitmap imageMedium = ResizeImage(myImage, 832, 350);

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
                }

                var email = identityClaims.FindFirst("Email").Value;

                string success = categoryRepository.CreateNewCategory(categoryId, categoryText, categoryBgText, categoryImgName, categoryStatus, categoryGender, email);

                return true;
            }

            return false;
        }

        [HttpGet]
        [Route("api/AllProductCategories")]
        public List<CategoryViewModel> AllProductCategories()
        {
            var allCategories = new List<CategoryViewModel>();

            allCategories = categoryRepository.GetDropdownCategoriesGrid();

            return allCategories;
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
    }
}