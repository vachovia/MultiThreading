using System.Diagnostics;

namespace BasicSyntax
{
    internal class DivideAndConquer
    {
        static int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        static int SumSegment(int start, int end)
        {
            int segmentSum = 0;

            for (int i = start; i < end; i++)
            {
                Thread.Sleep(100);
                segmentSum += array[i];
            }
            Console.WriteLine($"SumSegment function result {segmentSum}");

            return segmentSum;
        }

        public static void DivideAndConquerExample()
        {
            int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;

            // var startTime = DateTime.Now;
            var timer = Stopwatch.StartNew();

            int numofThreads = 4;
            int segmentLength = array.Length / numofThreads;

            Thread[] threads = new Thread[numofThreads];

            threads[0] = new Thread(() => { sum1 = SumSegment(0, segmentLength); });
            threads[1] = new Thread(() => { sum2 = SumSegment(segmentLength, 2 * segmentLength); });
            threads[2] = new Thread(() => { sum3 = SumSegment(2 * segmentLength, 3 * segmentLength); });
            threads[3] = new Thread(() => { sum4 = SumSegment(3 * segmentLength, array.Length); });

            foreach (var thread in threads) { thread.Start(); } // Start all threads but without waiting for them to complete

            foreach (var thread in threads) { thread.Join(); } // Wait for all threads to complete - otherwise, the main thread may proceed before they finish

            timer.Stop();

            Console.WriteLine($"The sum is {sum1 + sum2 + sum3 + sum4}");
            Console.WriteLine($"The time it takes: {timer.ElapsedMilliseconds}");

            Console.ReadLine();
        }
    }
}
