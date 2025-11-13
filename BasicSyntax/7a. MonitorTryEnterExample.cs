using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSyntax
{
    internal class MonitorTryEnterExample
    {
        private static int _counter = 0;
        private static readonly object _lockObject = new object();

        public void AccessResourceWithTryEnter(int threadId)
        {
            bool acquiredLock = false;
            try
            {
                // Attempt to acquire the lock for up to 500 milliseconds
                Monitor.TryEnter(_lockObject, 1, ref acquiredLock);

                if (acquiredLock)
                {
                    Console.WriteLine($"Thread {threadId}: Lock acquired. Counter before: {_counter}");
                    _counter++;
                    Thread.Sleep(100); // Simulate some work
                    Console.WriteLine($"Thread {threadId}: Lock released. Counter after: {_counter}");
                }
                else
                {
                    Console.WriteLine($"Thread {threadId}: Could not acquire lock within the timeout.");
                }
            }
            finally
            {
                if (acquiredLock)
                {
                    Monitor.Exit(_lockObject);
                }
            }
        }

        public static void MonitorTryEnterExampleMain()
        {
            MonitorTryEnterExample example = new MonitorTryEnterExample();

            // Create multiple threads to access the shared resource
            for (int i = 0; i < 5; i++)
            {
                int threadId = i;
                Thread thread = new Thread(() => example.AccessResourceWithTryEnter(threadId));
                thread.Start();
            }

            Console.ReadKey();
        }
    }
}
