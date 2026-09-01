using System.Diagnostics;

namespace BasicSyntax
{
    internal class ThreadCreation
    {
        // Stop flag for the busy-spin threads. volatile so every thread sees the
        // write promptly without locking.
        private static volatile bool _keepSpinning;

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

            // WriteThreadId();

            thread1.Start();
            thread2.Start();

            WriteThreadId();

            Console.ReadLine();
        }

        public static void PriorityAffinityDemo()
        {
            ///////////////////////////////////////////////////////////////////////
            // ThreadPriority only changes anything when threads actually COMPETE
            // for a CPU. With Thread.Sleep and plenty of cores, the threads never
            // contend, so priority looks like it does nothing.
            //
            // Here we force the gap to show:
            //   1. Pin the whole process to a SINGLE core, so all threads share
            //      one CPU and must fight for it.
            //   2. Spin several Highest- and Lowest-priority threads in a tight
            //      loop (no sleeping) for a fixed window.
            //   3. Compare how many iterations each priority group completed.
            // The Highest threads get far more CPU time, so their total wins big.
            //
            // The main thread sets the run window: it sleeps for the duration,
            // then clears the stop flag. A thread waking from Thread.Sleep gets a
            // scheduler priority boost, so the main thread reliably preempts the
            // spinners to stop them even while they saturate the single core.
            ///////////////////////////////////////////////////////////////////////

            const int threadsPerPriority = 4;
            TimeSpan runFor = TimeSpan.FromSeconds(3);

            Console.WriteLine($"Pinning the process to a single core and spinning " +
                $"{threadsPerPriority} Highest + {threadsPerPriority} Lowest threads for " +
                $"{runFor.TotalSeconds} seconds...");
            Console.WriteLine("(no output until the measurement window finishes)");

            // Pin the process to CPU 0 only (bit 0 set). Windows-only API.
            // Save the original mask so we can restore normal scheduling afterward.
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr originalAffinity = IntPtr.Zero;
            if (OperatingSystem.IsWindows())
            {
                originalAffinity = currentProcess.ProcessorAffinity;
                currentProcess.ProcessorAffinity = (IntPtr)1;
            }

            var highCounters = new long[threadsPerPriority];
            var lowCounters = new long[threadsPerPriority];
            var threads = new List<Thread>();

            _keepSpinning = true;

            for (int i = 0; i < threadsPerPriority; i++)
            {
                int index = i; // capture a fresh copy per iteration

                Thread high = new Thread(() => Spin(highCounters, index))
                {
                    Name = $"High-{index}",
                    Priority = ThreadPriority.Highest
                };

                Thread low = new Thread(() => Spin(lowCounters, index))
                {
                    Name = $"Low-{index}",
                    Priority = ThreadPriority.Lowest
                };

                threads.Add(high);
                threads.Add(low);
            }

            foreach (Thread t in threads)
            {
                t.Start();
            }

            // The main thread sleeps for the measurement window, then clears the
            // flag. Waking from Sleep grants it a scheduler boost, so it preempts
            // the spinners to stop them even on the saturated core.
            Thread.Sleep(runFor);
            _keepSpinning = false;

            foreach (Thread t in threads)
            {
                t.Join();
            }

            // Restore normal scheduling across all cores.
            if (OperatingSystem.IsWindows())
            {
                currentProcess.ProcessorAffinity = originalAffinity;
            }

            long highTotal = highCounters.Sum();
            long lowTotal = lowCounters.Sum();

            Console.WriteLine();
            Console.WriteLine($"Highest-priority threads: {highTotal:N0} total iterations");
            Console.WriteLine($"Lowest-priority threads:  {lowTotal:N0} total iterations");

            if (lowTotal == 0)
            {
                Console.WriteLine("The Lowest-priority threads were COMPLETELY starved: with the core");
                Console.WriteLine("saturated by Highest threads, they never got scheduled at all.");
                Console.WriteLine("(This is exactly the starvation/priority-inversion risk that makes");
                Console.WriteLine(" hard-coded thread priorities dangerous in real applications.)");
            }
            else
            {
                Console.WriteLine($"Highest did roughly {(double)highTotal / lowTotal:N1}x the work of Lowest.");
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        // Tight, CPU-bound loop. Counts locally and only writes the result back
        // once at the end to avoid false sharing on the counters array.
        static void Spin(long[] counters, int index)
        {
            long count = 0;
            while (_keepSpinning)
            {
                count++;
            }
            counters[index] = count;
        }
    }
}
