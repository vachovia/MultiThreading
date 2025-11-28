namespace BasicSyntax
{
    public class ManualResetEventExample
    {
        string? userInput = null;
        ManualResetEventSlim _manualResetEvent;

        public ManualResetEventExample(ManualResetEventSlim manualResetEvent)
        {
            _manualResetEvent = manualResetEvent;
        }

        public void ManualResetEventExec()
        {

            Console.WriteLine("Press enter to release all threads...");
            
            for (int i = 0; i < 3; i++)
            {
                Thread workerThread = new Thread(Worker);
                workerThread.Name = $"Worker {i + 1}";
                workerThread.Start();
            }

            Console.ReadLine();

            _manualResetEvent.Set(); // Release all waiting threads

            Console.ReadLine();
        }

        void Worker()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for the signal...");

            _manualResetEvent.Wait();

            Thread.Sleep(1000);

            Console.WriteLine($"{Thread.CurrentThread.Name} has been released.");
        }
    }
}
