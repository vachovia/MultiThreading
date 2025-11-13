namespace BasicSyntax
{
    internal class ThreadCreation
    {
        static void WriteThreadId()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: {Thread.CurrentThread.Name}");
                Thread.Sleep(50);
            }
        }

        public static void ThreadCreationExample()
        {
            Thread thread1 = new Thread(WriteThreadId);
            Thread thread2 = new Thread(WriteThreadId);

            thread1.Name = "Thread1";
            thread2.Name = "Thread2";
            Thread.CurrentThread.Name = "Main thread";

            thread1.Priority = ThreadPriority.Highest;
            thread2.Priority = ThreadPriority.Lowest;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            thread1.Start();
            thread2.Start();

            WriteThreadId();

            Console.ReadLine();
        }
    }
}
