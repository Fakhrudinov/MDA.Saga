using System.Threading.Tasks;
using MassTransit;
using Restaurant.Messages;
using Restaurant.Messages.Interfaces;

namespace Restaurant.Kitchen.Consumers
{
    internal class KitchenTableBookedConsumer : IConsumer<ITableBooked>
    {
        private readonly Manager _manager;

        public KitchenTableBookedConsumer(Manager manager)
        {
            _manager = manager;
        }

        public Task Consume(ConsumeContext<ITableBooked> context)
        {
           var result = context.Message.Success;

           if (result)
            {
                var (confirmation, dish) = _manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder);
                if (confirmation)
                {
                    context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
                }
                else
                {
                    context.Publish<IKitchenAccident>(new KitchenAccident(context.Message.OrderId, dish!));
                }
            }

            return context.ConsumeCompleted;
        }
    }
}