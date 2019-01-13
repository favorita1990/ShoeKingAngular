using ShoeKingAPI.Context;
using ShoeKingAPI.Models;
using ShoeKingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoeKingAPI.Repository
{
    public class OrderDetailRepository
    {
        private ContextDB db = new ContextDB();

        public void create(OrderDetail orderDetail)
        {
            db.OrderDetails.Add(orderDetail);
            db.SaveChanges();
        }

        public OrderDetailViewModel Map(OrderDetail orderDetail)
        {

            OrderDetailViewModel newOrderDetail = new OrderDetailViewModel()
            {
                OrderDetailsId = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                Price = orderDetail.Price,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                Specials = orderDetail.Specials,
                Status = orderDetail.Status
            };

            return newOrderDetail;
        }

        public List<OrderDetailViewModel> GetAllOrderDetailsGrid()
        {
            List<OrderDetailViewModel> allOrders = new List<OrderDetailViewModel>();

            foreach (var orderDetail in db.OrderDetails.ToList())
            {
                allOrders.Add(Map(orderDetail));
            }

            return allOrders;
        }

        public List<OrderDetailViewModel> GetOrderDetailsGridByOrderId(int orderId)
        {
            List<OrderDetailViewModel> allOrders = new List<OrderDetailViewModel>();

            foreach (var orderDetail in db.OrderDetails.Where(w => w.OrderId == orderId).ToList())
            {
                allOrders.Add(Map(orderDetail));
            }

            return allOrders;
        }
    }
}