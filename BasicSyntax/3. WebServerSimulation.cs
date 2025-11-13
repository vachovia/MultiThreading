namespace BasicSyntax
{
    internal class WebServerSimulation
    {
        static volatile bool isRunning = true;
        static Queue<string?> requestQueue = new Queue<string?>();

        public static void WebServerSimulationExample()
        {
            // Start a thread to monitor the request queue
            // it does not block the main thread from accepting new inputs.
            Thread monitoringThread = new Thread(MonitorQueue);
            monitoringThread.Start();
            
            Console.WriteLine("Server is running. Type 'exit' to stop.");

            // Simulate incoming requests
            while (isRunning)
            {
                // main thread reads user input
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
            // Continuously monitor the queue for new requests
            while (isRunning)
            {
                if (requestQueue.Count > 0)
                {
                    string? input = requestQueue.Dequeue();

                    // Process each request in a separate thread to simulate concurrent handling
                    // otherwise, processing one request would block the monitoring of new requests.
                    Thread processingThread = new Thread(() => ProcessInput(input));

                    processingThread.Start();
                }
                // Small delay to prevent busy-waiting
                Thread.Sleep(100);
            }
        }

        static void ProcessInput(string? input)
        {
            Thread.Sleep(2000);

            Console.WriteLine($"Processed input: {input}");
        }
    }
}
