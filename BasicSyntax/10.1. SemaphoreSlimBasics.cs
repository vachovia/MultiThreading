using System.Diagnostics;

namespace BasicSyntax
{
    // A semaphore is just a LOCK WITH N SLOTS (a counter of permits).
    //
    //   lock / Monitor          -> 1 thread inside at a time
    //   SemaphoreSlim(3, 3)     -> 3 threads inside at a time
    //   SemaphoreSlim(1, 1)     -> same behaviour as lock
    //
    // Wait()    takes a slot. If none are free, the thread BLOCKS until one is released.
    // Release() gives a slot back and wakes up one waiting thread.
    //
    // 8 workers, 3 slots, each worker "works" for 2 seconds:
    // the output comes out in waves of 3 - that is the whole idea.
    internal class SemaphoreSlimBasics
    {
        // initialCount: how many slots are free at the start
        // maxCount:     the hard ceiling - an extra Release() throws SemaphoreFullException
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);

        private static Stopwatch _clock = new Stopwatch();

        private static void Log(string message) => Console.WriteLine($"[{_clock.Elapsed.TotalSeconds,4:0.0}s] {message}");

        public static void Exec()
        {
            _clock.Start();

            Thread[] workers = new Thread[8];

            for (int i = 0; i < workers.Length; i++)
            {
                int id = i + 1; // copy the loop variable - the lambda captures it
                workers[i] = new Thread(() => Worker(id));
                workers[i].Start();
            }

            foreach (Thread worker in workers)
            {
                worker.Join();
            }

            Log("All workers finished.");

            Console.ReadLine();
        }

        private static void Worker(int id)
        {
            Log($"Worker {id} is waiting for a free slot...");

            // Wait() stays OUTSIDE the try: if it throws we never took a slot,
            // so the finally must not give one back. Same idea as 'lockAcquired' in 9.
            _semaphore.Wait();

            try
            {
                // CurrentCount = slots still free. Read it right after entering
                // and you will never see it go below 0.
                Log($"    Worker {id} ENTERED.  Free slots left: {_semaphore.CurrentCount}");

                Thread.Sleep(2000); // simulate work
            }
            finally
            {
                _semaphore.Release(); // always give the slot back, even on exception
                Log($"    Worker {id} LEFT.     Free slots now:  {_semaphore.CurrentCount}");
            }
        }
    }
}
