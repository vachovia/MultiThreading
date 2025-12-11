namespace BasicSyntax
{
    public class SemaphoreExample
    {
        public volatile bool isRunning = true;
        public object queueLock = new object();
        public Queue<string?> requestQueue = new Queue<string?>();
        private SemaphoreSlim _semaphore;
        // Semaphore to limit concurrent processing to 3 threads
        // Connection pool size simulation - max 3 concurrent connections

        public SemaphoreExample(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public void Exec()
        {
            // 2. Start the requests queue monitoring thread
            Thread monitoringThread = new Thread(MonitorQueue);
            monitoringThread.Start();

            // 1. Enqueue the requests
            Console.WriteLine("Server is running. Type 'exit' to stop.");

            // Simulate incoming requests
            while (isRunning)
            {
                string? input = Console.ReadLine();

                if (input?.ToLower() == "exit")
                {
                    isRunning = false;
                    break;
                }

                lock (queueLock)
                {
                    requestQueue.Enqueue(input);
                }
            }

            Console.ReadLine();
        }

        void MonitorQueue()
        {
            // Continuously monitor the queue for new requests
            while (isRunning)
            {
                if (requestQueue.Count > 0)
                {
                    string? input;

                    lock (queueLock)
                    {
                        input = requestQueue.Dequeue();
                    }

                    _semaphore.Wait();

                    Thread processingThread = new Thread(() => ProcessInput(input));
                    processingThread.Start();
                }

                Thread.Sleep(100);
            }
        }

        void ProcessInput(string? input)
        {
            try
            {
                // Simulate processing time
                Thread.Sleep(2000);
                Console.WriteLine($"Processed input: {input}");
            }
            finally
            {
                var previousCount = _semaphore.Release();
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} released the semaphore. Previous count is: {previousCount}");
            }
        }
    }
}
