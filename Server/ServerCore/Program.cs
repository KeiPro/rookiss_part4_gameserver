namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;

        public void Acquire()
        {
            while (true)
            {
                // CAS (Compare - And - Swap)
                int expected = 0;
                int desired = 1;
                
                // locked와 expected의 값을 비교해서 같으면 _locked의 값을 desired값으로 바꿔준다.
                if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
                    break;


                // Thread.Sleep(1); // 무조건 휴식 => 무조건 1ms 정도 쉬겠다는 뜻.
                // Thread.Sleep(0); // 조건부 양보 => 나보다 우선순위가 낮은 얘들한테는 양보 불가 => 우선순위가 나보다 같거나 높은 쓰레드가 없으면 다시 본인한테 돌아옴.
                Thread.Yield(); // 관대한 양보 => 관대하게 양보할테니, 지금 실행이 가능한 쓰레드가 있으면 실행하세요 => 실행 가능한 얘가 없으면 남은 시간을 보인한테 소진.
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
            for (int i = 0; i < 1000000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
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
