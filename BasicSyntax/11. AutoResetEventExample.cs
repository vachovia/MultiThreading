namespace BasicSyntax
{
    public class AutoResetEventExample
    {
        string? userInput = null;
        AutoResetEvent _autoResetEvent;

        public AutoResetEventExample(AutoResetEvent autoResetEvent)
        {
            _autoResetEvent = autoResetEvent;
        }

        public void AutoResetEventSingleExec()
        {
            ///////////////////////////////////////////////
            // Single Worker threads
            //////////////////////////////////////////////

            // Start the worker thread
            Thread workerThread = new Thread(WorkerSingle);
            workerThread.Start();

            // Main thread receives user input and signals the worker thread
            Console.WriteLine("Server is running. Type 'exit' to stop.");

            while (true)
            {
                userInput = Console.ReadLine() ?? "";

                // Signal the worker thread if input is "go"
                if (userInput.ToLower() == "go")
                {
                    _autoResetEvent.Set(); // Signal the worker thread to proceed
                }
            }
        }

        void WorkerSingle()
        {
            while (true)
            {
                Console.WriteLine("Worker thread is waiting for signal.");
                // Wait for the signal from the main thread
                _autoResetEvent.WaitOne(); // Blocks here until signaled - turned off automatically after releasing one thread

                Console.WriteLine("Worker thread proceeds.");
                // Simulate processing time
                Thread.Sleep(2000);
            }
        }

        public void AutoResetEventMultiExec()
        {
            ///////////////////////////////////////////////////////////////////////
            // Single Worker thread
            ///////////////////////////////////////////////////////////////////////            

            Console.WriteLine("Server is running. Type 'go' to proceed and  'exit' to stop.");

            // Start the worker thread
            for (int i = 0; i < 3; i++)
            {
                Thread workerThread = new Thread(WorkerMulti);
                workerThread.Name = $"Worker {i + 1}";
                workerThread.Start();
            }

            // Main thread receives user input

            while (true)
            {
                userInput = Console.ReadLine() ?? "";

                // Signal the worker thread if input is "go"
                if (userInput.ToLower() == "go")
                {
                    _autoResetEvent.Set(); // Signal the worker thread to proceed
                }
            }
        }

        void WorkerMulti()
        {
            while (true)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for signal.");
                // Wait for the signal from the main thread
                _autoResetEvent.WaitOne(); // Blocks here until signaled - turned off automatically after releasing one thread

                Console.WriteLine($"{Thread.CurrentThread.Name} proceeds.");
                // Simulate processing time
                Thread.Sleep(2000);
            }
        }
    }
}
