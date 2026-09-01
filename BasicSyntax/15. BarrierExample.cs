namespace BasicSyntax
{
    public class BarrierExample
    {
        private Barrier _barrier;

        public BarrierExample(Barrier barrier)
        {
            _barrier = barrier;
        }

        public void Exec()
        {
            ///////////////////////////////////////////////////////////////////////
            // Barrier lets a fixed set of threads work through several phases
            // together: each thread does its part of a phase, then calls
            // SignalAndWait() and blocks until EVERY participant has arrived.
            // Only then does the next phase begin. The post-phase action runs
            // once per phase, on the last thread to arrive, before the others
            // are released.
            ///////////////////////////////////////////////////////////////////////

            // Start 3 worker threads - must match the participant count the
            // Barrier was created with (see Program.cs).
            var workers = new List<Thread>();
            for (int i = 0; i < 3; i++)
            {
                Thread workerThread = new Thread(Worker);
                workerThread.Name = $"Worker {i + 1}";
                workerThread.Start();
                workers.Add(workerThread);
            }

            // Wait for every worker to finish before returning. Without this,
            // Exec() returns immediately, the caller's 'using' block disposes
            // the Barrier, and the still-running workers throw
            // ObjectDisposedException when they call SignalAndWait().
            foreach (Thread worker in workers)
            {
                worker.Join();
            }
        }

        void Worker()
        {
            for (int phase = 1; phase <= 3; phase++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} is working on phase {phase}.");

                // Simulate uneven amounts of work so threads finish at different times
                Thread.Sleep(500 * Thread.CurrentThread.ManagedThreadId % 2000 + 500);

                Console.WriteLine($"{Thread.CurrentThread.Name} finished phase {phase}, waiting at the barrier.");

                // Block here until all participants have signaled for this phase.
                // The post-phase action (configured in Program.cs) runs once before
                // everyone is released into the next phase.
                _barrier.SignalAndWait();
            }

            Console.WriteLine($"{Thread.CurrentThread.Name} completed all phases.");
        }
    }
}
