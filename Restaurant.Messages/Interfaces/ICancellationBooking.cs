using System;

namespace Restaurant.Messages.Interfaces
{
    public interface ICancellationBooking
    {
        public Guid OrderId { get; }
        public Dish? Dish { get; }
    }
}
