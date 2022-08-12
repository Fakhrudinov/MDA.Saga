using System;

namespace Restaurant.Messages.Interfaces
{
    public interface ITableBooked
    {
        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public bool Success { get; set; }
    }
}
