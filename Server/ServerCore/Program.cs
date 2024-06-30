namespace ServerCore
{
    internal class Program
    {
        // 인자값으로 넣어준 Func은 스레드가 새로 실행될 때마다 ThreadName 변수를 새로 만들어주는 것이 아니라,
        // 한 번이라도 ThreadName.Value가 셋팅이 안 되었다면 해당 Func을 실행해서 ThreadName.Value를 셋팅해주고
        // 만약 셋팅이 되어 있었다면 셋팅된 쓰레드 이름 그대로 사용하겠다는 것.
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => { return $"My Name is {Thread.CurrentThread.ManagedThreadId}"; });

        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated;
            if (repeat)
                Console.WriteLine(ThreadName.Value + " (repeat)");
            else
                Console.WriteLine(ThreadName.Value);
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            // Task를 직접 만들지 않고 ThreadPool에서 직접 필요한 만큼 꺼내와서 활용하는 Parallel클래스.
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            ThreadName.Dispose();
        }
    }
}
