using Restaurant.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Restaurant.Notification
{
    public class Notifier
    {
        //импровизированный кэш для хранения статусов, номера заказа и клиента
        private readonly ConcurrentDictionary<Guid, Tuple<Guid?, EnumAccepted>> _state = new();

        public void Accept(Guid orderId, EnumAccepted accepted, Guid? clientId = null)
        {
            _state.AddOrUpdate(orderId, new Tuple<Guid?, EnumAccepted>(clientId, accepted),
                (guid, oldValue) => new Tuple<Guid?, EnumAccepted>
                    (oldValue.Item1 ?? clientId, oldValue.Item2 | accepted));

            Notify(orderId);
        }

        private void Notify(Guid orderId)
        {
            var booking = _state[orderId];
            
            switch (booking.Item2)
            {
                case EnumAccepted.All:
                    Console.WriteLine($"Успешно забронировано с номером заказа #{booking.Item1}");
                    _state.Remove(orderId, out _);
                    break;
                case EnumAccepted.Rejected:
                    Console.WriteLine($"Заказ #{booking.Item1} - невозможен, к сожалению, все столики заняты");
                    _state.Remove(orderId, out _);
                    break;
                case EnumAccepted.Kitchen:
                case EnumAccepted.Booking:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Notify(Guid orderId, Dish? dish)
        {
            Console.WriteLine($"Отмена бронирования стола по заказу {orderId} в связи с отсутсвием блюда {dish.Name}!");
        }
    }
}