using System;

namespace Restaurant.Messages.Interfaces
{
    public interface IKitchenAccident
    {
        public Guid OrderId { get; }
        public Dish Dish { get; }
    }
}
