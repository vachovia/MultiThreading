#if NET9_0_OR_GREATER
// global using counterLock = System.Threading.Lock;
#else
// global using counnterLock = System.Object;
#endif
namespace BasicSyntax
{
    internal class ExclusiveLock
    {
        static int counter = 0;
        static object counterLock = new();
        // System.Threading.Lock

        static void IncrementCounter()
        {
            for (int i = 0; i < 100000; i++)
            {
                // operation is divisible but lock makes it atomic
                // executes only one thread at a time because of exclusive lock
                // embeds try/catch/finally mechanizm to ensure lock is released
                lock (counterLock)
                {
                    counter = counter + 1;
                }                    
            }
        }

        public static void Exec()
        {
            Thread thread1 = new Thread(IncrementCounter);
            Thread thread2 = new Thread(IncrementCounter);

            thread1.Start();
            thread2.Start();
            
            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Final counter value is: {counter}");

            Console.ReadLine();
        }
    }
}
