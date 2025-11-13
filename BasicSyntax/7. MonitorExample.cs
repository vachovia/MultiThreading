namespace BasicSyntax
{
    internal class MonitorExample
    {
        #region Counter Example

        static int counter = 0;
        static object counterLock = new object();

        static void IncrementCounter()
        {
            for (int i = 0; i < 100000; i++)
            {
                Monitor.Enter(counterLock);

                try
                {
                    counter = counter + 1;
                }
                finally
                {
                    Monitor.Exit(counterLock);
                }
            }
        }

        static public void Counter()
        {
            Thread thread1 = new Thread(IncrementCounter);
            Thread thread2 = new Thread(IncrementCounter);

            thread1.Start(); // thread1.Join(); - in this case we wait for thread1 to finish before starting thread2 and result will be 200000
            thread2.Start(); // thread2.Join();
            // no blocking call to wait for threads to finish - result is not 200000 - it is race condition
            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Final counter value is: {counter}");
        }

        #endregion

        #region Tickets Booking Example

        static int availableTickets = 10;
        static volatile bool isRunning = true;
        static object ticketsLock = new object();
        static Queue<string?> requestQueue = new Queue<string?>();

        public static void TicketsBooking()
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
            while (isRunning)
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
            if (Monitor.TryEnter(ticketsLock, 2000))
            {
                try
                {
                    // Simulate processing time
                    Thread.Sleep(3000);

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
                finally
                {
                    Monitor.Exit(ticketsLock);
                }
            }
        }

        #endregion
    }
}
