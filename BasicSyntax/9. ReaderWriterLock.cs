namespace BasicSyntax
{
    public class ReaderWriterLockExample
    {
        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private static Dictionary<int, string> _cache = new Dictionary<int, string>();

        public static void Add(int key, string value)
        {
            bool lockAcquired = false; // track if we acquired the lock

            try
            {
                _lock.EnterWriteLock(); // only one thread can write at a time - exclusive lock !!!!!!!!!

                lockAcquired = true;

                _cache[key] = value; // it is not atomic
            }
            finally
            {
                if (lockAcquired) _lock.ExitWriteLock();
            }
        }

        public static string? Get(int key)
        {
            bool lockAcquired = false; // track if we acquired the lock

            try
            {
                _lock.EnterReadLock(); // multiple threads can read simultaneously - shared lock !!!!!!!!!

                lockAcquired = true;

                return _cache.TryGetValue(key, out string? value) ? value : null;
            }
            finally
            {
                if (lockAcquired) _lock.ExitReadLock();
            }
        }
    }
}
