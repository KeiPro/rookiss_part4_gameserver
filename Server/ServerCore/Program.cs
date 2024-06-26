namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;

        public void Acquire()
        {
            while (true)
            {
                // Interlocked.Exchange의 반환값은 오리지널 값이다. 즉, 내가 1로 바꾸겠다고 하기전의 값을 말한다.
                int original = Interlocked.Exchange(ref _locked, 1);
                if (original == 0) // 이전의 값이 0이라면 내 코드로 인해서 1로 바뀐것이기 때문에 break를 한다는 뜻이다.
                    break;
            }
        }

        public void Release()
        {
            _locked = 0;
        }
    }

    internal class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);
            Console.WriteLine(_num);
        }
    }
}
