namespace BasicSyntax
{
    internal class InterlockedExample
    {
        static int counter = 0;

        static void IncrementCounter()
        {
            for (int i = 0; i < 100000; i++)
            {
                // Same read-modify-write as file 5, but no lock needed.
                // Interlocked.Increment does read + add + write as ONE atomic
                // CPU instruction, so threads never block - they just can't be
                // interrupted mid-increment. Cheaper than lock, but only works
                // for a single variable.
                Interlocked.Increment(ref counter);
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

// Both 5 and 5.1 print the correct 200000; the difference is how. File 5 makes the increment safe by excluding other threads (they wait);
// file 5.1 makes it safe by making the increment itself indivisible (nobody waits).
// For a single counter, 5.1 is the lighter, faster choice — which is exactly the takeaway from our discussion.