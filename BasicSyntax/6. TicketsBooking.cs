namespace BasicSyntax
{
    internal class TicketsBooking
    {
        static int availableTickets = 10;
        static volatile bool isRunning = true;
        static object ticketsLock = new object();
        static Queue<string?> requestQueue = new Queue<string?>();

        public static void TicketsBookingExample()
        {
            Thread monitoringThread = new Thread(MonitorQueue);
            monitoringThread.Start();

            // 1. Enqueue the requests
            Console.WriteLine("Server is running. \r\n Type 'b' to book a ticket. \r\n Type 'c' to cancel. \r\n Type 'exit' to stop. \r\n");

            while (isRunning)
            {
                string? input = Console.ReadLine();
                if (input?.ToLower() == "exit")
                {
                    isRunning = false;
                    break;
                }

                requestQueue.Enqueue(input);
            }

            Console.ReadLine();
        }

        static void MonitorQueue()
        {
            while (true)
            {
                if (requestQueue.Count > 0)
                {
                    string? input = requestQueue.Dequeue();
                    Thread processingThread = new Thread(() => ProcessBooking(input));
                    processingThread.Start();
                }
                Thread.Sleep(100);
            }
        }

        // 3. Processing the requests
        static void ProcessBooking(string? input)
        {
            // Simulate processing time
            Thread.Sleep(2000);

            lock (ticketsLock)
            {
                if (input == "b")
                {
                    if (availableTickets > 0)
                    {
                        availableTickets--;
                        Console.WriteLine();
                        Console.WriteLine($"Your seat is booked. {availableTickets} seats are still available.");
                    }
                    else
                    {
                        Console.WriteLine($"Tickets are not available.");
                    }
                }
                else if (input == "c")
                {
                    if (availableTickets < 10)
                    {
                        availableTickets++;
                        Console.WriteLine();
                        Console.WriteLine($"Your booking is canceled. {availableTickets} seats are available.");
                    }
                    else
                    {
                        Console.WriteLine($"Error. You cannot cancel a booking at this time.");
                    }
                }
            }
        }
    }
}
