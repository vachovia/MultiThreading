using BasicSyntax;

/*1.   ThreadCreation.ThreadCreationExample();*/
/*2.   DivideAndConquer.DivideAndConquerExample();*/
/*3.   WebServerSimulation.Run();*/
/*4.   ThreadsSync.Exec();*/
/*5.   ExclusiveLock.Exec();*/
/*6.   TicketsBooking.Run();*/
/*7.   MonitorExample.Counter(); MonitorExample.TicketsBooking();*/
/*7a.  MonitorTryEnterExample.MonitorTryEnterExampleMain();*/
/*8.   MutexExample.MutexDemo();*/

/*10.  
       // Semaphore is global within the process, but SemaphoreSlim is local to the process
       using SemaphoreSlim semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);
       var sem = new SemaphoreExample(semaphore);
       sem.SemaphoreExampleExec(); 
*/
/*11.
       // AutoResetEvent 
       using AutoResetEvent autoResetEvent = new AutoResetEvent(false);
       var arEvent = new AutoResetEventExample(autoResetEvent);
       // arEvent.AutoResetEventSingleExec();
       arEvent.AutoResetEventMultyExec();
*/
/*12.
       // ManualResetEvent 
       using ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
       var mrEvent = new ManualResetEventExample(manualResetEvent);
       mrEvent.ManualResetEventExec();
 */
/*13.
       // TwoWaySignalingInProducer 
       using ManualResetEventSlim consumeEvent = new ManualResetEventSlim(false);
       using ManualResetEventSlim produceEvent = new ManualResetEventSlim(true);
       var producerExample = new TwoWaySignalingInProducer(consumeEvent, produceEvent);
       producerExample.TwoWaySignalingInProducerExec();
*/
/*14.  DeadlockExample.Exec();*/