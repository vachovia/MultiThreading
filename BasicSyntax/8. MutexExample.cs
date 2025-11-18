using System.Reflection;

namespace BasicSyntax
{
    public class MutexExample
    {
        public static void MutexDemo()
        {
            // find in the output folder - bin/Debug/net8.0/counter.txt
            string filePath = "counter.txt";

            // Global mutex to work across processes - we name it uniquely
            // Note: Mutex is heavier than Monitor or lock, so use it only when necessary
            // lock is used for thread synchronization within the same process
            using (var mutex = new Mutex(false, $"GlobalFileMutex:{filePath}"))
            {
                for (int i = 0; i < 10000; i++)
                {
                    mutex.WaitOne();
                    try
                    {
                        int counter = ReadCounter(filePath);
                        counter++;
                        WriteCounter(filePath, counter);
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
            // With Mutex we ensure that even if multiple processes run this code simultaneously - result is 20000
            Console.WriteLine("Process finished.");
            Console.ReadLine();

        }

        static int ReadCounter(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();
                return string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
            }
        }

        static void WriteCounter(string filePath, int counter)
        {
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(counter);
            }
        }
    }
}
