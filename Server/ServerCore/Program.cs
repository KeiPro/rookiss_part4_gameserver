namespace ServerCore
{
    internal class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++)
                Console.WriteLine("Hello Thread!");
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1); // 최소 1개
            ThreadPool.SetMaxThreads(5, 5); // 최대 5개

            for (int i = 0; i < 5; i++)
            {
                // LongRunning옵션을 사용하면 -> 워커 쓰레드에서 관리가 되지 않도록 하는 처리가 된다.
                // 해당 옵션을 사용하지 않았다면 워커 쓰레드에서 관리가 되기 때문에 
                Task t = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning); 
                t.Start();
            }

            ThreadPool.QueueUserWorkItem(MainThread);

            //Thread t = new Thread(MainThread); 
            //t.Name = "Test Thread";
            //t.IsBackground = true; // 백그라운드에서 실행한다. Main함수가 끝이나면 쓰레드도 종료가 된다.
            //t.Start();
            //Console.WriteLine("Wating for Thread!");
            //t.Join(); // 쓰레드가 종료가 되면 넘어간다. 즉, 쓰레드가 실행되는 동안에는 계속 유지된다.
            //Console.WriteLine("Hello World!");
            while (true) ;
        }
    }
}
