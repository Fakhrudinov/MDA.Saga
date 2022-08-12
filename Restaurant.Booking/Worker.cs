using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Restaurant.Messages;

namespace Restaurant.Booking
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaurant;
        private readonly Random _random = new Random();

        public Worker(IBus bus, Restaurant restaurant)
        {
            _bus = bus;
            _restaurant = restaurant;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\r\n\tВыберите действие:\r\n" +
                    "\t\t0 Распечатать список столов и их состояние\r\n" +
                    "\t\t1 Забронировать свободный столик асинхронно с нормальным блюдом\r\n" +
                    "\t\t2 Забронировать свободный столик aсинхронно с отказным блюдом\r\n" +
                    "\t\t3 Снять бронь асинхронно\r\n"
                    );

                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int choise) && (choise < 0 || choise > 4))
                {
                    Console.WriteLine("\tВнимание, некорректный ввод! допускается только целые числа  0  1  2  3");
                    continue;
                }

                var orderId = NewId.NextGuid();
                var clientId = NewId.NextGuid();

                if (choise == 0)
                {
                    _restaurant.PrintTablesStatus();
                    continue;
                }
                else if (choise == 1)
                {                 
                    var result = await _restaurant.BookFreeTableAsync(1, orderId, stoppingToken);

                    await _bus.Publish(new TableBooked(orderId, result ?? false, clientId, new Dish { Id = _random.Next(1, 2) }),
                                        context => context.Durable = false, stoppingToken);
                }
                else if (choise == 2)
                {

                    var result = await _restaurant.BookFreeTableAsync(1, orderId, stoppingToken);

                    await _bus.Publish(new TableBooked(orderId, result ?? false, clientId, new Dish { Id = 3 }),
                                        context => context.Durable = false, stoppingToken);
                }
                else
                {
                    bool tableNumberNotInputed = true;
                    while (tableNumberNotInputed)
                    {
                        Console.WriteLine("\tВыберите номер стола, которому отменяем бронь");

                        string userInputTableNumber = Console.ReadLine();

                        if (int.TryParse(userInputTableNumber, out int tableNumber))
                        {
                            if (tableNumber < 1 || // минимум
                                tableNumber > 10) //максимум
                            {
                                Console.WriteLine($"Tакого столика не существует. Вводите целое число, с 1 по {10} включительно");
                                continue;
                            }

                            tableNumberNotInputed = false; // ввод номера стола завершен

                            await _restaurant.DeleteBookingForTableAsync(tableNumber);
                        }
                        else
                        {
                            Console.WriteLine("Допустимы только целые числа!");
                        }
                    }
                }
            }
        }
    }
}