using Restaurant.Messages.Interfaces;
using System;

namespace Restaurant.Messages
{
    public class TableBooked : ITableBooked
    {
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public bool Success { get; set; }

        public TableBooked(Guid orderId, bool success, Guid clientId, Dish? preOrder = null)
        {
            OrderId = orderId;
            ClientId = clientId;
            Success = success;
            PreOrder = preOrder;
        }
    }
}