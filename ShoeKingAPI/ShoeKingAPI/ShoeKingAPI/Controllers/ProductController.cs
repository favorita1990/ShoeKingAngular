using ShoeKingAPI.Models.ViewModels;
using ShoeKingAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ShoeKingAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ProductRepository productRepository = new ProductRepository();

        [HttpGet]
        [Route("api/GetProductById")]
        public ProductViewModel GetProductById(int? id)
        {
            if (id != null)
            {
                var productId = Convert.ToInt32(id);
                var product = productRepository.find(productId);

                if (product != null)
                {
                    var productViewModel = productRepository.GetProductById(product.Id);

                    return productViewModel;
                }

                return null;
            }

            return null;
        }

        [HttpPost]
        [Route("api/AddRating")]
        public RatingVm AddRating(int rateNumber, int productId)
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var email = identityClaims.FindFirst("Email").Value;
                var rating = productRepository.AddRatingToProduct(productId, email, rateNumber);
                return rating;
            }

            return null;
        }

        [HttpGet]
        [Route("api/GetRatings")]
        public RatingVm GetRatings(int productId)
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var email = identityClaims.FindFirst("Email").Value;
                var ratingTemp = productRepository.GetRatings(productId, email);
                return ratingTemp;
            }

            var rating = productRepository.GetRatings(productId, null);

            return rating;
        }


        [HttpPost]
        [Route("api/AddComment")]
        public List<CommentVm> AddComment(string text, int productId)
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var email = identityClaims.FindFirst("Email").Value;
                var comments = productRepository.AddCommentToProduct(productId, email, text);
                return comments;
            }

            return null;
        }

        [HttpGet]
        [Route("api/GetComments")]
        public List<CommentVm> GetComments(int productId)
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var email = identityClaims.FindFirst("Email").Value;
                var commentsTemp = productRepository.GetCommentsByProductId(productId, email);
                return commentsTemp;
            }

            var comments = productRepository.GetCommentsByProductId(productId, null);

            return comments;
        }

        [HttpPost]
        [Route("api/RemoveComment")]
        public List<CommentVm> RemoveComment(int commentId, int productId)
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            if (claims.Count() > 0)
            {
                var email = identityClaims.FindFirst("Email").Value;
                var comments = productRepository.RemoveCommentToProduct(productId, email, commentId);
                return comments;
            }

            return null;
        }
    }
}