using System;

namespace Restaurant.Messages.Interfaces
{
    public interface IKitchenReady
    {
        public Guid OrderId { get; }
        public bool Ready { get; }
    }
}
