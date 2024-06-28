namespace ServerCore
{
    class Lock
    {
        // AutoResetEvent : 커널단의 관리자가 관리하고 있는 bool변수.
        // param : true는 문을 연 상태로 시작할 것인가? 닫은 상태로 시작할 것인가?에 대한 bool값임.
        // Auto라는 단어의 뜻은 하나가 지나가면 자동으로 문을 닫아준다는 클래스가 된다.
        ManualResetEvent _available = new ManualResetEvent(true);

        public void Acquire()
        {
            // 입장 시도 -> _available의 bool변수가 false로 바뀐다. _available.Reset()함수가 포함되어 있음.
            _available.WaitOne();

            // _available.Reset(); // bool = false
        }

        public void Release()
        {
            _available.Set(); // 원상 복귀를 하는 코드 즉, 맨 처음에 true로 설정했으므로 true로 돌려준다.
        }
    }

    internal class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();

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
