namespace BasicSyntax
{
    public class TwoWaySignalingInProducer
    {
        int consumerCount = 0;
        object lockConsumerCount = new object();
        Queue<int> queue = new Queue<int>();
        ManualResetEventSlim _consumeEvent;
        ManualResetEventSlim _produceEvent;

        public TwoWaySignalingInProducer(ManualResetEventSlim consumeEvent, ManualResetEventSlim produceEvent)
        {
            _consumeEvent = consumeEvent;
            _produceEvent = produceEvent;
        }

        public void TwoWaySignalingInProducerExec()
        {

            Thread[] consumerThreads = new Thread[3];

            for (int i = 0; i < 3; i++)
            {
                consumerThreads[i] = new Thread(Consume);
                consumerThreads[i].Name = $"Consumer {i + 1}";
                consumerThreads[i].Start();
            }

            // Producer
            while (true)
            {
                _produceEvent.Wait();
                _produceEvent.Reset();

                Console.WriteLine("To produce, enter 'p'");
                var input = Console.ReadLine() ?? "";

                if (input.ToLower() == "p")
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        queue.Enqueue(i);
                        Console.WriteLine($"Produced: {i}");
                    }

                    _consumeEvent.Set();
                }
            }

        }

        // Consumer's behavior
        void Consume()
        {
            while (true)
            {
                _consumeEvent.Wait();

                while (queue.TryDequeue(out int item))
                {
                    // work on the items produced
                    Thread.Sleep(500);
                    Console.WriteLine($"Consumed: {item} from thread: {Thread.CurrentThread.Name}");
                }

                lock (lockConsumerCount)
                {
                    consumerCount++;

                    if (consumerCount == 3)
                    {
                        _consumeEvent.Reset();
                        _produceEvent.Set();
                        consumerCount = 0;

                        Console.WriteLine("****************");
                        Console.WriteLine("**** More Please! *****");
                        Console.WriteLine("****************");
                    }
                }
            }
        }
    }
}
