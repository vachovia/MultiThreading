namespace BasicSyntax
{
    internal class ThreadsSync
    {
        static int counter = 0;

        static void IncrementCounter()
        {
            for (int i = 0; i < 100000; i++)
            {
                counter = counter + 1;
                // compiler makes two operations and operation is not atomic
                // this is why result is not 200000 
                // var temp = counter;
                // counter = temp + 1;
            }
        }

        static public void Exec()
        {
            Thread thread1 = new Thread(IncrementCounter);
            Thread thread2 = new Thread(IncrementCounter);

            thread1.Start(); // thread1.Join(); - in this case we wait for thread1 to finish before starting thread2 and result will be 200000
            thread2.Start(); // thread2.Join();
            // no blocking call to wait for threads to finish - result is not 200000 - it is race condition
            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Final counter value is: {counter}");

            Console.ReadLine();
        }
    }
}
