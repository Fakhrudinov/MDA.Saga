using Restaurant.Messages.Interfaces;
using System;

namespace Restaurant.Messages
{
    public class CancellationBooking : ICancellationBooking
    {
        public Guid OrderId { get; }
        public Dish? Dish { get; }

        public CancellationBooking(Guid orderId, Dish? dish)
        {
            OrderId = orderId;
            Dish = dish;
        }
    }
}
